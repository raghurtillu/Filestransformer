using Filestransformer.Settings;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.StateMachines.TransformationDispatcher;
using Filestransformer.StateMachines.TransformationDispatcher.Events;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using Microsoft.PSharp;
using System;
using System.Collections.Generic;

namespace Filestransformer.StateMachines.FileGroupManager
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class FileGroupManager : FileGroupManagerBase
    {
        // configuration related
        private ILogger logger;
        private int maximumParallelFileTransformations;
        private string inputDirectory;
        private string outputDirectory;
        private int fileChunkSizeToReadInBytes;
        private FileEncoding fileEncoding;

        // file transformation related
        private Queue<string> pendingTranformations;
        private Dictionary<string, MachineId> activeTransformations;

        protected override void InitializeFileGroupManager()
        {
            var config = ReceivedEvent as eFileGroupManagerConfig;
            logger = config.Logger;
            maximumParallelFileTransformations = config.MaximumParallelFileTransformations;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;
            fileChunkSizeToReadInBytes = config.FileChunkSizeToReadInBytes;
            fileEncoding = config.FileEncoding;

            pendingTranformations = new Queue<string>();
            activeTransformations = new Dictionary<string, MachineId>(StringComparer.OrdinalIgnoreCase);

            logger.WriteLine($"Initialized {nameof(FileGroupManager)} machine successfully.");
        }

        /// <summary>
        /// Dispatches transformation file request to <seealso cref="TransformationDispatcher"/>
        /// </summary>
        protected override void DispatchPendingFileTransformationJobRequests()
        {
            // TODO: Have a cap around global maximum number of pending + active requests in the system; for now drain the pending requests list
            //
            while (pendingTranformations.Count > 0)
            {
                string groupName, unqualifiedFileName;

                // TODO: for v2 can have a Policy enforcement around pending requests e.g. Priority, Shortest time to transform etc; for now simply do a FIFO
                //
                var fileName = pendingTranformations.Peek();
                FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fileName,
                        out groupName, out unqualifiedFileName);
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    logger.WriteLine($"File {fileName} is in invalidformat, missing group name, " +
                        $"will skip processing this request", LogLevelContext.Warning);
                }
                else
                {
                    // create dispatcher machine if not exists
                    if (!activeTransformations.ContainsKey(groupName))
                    {
                        var configEvent = new eFileTransformationDispatcherConfig(logger, groupName, maximumParallelFileTransformations,
                            inputDirectory, outputDirectory, fileChunkSizeToReadInBytes, fileEncoding);

                        activeTransformations[groupName] = this.CreateMachine(typeof(FileTransformationDispatcher));
                        this.Send(activeTransformations[groupName], configEvent);
                    }

                    // send transformation request to the dispatcher
                    this.Send(activeTransformations[groupName], new eAddFileToTransform(fileName));
                }

                pendingTranformations.Dequeue();
            }
        }

        #region AddingFiles to transform

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void HandleAddFilesToTransform()
        {
            var addFilesEvent = ReceivedEvent as eAddFilesToTransform;
            foreach (var fileName in addFilesEvent.FullyQualifiedFileNames)
            {
                HandleAddFileToTransform(fileName);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void HandleAddFileToTransform()
        {
            var addFileEvent = ReceivedEvent as eAddFileToTransform;
            HandleAddFileToTransform(addFileEvent.FullyQualifiedFileName);
        }

        private void HandleAddFileToTransform(string fullyQualifiedFileName)
        {
            pendingTranformations.Enqueue(fullyQualifiedFileName);
        }

        #endregion
    }
}
