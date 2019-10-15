using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows.Exceptions
{


    [Serializable]
    public class InvalidMethodReferenceException : Exception
    {
        public InvalidMethodReferenceException() { }
        public InvalidMethodReferenceException(string message) : base(message) { }
        public InvalidMethodReferenceException(string message, Exception inner) : base(message, inner) { }
        protected InvalidMethodReferenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
