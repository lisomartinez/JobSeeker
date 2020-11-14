using System;

namespace JobSeeker.Domain
{
    public class CandidateException : Exception
    {
        public override string Message { get; }

        public CandidateException(string message) : base(message)
        {
            Message = message;
        }
    }
}