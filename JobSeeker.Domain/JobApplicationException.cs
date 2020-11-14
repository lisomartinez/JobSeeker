using System;

namespace JobSeeker.Domain
{
    public class JobApplicationException : Exception
    {
        public JobApplicationException(string message) : base(message)
        {
        }
    }
}