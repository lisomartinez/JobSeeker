using System;
using System.Runtime.Serialization;

namespace JobSeeker.Domain
{
    [Serializable]
    public class HumanResourcesAgencyException : Exception
    {
        protected HumanResourcesAgencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HumanResourcesAgencyException(string? message) : base(message)
        {
        }

        public HumanResourcesAgencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}