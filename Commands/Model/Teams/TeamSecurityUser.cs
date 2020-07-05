using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamSecurityUser
    {
        #region Public Members

        /// <summary>
        /// Defines User Principal Name (UPN) of the target user
        /// </summary>
        public string UserPrincipalName { get; set; }

        #endregion
    }
}
