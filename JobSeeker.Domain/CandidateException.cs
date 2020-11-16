using System;
using System.Runtime.Serialization;

namespace JobSeeker.Domain
{
    [Serializable]
    public class CandidateException : Exception
    {
        protected CandidateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CandidateException(string? message) : base(message)
        {
        }

        public CandidateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}