using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamSecurity
    {
        #region Public Members

        /// <summary>
        /// Defines the Owners of the Team
        /// </summary>
        public List<TeamSecurityUser> Owners { get; private set; } = new List<TeamSecurityUser>();           

        /// <summary>
        /// Declares whether to clear existing owners before adding new ones
        /// </summary>
        public bool ClearExistingOwners { get; set; }

        /// <summary>
        /// Defines the Members of the Team
        /// </summary>
        public List<TeamSecurityUser> Members { get; private set; } = new List<TeamSecurityUser>();

        /// <summary>
        /// Declares whether to clear existing members before adding new ones
        /// </summary>
        public bool ClearExistingMembers { get; set; }

        /// <summary>
        /// Defines whether guests are allowed in the Team
        /// </summary>
        public bool AllowToAddGuests { get; set; }

        #endregion
    }
}
