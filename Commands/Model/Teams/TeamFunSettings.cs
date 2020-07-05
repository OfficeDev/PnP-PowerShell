using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamFunSettings
    {
        #region Private Members

        private string _giphyContentRating;

        #endregion

        #region Public Members

        /// <summary>
        /// Defines whether Giphys are consented or not
        /// </summary>
        public bool? AllowGiphy { get; set; }

        /// <summary>
        /// Defines the Content Rating for Giphys
        /// </summary>
        public TeamGiphyContentRating GiphyContentRating { get; set; }

        /// <summary>
        /// Defines whether Stickers and Memes are consented or not
        /// </summary>
        public bool? AllowStickersAndMemes { get; set; }

        /// <summary>
        /// Defines whether Custom Memes are consented or not
        /// </summary>
        public bool? AllowCustomMemes { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the Content Rating for Giphys
    /// </summary>
    public enum TeamGiphyContentRating
    {
        moderate,
        strict
    }
}
