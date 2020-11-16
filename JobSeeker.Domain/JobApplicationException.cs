using System;

namespace JobSeeker.Domain
{
    [Serializable]
    public class JobApplicationException : Exception
    {
        public JobApplicationException(string? message) : base(message)
        {
        }
    }
}