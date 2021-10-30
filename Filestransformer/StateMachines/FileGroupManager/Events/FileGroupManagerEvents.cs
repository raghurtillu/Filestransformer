using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;


namespace Filestransformer.StateMachines.FileGroupManager.Events
{
    public class eFileGroupManagerConfig : Event
    {
        public ILogger Logger { get; }
        public int FileGroups { get; }
        public string InputDirectory { get; }

    }
}
