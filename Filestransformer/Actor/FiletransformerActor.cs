using Filestransformer.Settings;
using Filestransformer.StateMachines.FileSystemWatcher;
using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System;
using System.Threading;
namespace Filestransformer.Actor
{
    public class FiletransformerActor : IActor
    {
        private readonly Setting settings;
        private static AutoResetEvent WaitEvent = new AutoResetEvent(false);

        private IMachineRuntime psharpRuntime;
        private MachineId filesystemWatcher;

        public FiletransformerActor(Setting settings)
        {
            this.settings = settings;
            psharpRuntime = PSharpRuntime.Create(Microsoft.PSharp.Configuration.Create());
            psharpRuntime.OnFailure += delegate (Exception ex)
            {
                string errorMessage = Environment.NewLine + $"An error occurred in one or more PSharp state machines: {ex.ToString()}" +
                    Environment.NewLine + Environment.NewLine + "The program cannot continue any further, press enter to exit the program.";
                LoggerFactory.GetLogger().WriteLine(errorMessage);
                Console.ReadKey();
                Environment.Exit(1);
            };

            filesystemWatcher = psharpRuntime.CreateMachine(typeof(FileSystemWatcher));
        }

        public void Run()
        {
            var logger = LoggerFactory.GetLogger();

            logger.WriteLine("");
            logger.WriteLine("Running file transformer with the following settings: ");
            logger.WriteLine($"FileGroups: {settings.FileGroups}");
            logger.WriteLine($"MaximumParallelFileTransformations: {settings.MaxParallelFileTransformations}");
            logger.WriteLine($"InputDirectoryPath: {settings.InputDirectoryPath}");
            logger.WriteLine($"OutputDirectoryPath: {settings.OutputDirectoryPath}");

            // watch input directory to detect for new files
            //
            psharpRuntime.SendEvent(filesystemWatcher, new eFileSystemWatcherConfig(settings.InputDirectoryPath, logger));

            // block forever
            WaitEvent.WaitOne();
        }
    }
}
