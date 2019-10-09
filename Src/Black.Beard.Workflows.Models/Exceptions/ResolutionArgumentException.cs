using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows.Exceptions
{


    [Serializable]
    public class ResolutionArgumentException : Exception
    {
        public ResolutionArgumentException() { }
        public ResolutionArgumentException(string message) : base(message) { }
        public ResolutionArgumentException(string message, Exception inner) : base(message, inner) { }
        protected ResolutionArgumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
