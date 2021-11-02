using Filestransformer.Settings;
using Filestransformer.StateMachines.FileGroupManager;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.StateMachines.FileSystemWatcher;
using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System;
using System.Threading;

namespace Filestransformer.Actor
{
    /// <summary>
    /// FileTransformerActor
    /// Orchestrator for file transformation workflow
    /// </summary>
    public class FiletransformerActor : IActor
    {
        private readonly Setting settings;
        private static AutoResetEvent WaitEvent = new AutoResetEvent(false);

        private IMachineRuntime psharpRuntime;
        private MachineId filesystemWatcher;
        private MachineId fileGroupManager;

        public FiletransformerActor(Setting settings)
        {
            this.settings = settings;
            psharpRuntime = PSharpRuntime.Create(Microsoft.PSharp.Configuration.Create());
            psharpRuntime.OnFailure += delegate (Exception ex)
            {
                string errorMessage = Environment.NewLine + $"An error occurred in one or more PSharp state machines: {ex.ToString()}" +
                    Environment.NewLine + Environment.NewLine + "The program cannot continue any further, press enter to exit the program.";
                LoggerFactory.GetLogger().WriteLine(errorMessage, LogLevelContext.Error);
                Console.ReadKey();
                Environment.Exit(1);
            };

            filesystemWatcher = psharpRuntime.CreateMachine(typeof(FileSystemWatcher));
            fileGroupManager = psharpRuntime.CreateMachine(typeof(FileGroupManager));
        }

        public void Run()
        {
            var logger = LoggerFactory.GetLogger();

            logger.WriteLine("");
            logger.WriteLine("Running file transformer with the following settings: ");
            logger.WriteLine($"MaximumParallelFileTransformationsPerGroup: {settings.MaxParallelFileTransformationsPerGroup}");
            logger.WriteLine($"InputDirectoryPath: {settings.InputDirectoryPath}");
            logger.WriteLine($"OutputDirectoryPath: {settings.OutputDirectoryPath}");
            logger.WriteLine($"FileChunkSizeToReadInBytes: {settings.FileChunkSizeToReadInBytes}");
            logger.WriteLine($"FileEncoding: {settings.FileEncoding}");

            // create input and output directories if not existing
            CreateDirectoryIfNotExists(settings.InputDirectoryPath);
            CreateDirectoryIfNotExists(settings.OutputDirectoryPath);

            // watch input directory to detect for new files
            psharpRuntime.SendEvent(filesystemWatcher, new eFileSystemWatcherConfig(logger, settings.InputDirectoryPath, 
                fileGroupManager));

            // initialize filegroupmanager machine
            psharpRuntime.SendEvent(fileGroupManager, new eFileGroupManagerConfig(logger, settings.MaxParallelFileTransformationsPerGroup, 
                settings.InputDirectoryPath, settings.OutputDirectoryPath,
                settings.FileChunkSizeToReadInBytes, settings.FileEncoding));

            // block forever
            WaitEvent.WaitOne();
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            var logger = LoggerFactory.GetLogger();
            try
            {
                if (System.IO.Directory.Exists(path)) { return; }
                System.IO.Directory.CreateDirectory(path);
                logger.WriteLine($"Created directory '{path}' successfully.");
            }
            catch (Exception ex)
            {
                logger.WriteLine("");
                logger.WriteLine($"An error occurred while creating directory {path}: {ex.ToString()}");
                throw;
            }
        }
    }
}
