using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public interface IHumanResourcesAgency
    {
        void Register(string username, string password, string email, string name);
        bool HasUsers();
        bool HasRegisteredUser(string username);
        int NumberOfUsers();
        void DoOnLogin<T>(string username, string password, Func<User, T> onAuthenticaded, Func<T> onFailure);
        User UserFrom(string username);
        void ApplyToJob(string username, string position, string company, string description);
        bool CandidateHasAppliedTo(string username, string position, string company);
        int NumberOfUserApplications(string username);
        List<Application> ApplicationsOf(string username);
    }

    public class HumanResourcesAgency : IHumanResourcesAgency
    {
        public const string NotRegisteredUserCannotOperate = "Not registered user cannot operate";
        public const string CannotLoginUserNotRegistered = "Cannot login not registered user";
        public const string CannotRegisterUserMoreThanOnce = "Cannot register a user more than once";
        private readonly Dictionary<string, string> _passwordsByUser = new();
        private readonly Dictionary<User, Candidate> _users = new();


        public void Register(string username, string password, string email, string name)
        {
            AsssertUserIsNotAlreadyRegistered(username);
            var user = User.Named(username, name, email);
            _users.Add(user, Candidate.With(user));
            _passwordsByUser.Add(username, password);
        }

        public bool HasUsers()
        {
            return _users.Count != 0;
        }

        public bool HasRegisteredUser(string username)
        {
            return _users.Keys.FirstOrDefault(user => user.HasUserName(username)) != null;
        }


        public int NumberOfUsers()
        {
            return _users.Count;
        }

        public void DoOnLogin<T>(string username, string password, Func<User, T> onAuthenticaded, Func<T> onFailure)
        {
            if (!IsAuthenticated(username, password))
            {
                onFailure();
            }
            else
            {
                var user = UserFrom(username);
                onAuthenticaded(user);
            }
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
            return HasRegisteredUser(username) && CandidateFrom(username).HasAppliedToJob(position, company);
        }

        public int NumberOfUserApplications(string username)
        {
            return HasRegisteredUser(username) ? CandidateFrom(username).NumberOfApplications() : 0;
        }

        public List<Application> ApplicationsOf(string username)
        {
            AssertUserIsRegistered(username);
            return CandidateFrom(username).Applications();
        }

        private void AsssertUserIsNotAlreadyRegistered(string username)
        {
            if (HasRegisteredUser(username))
            {
                throw new HumanResourcesAgencyException(CannotRegisterUserMoreThanOnce);
            }
        }

        private KeyValuePair<User, Candidate> FindUser(string username)
        {
            return _users.First(usr => usr.Key.HasUserName(username));
        }

        private bool IsAuthenticated(string username, string password)
        {
            var foundUser = _passwordsByUser.TryGetValue(username, out var userPassword);
            return foundUser && password == userPassword;
        }

        private void AssertUserIsRegistered(string username)
        {
            if (!HasRegisteredUser(username))
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