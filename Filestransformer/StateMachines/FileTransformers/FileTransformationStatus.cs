namespace Filestransformer.StateMachines.FileTransformers
{
    public enum FileTransformationStatus
    {
        /// <summary>
        /// Transformation is currently running
        /// </summary>
        InProgress,

        /// <summary>
        /// Transformation completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// Transformation failed to complete
        /// </summary>
        Failed,

        //Blocked
    }
}
