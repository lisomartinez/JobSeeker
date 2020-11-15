namespace JobSeeker.Domain
{
    public class User
    {
        private readonly string _username;
        private readonly string _name;
        private readonly string _email;

        private User(string username, string name, string email)
        {
            _username = username;
            _name = name;
            _email = email;
        }

        public const string CandidateUsernameCannotBeBlank = "Candidate username cannot be blank";
        public const string CandidateNameCannotBeBlank = "Candidate name cannot be blank";
        public const string CandidateEmailCannotBeBlank = "Candidate email cannot be blank";

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
            return _name == aName;
        }

        public bool HasUserName(string aUserName)
        {
            return _username == aUserName;
        }
    }
}