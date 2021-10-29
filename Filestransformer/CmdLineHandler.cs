using CommandLine;
using Filestransformer.Actor;
using Filestransformer.Support.CommandLine;
using Filestransformer.Support.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Filestransformer
{
    public class CmdLineHandler
    {
        public static IActor GetActor(Options op)
        {
            if (op.FileGroups <= 0)
            {
                throw new ArgumentException($"Invalid value '{op.FileGroups}' specified.");
            }
            if (op.MaximumParallelFileTransformations <= 0)
            {
                throw new ArgumentException($"Invalid value '{op.MaximumParallelFileTransformations}' specified.");
            }

            Settings.Instance.Initialize(op.FileGroups, op.MaximumParallelFileTransformations);
            return new FiletransformerActor(Settings.Instance);
        }

        public static IActor HandleOptionsParseError(IEnumerable<Error> errors)
        {
            var logger = LoggerFactory.GetLogger();
            logger.WriteLine("One or more errors occurred while parsing command line options.");
            logger.WriteLine($"Errors: '{errors.Count()}'");
            if (errors.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
                logger.WriteLine("See Help section.");
            }
            return null;
        }
    }
}
