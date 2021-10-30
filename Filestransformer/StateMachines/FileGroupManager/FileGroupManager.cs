using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using System.IO;

namespace Filestransformer.StateMachines.FileGroupManager
{
    public class FileGroupManager : FileGroupManagerBase
    {
        private ILogger logger;
        private int fileGroups;
        private int maximumParallelFileTransformations;
        private string inputDirectory;
        private string outputDirectory;

        protected override void InitializeFileGroupManager()
        {
            var config = ReceivedEvent as eFileGroupManagerConfig;
            logger = config.Logger;
            fileGroups = config.FileGroups;
            maximumParallelFileTransformations = config.MaximumParallelFileTransformations;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;

            logger.WriteLine($"Initialized {nameof(FileGroupManager)} machine successfully.");
        }

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
            string groupName, unqualifiedFileName;
            FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fullyQualifiedFileName,
                out groupName, out unqualifiedFileName);

            logger.WriteLine($"Adding file to transform, group: {groupName}, filename: {unqualifiedFileName}");
        }
    }
}
