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
    public class FileTransformationDispatcher : FileTransformationDispatcherBase
    {
        // configuration related
        private ILogger logger;
        private string group;
        private int maximumParallelFileTransformations;
        private string inputDirectory;
        private string outputDirectory;
        private int fileChunkSizeToReadInBytes;
        private FileEncoding fileEncoding;

        // file transformation related
        private Queue<string> pendingTransformations;
        private Dictionary<string, MachineId> activeTransformations;
        private int totalSuccessful = 0;
        private int totalFailed = 0;

        protected override void InitializeFileTransformationDispatcher()
        {
            var config = ReceivedEvent as eFileTransformationDispatcherConfig;
            logger = config.Logger;
            group = config.Group;
            maximumParallelFileTransformations = config.MaximumParallelFileTransformations;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;
            fileChunkSizeToReadInBytes = config.FileChunkSizeToReadInBytes;
            fileEncoding = config.FileEncoding;

            pendingTransformations = new Queue<string>();
            activeTransformations = new Dictionary<string, MachineId>(StringComparer.OrdinalIgnoreCase);

            logger.WriteLine($"Initialized {nameof(FileTransformationDispatcher)} machine for {group} successfully.");
        }

        protected override void HandleAddFileToTransform()
        {
            var transformRequest = ReceivedEvent as eAddFileToTransform;
            logger.WriteLine($"Got request to transform file {transformRequest.FullyQualifiedFileName}");
            pendingTransformations.Enqueue(transformRequest.FullyQualifiedFileName);
        }

        protected override void DispatchPendingFileTransformationJobRequests()
        {
            int maxIntake = maximumParallelFileTransformations - activeTransformations.Count;
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

                //logger.WriteLine($"Processed {fullyQualifiedFileName} successfully.");
                pendingTransformations.Dequeue();
            }

            DisplayCurrentState();
        }

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

        protected override bool IsRunningAtFullCapacity() => !(activeTransformations.Count < maximumParallelFileTransformations);

        protected override bool HasPendingJobs() => pendingTransformations?.Count > 0;

        protected override void DisplayIdleStateMessage()
        {
            logger.WriteLine($"Dispatcher {group} is running at full capactity, will retry in 10 seconds");
            DisplayCurrentState();
        }

        protected override void SetRetryTimer()
        {
            activeTransformations.Clear();
            this.StartTimer(TimeSpan.FromMilliseconds(10000));
        }

        private string GetFileName(string fullyQualifiedFileName)
        {
            string groupName, fileName;
            FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fullyQualifiedFileName,
                out groupName, out fileName);
            return fileName;
        }

        private void DisplayCurrentState() => logger.WriteLine($"Group {group} status: " +
                $"(Active: {activeTransformations.Count}, Pending: {pendingTransformations.Count}, MaxLimit: {maximumParallelFileTransformations}, " +
                $"Total completed: {totalSuccessful + totalFailed}, Total successful: {totalSuccessful}, Total failed: {totalFailed})", LogLevelContext.Warning);

        private MachineId CreateFileTransformerMachine() =>
            this.CreateMachine(FileTransformerFactory.GetFileTransformerType(FileTransformerType.Lowercase));

    }
}
