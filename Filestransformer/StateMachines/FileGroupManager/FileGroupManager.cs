using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using Filestransformer.StateMachines.TransformationDispatcher;
using Microsoft.PSharp;
using System.IO;
using Filestransformer.StateMachines.TransformationDispatcher.Events;

namespace Filestransformer.StateMachines.FileGroupManager
{
    public class FileGroupManager : FileGroupManagerBase
    {
        // configuration related
        private ILogger logger;
        private int fileGroups;
        private int maximumParallelFileTransformations;
        private string inputDirectory;
        private string outputDirectory;

        // file transformation related
        private Queue<string> pendingTranformations;
        private Dictionary<string, MachineId> activeTransformations;

        protected override void InitializeFileGroupManager()
        {
            var config = ReceivedEvent as eFileGroupManagerConfig;
            logger = config.Logger;
            fileGroups = config.FileGroups;
            maximumParallelFileTransformations = config.MaximumParallelFileTransformations;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;

            pendingTranformations = new Queue<string>();
            activeTransformations = new Dictionary<string, MachineId>(StringComparer.OrdinalIgnoreCase);

            logger.WriteLine($"Initialized {nameof(FileGroupManager)} machine successfully.");
        }

        protected override void DispatchPendingFileTransformationJobRequests()
        {
            // TODO cap this to max limit, for now drain the list
            while (pendingTranformations.Count > 0)
            {
                string groupName, unqualifiedFileName;

                var fileName = pendingTranformations.Peek();
                FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fileName,
                        out groupName, out unqualifiedFileName);
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    logger.WriteLine($"File {fileName} is in invalidformat, missing group name, " +
                        $"will skip processing this request");
                }
                else
                {
                    // create dispatcher machine if not exists
                    if (!activeTransformations.ContainsKey(groupName))
                    {
                        var configEvent = new eFileTransformationDispatcherConfig(logger, groupName, maximumParallelFileTransformations,
                            inputDirectory, outputDirectory);

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

        protected override void HandleAddFilesToTransform()
        {
            var addFilesEvent = ReceivedEvent as eAddFilesToTransform;
            foreach (var fileName in addFilesEvent.FullyQualifiedFileNames)
            {
                HandleAddFileToTransform(fileName);
            }
        }

        protected override void HandleAddFileToTransform()
        {
            var addFileEvent = ReceivedEvent as eAddFileToTransform;
            HandleAddFileToTransform(addFileEvent.FullyQualifiedFileName);
        }

        private void HandleAddFileToTransform(string fullyQualifiedFileName)
        {
            //logger.WriteLine($"Adding file to dispatcher {fullyQualifiedFileName}");
            pendingTranformations.Enqueue(fullyQualifiedFileName);
        }

        #endregion
    }
}
