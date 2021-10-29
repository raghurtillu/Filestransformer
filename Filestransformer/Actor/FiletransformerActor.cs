using Filestransformer.Settings;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System.Threading;
namespace Filestransformer.Actor
{
    public class FiletransformerActor : IActor
    {
        private IMachineRuntime psharpRuntime;

        private readonly Setting settings;
        private static AutoResetEvent WaitEvent = new AutoResetEvent(false);

        public FiletransformerActor(Setting settings)
        {
            this.settings = settings;
            this.psharpRuntime = PSharpRuntime.Create(Microsoft.PSharp.Configuration.Create());
        }

        public void Run()
        {
            var logger = LoggerFactory.GetLogger();

            logger.WriteLine("");
            logger.WriteLine("Running file transformer with the following settings: ");
            logger.WriteLine($"FileGroups: {settings.FileGroups}");
            logger.WriteLine($"MaximumParallelFileTransformations: {settings.MaxParallelFileTransformations}");

            //MachineId tableStorageMachine = psharpRuntime.CreateMachine(typeof(TableStorage));

            // block forever
            WaitEvent.WaitOne();
        }
    }
}
