using CommandLine;
using Filestransformer.Actor;
using Filestransformer.Support.CommandLine;

namespace Filestransformer
{
    class Program
    {
        private static IActor Actor;

        static void Main(string[] args)
        {
            Actor = Parser.Default.ParseArguments<Options>(args)
                .MapResult((Options op) => CmdLineHandler.GetActor(op),
                    err => CmdLineHandler.HandleOptionsParseError(err));

            Actor?.Run();
        }
    }
}
