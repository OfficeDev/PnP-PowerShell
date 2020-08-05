using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamDiscoverySettings
    {
        #region Public Members

        /// <summary>
        /// Defines whether the Team is visible via search and suggestions from the Teams client
        /// </summary>
        public bool? ShowInTeamsSearchAndSuggestions { get; set; }

        #endregion
    }
}
