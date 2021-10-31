using System;

namespace Filestransformer.StateMachines.FileTransformers
{
    public enum FileTransformerType
    {
        Lowercase = 0,
    }

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
