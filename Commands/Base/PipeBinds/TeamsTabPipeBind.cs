using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Web;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsTabPipeBind
    {
        private readonly string _id;
        private readonly string _displayName;
        private readonly TeamTab _tab;

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

        public TeamsTabPipeBind(TeamTab tab)
        {
            _tab = tab;
        }

        public string Id => _id;

        public TeamTab GetTab(HttpClient httpClient, string accessToken, string groupId, string channelId)
        {
            if (_tab != null)
            {
                return _tab;
            }
            else
            {
                var tab = TeamsUtility.GetTabAsync(accessToken, httpClient, groupId, channelId, _id).GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(tab.Id))
                {
                    var tabs = TeamsUtility.GetTabsAsync(accessToken, httpClient, groupId, channelId).GetAwaiter().GetResult();
                    if (tabs != null)
                    {
                        // find the tab by id
                        tab = tabs.FirstOrDefault(t => t.DisplayName.Equals(_displayName, System.StringComparison.OrdinalIgnoreCase));
                    }
                }
                if (tab != null)
                {
                    return tab;
                }
                else
                {
                    throw new PSArgumentException("Cannot find tab");
                }
            }
        }
    }
}

