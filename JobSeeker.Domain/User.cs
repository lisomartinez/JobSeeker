using System;

namespace JobSeeker.Domain
{
    public class User
    {
        public const string CandidateUsernameCannotBeBlank = "Candidate username cannot be blank";
        public const string CannotHaveNotGuidUsernameFormat = "User should have Guid username";


        private User(string username)
        {
            Username = username;
        }

        public string Username { get; }


        public static User Named(string username)
        {
            AssertIsNotBlank(username);
            AssertUsernameHasGuidFormat(username);

            return new User(username);
        }

        private static void AssertUsernameHasGuidFormat(string username)
        {
            if (!Guid.TryParse(username, out var _))
            {
                throw new UserException(CannotHaveNotGuidUsernameFormat);
            }
        }

        private static void AssertIsNotBlank(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new UserException(CandidateUsernameCannotBeBlank);
            }
        }


        public bool HasUserName(string aUserName)
        {
            return Username == aUserName;
        }
    }
}