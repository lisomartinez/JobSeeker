using System;

namespace JobSeeker.Domain
{
    public class HumanResourcesAgencyException : Exception
    {
        public HumanResourcesAgencyException(string message) : base(message)
        {
        }
    }
}