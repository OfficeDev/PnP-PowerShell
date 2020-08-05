using System;
using System.Collections.Generic;
using System.Text;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public class User : IEquatable<User>
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UserPrincipalName { get; set; }
        public string UserType { get; set; }

        public bool Equals(User other)
        {
            return this.UserPrincipalName.Equals(other.UserPrincipalName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
