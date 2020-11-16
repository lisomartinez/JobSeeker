using System;

namespace JobSeeker.Domain
{
    [Serializable]
    public class CandidateException : Exception
    {
        public CandidateException(string? message) : base(message)
        {
        }
    }
}