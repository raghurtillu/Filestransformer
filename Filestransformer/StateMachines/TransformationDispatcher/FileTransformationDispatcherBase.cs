using Microsoft.PSharp;

namespace Filestransformer.StateMachines.TransformationDispatcher
{
    /// <summary>
    /// File transformation orchestrator machine for a group
    /// </summary>
    public abstract partial class FileTransformationDispatcherBase : Machine
    {
        /// <summary>
        /// Initializes machine
        /// </summary>
        protected abstract void InitializeFileTransformationDispatcher();

        /// <summary>
        /// Handler for a new file transform event <seealso cref="eAddFileToTransform"/>
        /// </summary>
        protected abstract void HandleAddFileToTransform();

        /// <summary>
        /// Dispatches file transformation request to <seealso cref="FileTransformer"/> machine
        /// </summary>
        protected abstract void DispatchPendingFileTransformationJobRequests();

        /// <summary>
        /// Handler for a file transformation response
        /// </summary>
        protected abstract void HandleFileTransformationResponse();

        /// <summary>
        /// Determines if the orchestrator is running at full capacity as specified in <seealso cref="Setting.MaxParallelFileTransformations"/>
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsRunningAtFullCapacity();

        /// <summary>
        /// Determines if the orchestrator have any pending transformation jobs
        /// </summary>
        /// <returns></returns>
        protected abstract bool HasPendingJobs();

        protected abstract void DisplayIdleStateMessage(bool timerExpired);

        protected abstract void SetRetryTimer();
    }
}
