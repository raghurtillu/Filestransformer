using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileGroupManager
{
    /// <summary>
    /// Front end machine to receive raw transfromation file events from <seealso cref="FileSystemWatcher"/>
    /// This actor has a global view of different <seealso cref="TransformationDispatcher"/>
    /// </summary>
    public abstract partial class FileGroupManagerBase : Machine
    {
        /// <summary>
        /// Initializes this machine
        /// </summary>
        protected abstract void InitializeFileGroupManager();

        /// <summary>
        /// Dispatches transformation file request to <seealso cref="TransformationDispatcher"/>
        /// </summary>
        protected abstract void DispatchPendingFileTransformationJobRequests();

        /// <summary>
        /// Handler for <seealso cref="eAddFileToTransform"/> event
        /// </summary>
        protected abstract void HandleAddFileToTransform();

        /// <summary>
        /// Handler for a list of files to transform event
        /// </summary>
        protected abstract void HandleAddFilesToTransform();
    }
}
