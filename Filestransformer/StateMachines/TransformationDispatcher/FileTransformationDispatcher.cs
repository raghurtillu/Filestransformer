using Filestransformer.Settings;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.StateMachines.FileTransformers;
using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.StateMachines.TransformationDispatcher.Events;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using Microsoft.PSharp;
using System;
using System.Collections.Generic;

namespace Filestransformer.StateMachines.TransformationDispatcher
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class FileTransformationDispatcher : FileTransformationDispatcherBase
    {
        // configuration related
        private ILogger logger;
        private string group;
        private int maximumParallelFileTransformationsPerGroup;
        private string inputDirectory;
        private string outputDirectory;
        private int fileChunkSizeToReadInBytes;
        private FileEncoding fileEncoding;

        // file transformation related
        private Queue<string> pendingTransformations;
        private Dictionary<string, MachineId> activeTransformations;
        private int totalSuccessful = 0;
        private int totalFailed = 0;

        private TimeSpan TIMER_RETRY_INTERVAL = TimeSpan.FromSeconds(10);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void InitializeFileTransformationDispatcher()
        {
            var config = ReceivedEvent as eFileTransformationDispatcherConfig;
            logger = config.Logger;
            group = config.Group;
            maximumParallelFileTransformationsPerGroup = config.MaximumParallelFileTransformationsPerGroup;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;
            fileChunkSizeToReadInBytes = config.FileChunkSizeToReadInBytes;
            fileEncoding = config.FileEncoding;

            pendingTransformations = new Queue<string>();
            activeTransformations = new Dictionary<string, MachineId>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void HandleAddFileToTransform()
        {
            var transformRequest = ReceivedEvent as eAddFileToTransform;
            logger.WriteLine($"Got request to transform file {transformRequest.FullyQualifiedFileName}");
            pendingTransformations.Enqueue(transformRequest.FullyQualifiedFileName);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void DispatchPendingFileTransformationJobRequests()
        {
            int maxIntake = maximumParallelFileTransformationsPerGroup - activeTransformations.Count;
            maxIntake = Math.Min(maxIntake, pendingTransformations.Count);

            logger.WriteLine($"{group} dispatcher will process {maxIntake} request in this epoch");
            while (maxIntake-- > 0)
            {
                string fullyQualifiedFileName = pendingTransformations.Peek();
                string fileName = GetFileName(fullyQualifiedFileName);
                
                // create transformation machine if not exists
                if (!activeTransformations.ContainsKey(fileName))
                {
                    var configEvent = new eFileTransformerEvent(this.Id, logger, fullyQualifiedFileName, inputDirectory, outputDirectory,
                        fileChunkSizeToReadInBytes, fileEncoding);
                    activeTransformations[fileName] = CreateFileTransformerMachine();
                    this.Send(activeTransformations[fileName], configEvent);
                }

                pendingTransformations.Dequeue();
            }

            DisplayCurrentState();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void HandleFileTransformationResponse()
        {
            var response = ReceivedEvent as eFileTranformationResponseEvent;
            var fileName = response.FileName;
            bool transformationDone = true;
            if (response.Status == FileTransformationStatus.Success)
            {
                totalSuccessful++;
                logger.WriteLine($"Transformation for {fileName} completed successfully, " +
                    $"transformation time: {response.TimeToComplete?.ToString("G")}", LogLevelContext.Info);
            }
            else if (response.Status == FileTransformationStatus.Failed)
            {
                totalFailed++;
                logger.WriteLine($"transformation for {fileName} failed," +
                    $" reason: {response.FailureReason}", LogLevelContext.Error);
            }
            else if (response.Status == FileTransformationStatus.InProgress)
            {
                logger.WriteLine($"transformation for {fileName} is still in progress");
                transformationDone = false;
            }
            else
            {
                throw new InvalidOperationException($"Unexpected enum value for {nameof(FileTransformationStatus)}: {response.Status}");
            }

            if (transformationDone)
            {
                // remove transformation machine
                string unqualifiedFileName = GetFileName(response.FileName);
                activeTransformations.Remove(unqualifiedFileName);
            }
            
            DisplayCurrentState();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override bool IsRunningAtFullCapacity() => 
            !(activeTransformations.Count < maximumParallelFileTransformationsPerGroup);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override bool HasPendingJobs() 
            => pendingTransformations?.Count > 0;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void DisplayIdleStateMessage(bool timerExpired)
        {
            if (timerExpired)
            {
                logger.WriteLine($"Dispatcher {group} timer expired, checking if there are any pending or active transformations to process");
            }
            else
            {
                logger.WriteLine($"Dispatcher {group} is running at full capactity, will retry in {TIMER_RETRY_INTERVAL.ToString(@"ss")} seconds");
            }            
            DisplayCurrentState();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void SetRetryTimer()
        {
            // Note that is the amount of time this machine will wait before a retry
            // If we get a transformation complete response from downstream transformation machine before the retry period ends and
            // we are running < full capacity, we will pick up processsing pending transformations immediately
            //
            this.StartPeriodicTimer(TIMER_RETRY_INTERVAL, TIMER_RETRY_INTERVAL);
        }

        private string GetFileName(string fullyQualifiedFileName)
        {
            string groupName, fileName;
            FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fullyQualifiedFileName,
                out groupName, out fileName);
            return fileName;
        }

        private void DisplayCurrentState() => 
            logger.WriteLine($"Group {group} status: " +
                $"(Active: {activeTransformations.Count}, Pending: {pendingTransformations.Count}, MaxLimit: {maximumParallelFileTransformationsPerGroup}, " +
                $"Total completed: {totalSuccessful + totalFailed}, Total successful: {totalSuccessful}, Total failed: {totalFailed})", LogLevelContext.Warning);

        private MachineId CreateFileTransformerMachine() =>
            this.CreateMachine(FileTransformerFactory.GetFileTransformerType(FileTransformerType.Lowercase));

    }
}
