using System;

namespace Filestransformer.StateMachines.FileTransformers
{
    public enum FileTransformerType
    {
        Lowercase = 0,
    }

    /// <summary>
    /// Factory to get FileTransformation machine type <seealso cref="FileTransformerType"/>
    /// </summary>
    public static class FileTransformerFactory
    {   
        public static Type GetFileTransformerType(FileTransformerType transformerType)
        {
            if (transformerType == FileTransformerType.Lowercase)
            {
                return typeof(LowercaseFileTransformer);
            }
            else
            {
                throw new NotImplementedException("Only supporting LowercaseFileTransformation currently");
            }
        }
    }
}
