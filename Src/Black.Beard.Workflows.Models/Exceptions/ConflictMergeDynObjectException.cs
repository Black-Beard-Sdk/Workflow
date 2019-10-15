using System;

namespace Bb.Workflows.Exceptions
{
    [Serializable]
    public class ConflictMergeDynObjectException : Exception
    {
        public ConflictMergeDynObjectException() { }
        public ConflictMergeDynObjectException(string message) : base(message) { }
        public ConflictMergeDynObjectException(string message, Exception inner) : base(message, inner) { }
        protected ConflictMergeDynObjectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
