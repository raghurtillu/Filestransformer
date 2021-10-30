using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileGroupManager
{
    public abstract partial class FileGroupManagerBase : Machine
    {
        protected abstract void InitializeFileGroupManager();

        protected abstract void HandleAddFileToTransform();

        protected abstract void HandleAddFilesToTransform();
    }
}
