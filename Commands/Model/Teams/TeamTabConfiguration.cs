using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamTabConfiguration
    {
        #region Public Members

        /// <summary>
        /// Identifier for the entity hosted by the Tab provider
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Url used for rendering Tab contents in Teams
        /// </summary>
        public string ContentUrl { get; set; }


        /// <summary>
        /// Url called by Teams client when a Tab is removed using the Teams Client
        /// </summary>
        public string RemoveUrl { get; set; }

        /// <summary>
        /// Url for showing Tab contents outside of Teams
        /// </summary>
        public string WebsiteUrl { get; set; }

        #endregion
    }
}
