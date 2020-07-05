using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Model.Teams;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsTabPipeBind
    {
        private readonly string _id;
        private readonly string _displayName;

        public TeamsTabPipeBind()
        {
        }

        public TeamsTabPipeBind(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(nameof(input));
            }
            if (Guid.TryParse(input, out Guid tabId))
            {
                _id = input;
            }
            else
            {
                _displayName = input;
            }
        }

        public string Id => _id;

        public TeamTab GetTab(HttpClient httpClient, string accessToken, string groupId, string channelId)
        {
            // find the tab by the displayName
            var tabs = TeamsUtility.GetTabs(accessToken, httpClient, groupId, channelId);
            if (tabs != null)
            {
                var tab = tabs.FirstOrDefault(t => t.DisplayName.Equals(_displayName, System.StringComparison.OrdinalIgnoreCase));
                if (tab != null)
                {
                    return tab;
                }
                else
                {
                    throw new PSArgumentException("Cannot find tab");
                }
            }
            return null;
        }

        public TeamTab GetTabById(HttpClient httpClient, string accessToken, string groupId, string channelId)
        {
            return TeamsUtility.GetTab(accessToken, httpClient, groupId, channelId, _id);
        }
    }
}

