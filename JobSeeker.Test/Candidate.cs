namespace JobSeeker.Test
{
    internal class Candidate
    {
        private readonly string _name;
        public const string CandidateNameCannotBeBlank = "Candidate name cannot be blank";

        private Candidate(string name)
        {
            _name = name;
        }

        public static Candidate Named(string name)
        {
            AssureNameIsNotBlank(name);
            return new Candidate(name);
        }

        private static void AssureNameIsNotBlank(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CandidateException(CandidateNameCannotBeBlank);
            }
        }

        public bool IsNamed(string aName)
        {
            return _name == aName;
        }
    }
}