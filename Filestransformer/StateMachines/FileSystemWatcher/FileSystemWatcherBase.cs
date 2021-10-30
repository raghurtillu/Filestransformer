using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileSystemWatcher
{
    public abstract partial class FileSystemWatcherBase : Machine
    {
        protected abstract void InitializeFileSystemWatcher();

        protected abstract void Run();
    }
}
