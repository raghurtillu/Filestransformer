using CommandLine;
using Filestransformer.Actor;
using Filestransformer.Support.Logger;
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

        //static void PrintUsage()
        //{
        //    var logger = LoggerFactory.GetLogger();
        //    logger.WriteLine("Filetransformer is a program to process text files, each file belongs one group, " +
        //        "each group can have arbitrary number of files associated with it, at most \"three\" transformations can be performed" +
        //        " in an epoch, but this can be overriden from command line");
        //}
    }
}
