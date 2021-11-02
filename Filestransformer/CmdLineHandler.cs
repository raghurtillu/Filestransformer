using CommandLine;
using Filestransformer.Actor;
using Filestransformer.Settings;
using Filestransformer.Support.CommandLine;
using Filestransformer.Support.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Filestransformer
{
    /// <summary>
    /// processes command line arguments and invokes FileTransformation workflow
    /// </summary>
    public class CmdLineHandler
    {
        /// <summary>
        /// Gets FileTransformation workflow actor <seealso cref="IActor"/>
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static IActor GetActor(Options op)
        {
            // merge console and default settings
            // console settings can override default settings
            //
            var settings = new DefaultSettingsProvider().GetSettings()
                .MergeFrom(new ConsoleSettingsProvider(op).GetSettings())
                    .Validate();
           
            return new FiletransformerActor(settings);
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
