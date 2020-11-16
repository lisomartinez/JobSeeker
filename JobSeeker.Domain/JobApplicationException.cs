using System;
using System.Runtime.Serialization;

namespace JobSeeker.Domain
{
    [Serializable]
    public class JobApplicationException : Exception
    {
        protected JobApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public JobApplicationException(string? message) : base(message)
        {
        }

        public JobApplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}