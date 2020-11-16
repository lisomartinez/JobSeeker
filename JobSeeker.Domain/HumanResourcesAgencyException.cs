using System;

namespace JobSeeker.Domain
{
    [Serializable]
    public class HumanResourcesAgencyException : Exception
    {
        public HumanResourcesAgencyException(string? message) : base(message)
        {
        }
    }
}