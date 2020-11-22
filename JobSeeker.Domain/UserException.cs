using System;

namespace JobSeeker.Domain
{
    [Serializable]
    internal class UserException : Exception
    {
        public UserException(string? message) : base(message)
        {
        }
    }
}