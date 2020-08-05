using Microsoft.SharePoint.Client;
using System;
using System.Linq.Expressions;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class UserPipeBind
    {
        private readonly int _id;
        private readonly string _login;
        private readonly User _user;

        public UserPipeBind()
        {
            _id = 0;
            _login = null;
            _user = null;
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

        public User User => _user;

        public User GetUser(ClientContext context)
        {
            // note: the following code to get the user is copied from Remove-PnPUser - it could be put into a utility class
            var retrievalExpressions = new Expression<Func<User, object>>[]
            {
                u => u.Id,
                u => u.LoginName,
                u => u.Email
            };

            User user = null;
            if (User != null)
            {
                user = User;
            }
            else if (Id > 0)
            {
                user = context.Web.GetUserById(Id);
            }
            else if (!string.IsNullOrWhiteSpace(Login))
            {
                user = context.Web.SiteUsers.GetByLoginName(Login);
            }
            if (context.HasPendingRequest)
            {
                context.Load(user, retrievalExpressions);
                context.ExecuteQueryRetry();
            }
            return user;
        }
    }
}
