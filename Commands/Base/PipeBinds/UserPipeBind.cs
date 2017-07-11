using Microsoft.SharePoint.Client;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class UserPipeBind
    {
        private readonly int _id;
        private readonly string _login;
        private readonly string _email;
        private readonly User _user;

        public UserPipeBind()
        {
            _id = 0;
            _login = null;
            _user = null;
            _email = null;
        }

        public UserPipeBind(int id)
        {
            _id = id;
        }

        public UserPipeBind(string id)
        {
            if (!int.TryParse(id, out _id))
            {
                _login = id;
            }
        }

        public UserPipeBind(User user)
        {
            _user = user;
        }

        public int Id => _id;

        public string Login => _login;
        //public string Email => _email;

        public User User => _user;

        internal User GetUser(Web web, bool Email = false)
        {
            User user = null;
            if (Id != -1)
                user = web.GetUserById(Id);
            else if (!String.IsNullOrEmpty(Login) && !Email)
                user = web.SiteUsers.GetByLoginName(Login);
            else if (!String.IsNullOrEmpty(Login) && Email)
                user = web.SiteUsers.GetByEmail(Login);
            else if (User != null)
                user = User;
            web.Context.Load(user);
            web.Context.Load(user.Groups);
            web.Context.ExecuteQueryRetry();
            if (user == null && !String.IsNullOrEmpty(Login) && !Email)
                return GetUser(web, true); //true to get the user with email instead of login
            return user;
        }
    }
}
