using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamMemberSettings
    {

        #region Public Members

        /// <summary>
        /// Defines if members can add and update channels
        /// </summary>
        public bool? AllowCreateUpdateChannels { get; set; }

        /// <summary>
        /// Defines if members can delete channels
        /// </summary>
        public bool? AllowDeleteChannels { get; set; }

        /// <summary>
        /// Defines if members can add and remove apps
        /// </summary>
        public bool? AllowAddRemoveApps { get; set; }

        /// <summary>
        /// Defines if members can add, update, and remove tabs
        /// </summary>
        public bool? AllowCreateUpdateRemoveTabs { get; set; }

        /// <summary>
        /// Defines if members can add, update, and remove connectors
        /// </summary>
        public bool? AllowCreateUpdateRemoveConnectors { get; set; }

        /// <summary>
        /// Defines if members can create private channels
        /// </summary>
        public bool? AllowCreatePrivateChannels { get; set; }

        #endregion
    }
}
