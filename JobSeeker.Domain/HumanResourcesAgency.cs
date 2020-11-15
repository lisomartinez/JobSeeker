using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public class HumanResourcesAgency
    {
        private HashSet<User> _users = new HashSet<User>();
        private Dictionary<string, string> _passwordsByUser = new Dictionary<string, string>();
        public const string CannotLoginUserNotRegistered = "Cannot login not registered user";
        public const string CannotRegisterUserMoreThanOnce = "Cannot register a user more than once";


        public void Register(string username, string password, string email, string name)
        {
            AsssertUserIsNotAlreadyRegistered(username);
            var user = User.Named(username, name, email);
            _users.Add(user);
            _passwordsByUser.Add(username, password);
        }

        private void AsssertUserIsNotAlreadyRegistered(string username)
        {
            if (HasRegisteredUser(username))
            {
                throw new HumanResourcesAgencyException(CannotRegisterUserMoreThanOnce);
            }
        }

        public bool HasUsers()
        {
            return _users.Count != 0;
        }

        public bool HasRegisteredUser(string username)
        {
            return _users.FirstOrDefault(user => user.HasUserName(username)) != null;
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
                return;
            }

            var user = _users.First(usr => usr.HasUserName(username));
            onAuthenticaded(user);
        }

        private bool IsAuthenticated(string username, string password)
        {
            var foundUser = _passwordsByUser.TryGetValue(username, out var userPassword);
            return foundUser && password == userPassword;
        }
    }
}