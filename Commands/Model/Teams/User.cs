using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UserIdentityType { get; set; }
    }
}
