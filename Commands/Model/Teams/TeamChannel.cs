using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamChannel
    {
        #region Public Members

        /// <summary>
        /// Defines a collection of Tabs for a Channel in a Team
        /// </summary>
        public List<TeamTab> Tabs { get; private set; }

        /// <summary>
        /// Defines a collection of Resources for Tabs in a Team Channel
        /// </summary>
        public List<TeamTabResource> TabResources { get; private set; }

        /// <summary>
        /// Defines a collection of Messages for a Team Channe
        /// </summary>
        public List<TeamChannelMessage> Messages { get; private set; }

        /// <summary>
        /// Defines the Display Name of the Channel
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Defines the Description of the Channel
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Defines whether the Channel is Favorite by default for all members of the Team
        /// </summary>
        public bool? IsFavoriteByDefault { get; set; }

        /// <summary>
        /// Declares the ID for the Channel
        /// </summary>
        public string Id { get; set; }

        #endregion
    }
}
