using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public interface ICandidateFolder
    {
        KeyValuePair<User, Candidate> FindUser(string username);
        Candidate CandidateFrom(string username);
        void AddCandidate(User user, Candidate candidateToAdd);
        bool HasUsers();
        bool HasRegisteredUser(string username);
        int NumberOfUsers();
    }

    public class TransientCandidateFolder : ICandidateFolder
    {
        private readonly Dictionary<User, Candidate> _users = new();

        public KeyValuePair<User, Candidate> FindUser(string username)
        {
            return _users.First(usr => usr.Key.HasUserName(username));
        }

        public Candidate CandidateFrom(string username)
        {
            return FindUser(username).Value;
        }

        public void AddCandidate(User user, Candidate candidateToAdd)
        {
            _users.Add(user, candidateToAdd);
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
    }
}