using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public class TeamAppInstance
    {
        #region Public Members

        /// <summary>
        /// Defines the unique ID of the App to install or update on the Team
        /// </summary>
        public string AppId { get; set; }

        #endregion
    }
}
