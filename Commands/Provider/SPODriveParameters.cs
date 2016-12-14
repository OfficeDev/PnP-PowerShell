using System.Management.Automation;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    public class SPODriveParameters
    {
        [Parameter(HelpMessage = "Drive web. Use either context or the web parameter. If neither supplied, current context will be used.")]
        public Web Web { get; set; }

        [Parameter(HelpMessage = "Drive context. Use either context or the web parameter. If neither supplied, current context will be used.")]
        public ClientRuntimeContext Context { get; set; }

        [Parameter(HelpMessage = "Url to connect to with in the context.")]
        public string Url { get; set; }

        [Parameter(HelpMessage = "Timeout for caching item (files and folders) in milliseconds. Default: 1000 (1 second).")]
        public int ItemCacheTimeout { get; set; }

        [Parameter(HelpMessage = "Timeout for caching webs in milliseconds. Default: 600000 (10 minutes).")]
        public int WebCacheTimeout { get; set; }
    }
}