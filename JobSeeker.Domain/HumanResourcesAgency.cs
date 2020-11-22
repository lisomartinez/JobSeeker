using System.Collections.Generic;

namespace JobSeeker.Domain
{
    public interface IHumanResourcesAgency
    {
        void Register(string username);
        User UserFrom(string username);
        void ApplyToJob(string username, string position, string company, string description);
        bool CandidateHasAppliedTo(string username, string position, string company);
        int NumberOfUserApplications(string username);
        List<Application> ApplicationsOf(string username);
    }

    public class HumanResourcesAgency : IHumanResourcesAgency
    {
        public const string NotRegisteredUserCannotOperate = "Not registered user cannot operate";
        public const string CannotRegisterUserMoreThanOnce = "Cannot register a user more than once";
        private readonly ICandidateFolder _candidateFolder;

        public HumanResourcesAgency(ICandidateFolder candidateFolder)
        {
            _candidateFolder = candidateFolder;
        }

        public void Register(string username)
        {
            AsssertUserIsNotAlreadyRegistered(username);
            var user = User.Named(username);
            _candidateFolder.AddCandidate(user, Candidate.With(user));
        }


        public User UserFrom(string username)
        {
            return FindUser(username).Key;
        }

        public void ApplyToJob(string username, string position, string company, string description)
        {
            AssertUserIsRegistered(username);
            var candidate = CandidateFrom(username);
            candidate.ApplyToJob(position, company, description);
        }

        public bool CandidateHasAppliedTo(string username, string position, string company)
        {
            return _candidateFolder.HasRegisteredUser(username) &&
                   CandidateFrom(username).HasAppliedToJob(position, company);
        }

        public int NumberOfUserApplications(string username)
        {
            return _candidateFolder.HasRegisteredUser(username) ? CandidateFrom(username).NumberOfApplications() : 0;
        }

        public List<Application> ApplicationsOf(string username)
        {
            AssertUserIsRegistered(username);
            return CandidateFrom(username).Applications();
        }

        private void AsssertUserIsNotAlreadyRegistered(string username)
        {
            if (_candidateFolder.HasRegisteredUser(username))
            {
                throw new HumanResourcesAgencyException(CannotRegisterUserMoreThanOnce);
            }
        }

        private KeyValuePair<User, Candidate> FindUser(string username)
        {
            return _candidateFolder.FindUser(username);
        }


        private void AssertUserIsRegistered(string username)
        {
            if (!_candidateFolder.HasRegisteredUser(username))
            {
                throw new HumanResourcesAgencyException(NotRegisteredUserCannotOperate);
            }
        }

        private Candidate CandidateFrom(string username)
        {
            return FindUser(username).Value;
        }
    }
}