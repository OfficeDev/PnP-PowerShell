using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamGuestSettings 
    {
        #region Public Members

        /// <summary>
        /// Defines whether Guests are allowed to create Channels or not
        /// </summary>
        public bool? AllowCreateUpdateChannels { get; set; }

        /// <summary>
        /// Defines whether Guests are allowed to delete Channels or not
        /// </summary>
        public bool? AllowDeleteChannels { get; set; }

        #endregion
    }
}
