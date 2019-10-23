using System;

namespace Bb.Workflows.Exceptions
{
    [Serializable]
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException() { }
        public MissingConfigurationException(string message) : base(message) { }
        public MissingConfigurationException(string message, Exception inner) : base(message, inner) { }
        protected MissingConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}
