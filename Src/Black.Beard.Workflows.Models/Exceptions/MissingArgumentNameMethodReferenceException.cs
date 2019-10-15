using System;

namespace Bb.Workflows.Exceptions
{
    [Serializable]
    public class MissingArgumentNameMethodReferenceException : Exception
    {
        public MissingArgumentNameMethodReferenceException() { }
        public MissingArgumentNameMethodReferenceException(string message) : base(message) { }
        public MissingArgumentNameMethodReferenceException(string message, Exception inner) : base(message, inner) { }
        protected MissingArgumentNameMethodReferenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
