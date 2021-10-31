using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileTransformers
{
    public class LowercaseFileTransformer : FileTransformer
    {
        private FileStream fileStream;

        protected override void InitializeFileTransformer()
        {
            base.InitializeFileTransformer();

            logger.WriteLine($"Initialized {nameof(LowercaseFileTransformer)} machine for {fileName} successfully.");
        }
    }
}
