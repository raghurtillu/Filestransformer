using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.TransformationDispatcher
{
    public abstract partial class FileTransformationDispatcherBase : Machine
    {
        protected abstract void InitializeFileTransformationDispatcher();

        protected abstract void HandleAddFileToTransform();

        protected abstract void DispatchPendingFileTransformationJobRequests();

        protected abstract void HandleFileTransformationResponse();

        protected abstract bool IsRunningAtFullCapacity();

        protected abstract bool HasPendingJobs();

        protected abstract void DisplayIdleStateMessage(bool timerExpired);

        protected abstract void SetRetryTimer();
    }
}
