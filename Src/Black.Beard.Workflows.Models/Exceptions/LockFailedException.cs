using System;

namespace Bb.Workflows.Exceptions
{


    [Serializable]
    public class LockFailedException : Exception
    {
        public LockFailedException() { }

        public LockFailedException(uint crc) : base(crc.ToString()) { }
        
        public LockFailedException(string message, Exception inner) : base(message, inner) { }
        
        protected LockFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}
