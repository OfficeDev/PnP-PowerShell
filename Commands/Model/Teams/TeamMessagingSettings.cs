using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamMessagingSettings
    {
        #region Public Members

        /// <summary>
        /// Defines if users can edit their messages
        /// </summary>
        public bool? AllowUserEditMessages { get; set; }

        /// <summary>
        /// Defines if users can delete their messages
        /// </summary>
        public bool? AllowUserDeleteMessages { get; set; }

        /// <summary>
        /// Defines if owners can delete any message
        /// </summary>
        public bool? AllowOwnerDeleteMessages { get; set; }

        /// <summary>
        /// Defines if @team mentions are allowed
        /// </summary>
        public bool? AllowTeamMentions { get; set; }

        /// <summary>
        /// Defines if @channel mentions are allowed
        /// </summary>
        public bool? AllowChannelMentions { get; set; }

        #endregion

    }
}
