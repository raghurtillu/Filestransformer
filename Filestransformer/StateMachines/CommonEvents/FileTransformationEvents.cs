using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.CommonEvents
{
    public class eAddFileToTransform : Event
    {
        /// <summary>
        /// Fully qualified filename (of the form group1.work1.txt) to transform
        /// </summary>
        public string FullyQualifiedFileName { get; }

        public eAddFileToTransform(string fullyQualifiedFileName)
        {
            this.FullyQualifiedFileName = fullyQualifiedFileName;
        }
    }

    public class eAddFilesToTransform : Event
    {
        public IList<string> FullyQualifiedFileNames { get; }

        public eAddFilesToTransform(IList<string> fullyQualifiedFileNames)
        {
            this.FullyQualifiedFileNames = fullyQualifiedFileNames;
        }
    }
}
