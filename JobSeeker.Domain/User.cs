namespace JobSeeker.Domain
{
    public class User
    {
        public const string CandidateUsernameCannotBeBlank = "Candidate username cannot be blank";
        public const string CandidateNameCannotBeBlank = "Candidate name cannot be blank";
        public const string CandidateEmailCannotBeBlank = "Candidate email cannot be blank";

        private User(string username, string name, string email)
        {
            Username = username;
            Name = name;
            Email = email;
        }

        public User(string username, string name, string email, string password)
        {
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }

        public string Username { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; } = "";

        public static User Named(string username, string name, string email)
        {
            AssertIsNotBlank(username, CandidateUsernameCannotBeBlank);
            AssertIsNotBlank(email, CandidateEmailCannotBeBlank);
            AssertIsNotBlank(name, CandidateNameCannotBeBlank);
            return new User(username, name, email);
        }

        private static void AssertIsNotBlank(string field, string exceptionMessage)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new CandidateException(exceptionMessage);
            }
        }


        public bool IsNamed(string aName)
        {
            return Name == aName;
        }

        public bool HasUserName(string aUserName)
        {
            return Username == aUserName;
        }
    }
}