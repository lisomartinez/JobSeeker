using System;

namespace JobSeeker.Test
{
    internal class CandidateException : Exception
    {
        public override string Message { get; }

        public CandidateException(string message) : base(message)
        {
            Message = message;
        }
    }
}