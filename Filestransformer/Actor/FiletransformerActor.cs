using Filestransformer.Settings;
using Filestransformer.StateMachines.FileSystemWatcher;
using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
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

            filesystemWatcher = psharpRuntime.CreateMachine(typeof(FileSystemWatcher));
        }

        public void Run()
        {
            var logger = LoggerFactory.GetLogger();

            logger.WriteLine("");
            logger.WriteLine("Running file transformer with the following settings: ");
            logger.WriteLine($"FileGroups: {settings.FileGroups}");
            logger.WriteLine($"MaximumParallelFileTransformations: {settings.MaxParallelFileTransformations}");
            logger.WriteLine($"InputFolderPath: {settings.InputFolderPath}");
            logger.WriteLine($"OututFolderPath: {settings.OutputFolderPath}");

            // watch input folder path to detect new files
            //
            psharpRuntime.SendEvent(filesystemWatcher, new eFileSystemWatcherConfig(settings.InputFolderPath, logger));

            // block forever
            WaitEvent.WaitOne();
        }
    }
}
