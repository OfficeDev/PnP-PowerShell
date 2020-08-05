using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamTabResource
    {
        #region Public Members

        /// <summary>
        /// Defines the Configuration for the Tab Resource
        /// </summary>
        public string TabResourceSettings { get; set; }

        /// <summary>
        /// Defines the Type of Resource for the Tab
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TabResourceType Type { get; set; }

        /// <summary>
        /// Defines the ID of the target Tab for the Resource
        /// </summary>
        public string TargetTabId { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the Types of Resources for the Tab
    /// </summary>
    public enum TabResourceType
    {
        /// <summary>
        /// Defines a Generic resource type
        /// </summary>
        Generic,
        /// <summary>
        /// Defines a Notebook resource type
        /// </summary>
        Notebook,
        /// <summary>
        /// Defines a Planner resource type
        /// </summary>
        Planner,
        /// <summary>
        /// Defines a Schedule resource type
        /// </summary>
        Schedule,
    }
}
