using System.Text.Json.Serialization;
using System.Web;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamTab
    {
        private string _displayName;
        #region Public Members

        /// <summary>
        /// Defines the Display Name of the Channel
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = HttpUtility.UrlDecode(value);
            }
        }

        [JsonPropertyName("teamsApp@odata.bind")]
        public string TeamsApp { get; set; }
        /// <summary>
        /// App definition identifier of the tab
        /// </summary>
        public string TeamsAppId { get; set; }

        /// <summary>
        /// Allows to remove an already existing Tab
        /// </summary>
        public bool Remove { get; set; }

        /// <summary>
        /// Defines the Configuration for the Tab
        /// </summary>
        public TeamTabConfiguration Configuration { get; set; }

        /// <summary>
        /// Declares the ID for the Tab
        /// </summary>
        public string Id { get; set; }

        #endregion
    }
}
