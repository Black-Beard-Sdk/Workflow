using System;
using System.Collections.Generic;
using System.Text;

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

    [Serializable]
    public class DuplicatedArgumentNameMethodReferenceException : Exception
    {
        public DuplicatedArgumentNameMethodReferenceException() { }
        public DuplicatedArgumentNameMethodReferenceException(string message) : base(message) { }
        public DuplicatedArgumentNameMethodReferenceException(string message, Exception inner) : base(message, inner) { }
        protected DuplicatedArgumentNameMethodReferenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidArgumentNameMethodReferenceException : Exception
    {
        public InvalidArgumentNameMethodReferenceException() { }
        public InvalidArgumentNameMethodReferenceException(string message) : base(message) { }
        public InvalidArgumentNameMethodReferenceException(string message, Exception inner) : base(message, inner) { }
        protected InvalidArgumentNameMethodReferenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

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
