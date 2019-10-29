using System;

namespace Bb.Workflows.Exceptions
{
    [Serializable]
    public class InvalidIncomingEventFormatException : Exception
    {
        public InvalidIncomingEventFormatException() { }
        public InvalidIncomingEventFormatException(string message) : base(message) { }
        public InvalidIncomingEventFormatException(string message, Exception inner) : base(message, inner) { }
        protected InvalidIncomingEventFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
