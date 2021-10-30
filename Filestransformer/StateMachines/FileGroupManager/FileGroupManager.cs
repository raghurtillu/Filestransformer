using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using System.IO;

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
        private Queue<string> pendingFileTransformationJobRequests;

        protected override void InitializeFileGroupManager()
        {
            var config = ReceivedEvent as eFileGroupManagerConfig;
            logger = config.Logger;
            fileGroups = config.FileGroups;
            maximumParallelFileTransformations = config.MaximumParallelFileTransformations;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;

            if (fileGroups <= 0)
            {
                throw new ArgumentException($"Invalid value '{fileGroups}' specified.");
            }
            else if (maximumParallelFileTransformations <= 0)
            {
                throw new ArgumentException($"Invalid value '{maximumParallelFileTransformations}' specified.");
            }

            pendingFileTransformationJobRequests = new Queue<string>();
            logger.WriteLine($"Initialized {nameof(FileGroupManager)} machine successfully.");
        }

        protected override void DispatchPendingFileTransformationJobRequests()
        {
            string groupName, unqualifiedFileName;

            // TODO cap this to max limit, for now drain the list
            while (pendingFileTransformationJobRequests.Count > 0)
            {
                var fileName = pendingFileTransformationJobRequests.Peek();
                pendingFileTransformationJobRequests.Dequeue();

                FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fileName,
                    out groupName, out unqualifiedFileName);
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    logger.WriteLine($"File {fileName} is in invalidformat, missing group name, " +
                        $"will skip processing this request");
                    continue;
                }
                else
                {
                    logger.WriteLine($"Dispatching {fileName} request");
                }
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
            logger.WriteLine($"Adding file to transform request {fullyQualifiedFileName}");
            pendingFileTransformationJobRequests.Enqueue(fullyQualifiedFileName);
        }

        #endregion
    }
}
