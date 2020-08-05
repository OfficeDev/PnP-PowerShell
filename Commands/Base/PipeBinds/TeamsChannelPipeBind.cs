using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Linq;
using System.Net.Http;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsChannelPipeBind
    {
        private readonly string _id;
        private readonly string _displayName;
        public TeamsChannelPipeBind()
        {

        }

        public TeamsChannelPipeBind(string input)
        {
            // check if it's a channel id
            if (input.EndsWith("@tread.skype") && input.Substring(2, 1) == ":")
            {
                _id = input;
            }
            else
            {
                _displayName = input;
            }
        }

        public TeamsChannelPipeBind(Model.Teams.TeamChannel channel)
        {
            _id = channel.Id;
        }


        public string Id => _id;

        public string GetId(HttpClient httpClient, string accessToken, string groupId)
        {
            if (!string.IsNullOrEmpty(_id))
            {
                return _id;
            }
            else
            {
                var channels = TeamsUtility.GetChannelsAsync(accessToken, httpClient, groupId).GetAwaiter().GetResult();
                return channels.FirstOrDefault(c => c.DisplayName.Equals(_displayName, StringComparison.OrdinalIgnoreCase)).Id;
            }
        }

        public TeamChannel GetChannel(HttpClient httpClient, string accessToken, string groupId)
        {
            var channels = TeamsUtility.GetChannelsAsync(accessToken, httpClient, groupId).GetAwaiter().GetResult();
            if(channels != null && channels.Any())
            {
                if(!string.IsNullOrEmpty(_id))
                {
                    return channels.FirstOrDefault(c => c.Id.Equals(_id, StringComparison.OrdinalIgnoreCase));
                } else
                {
                    return channels.FirstOrDefault(c => c.DisplayName.Equals(_displayName, StringComparison.OrdinalIgnoreCase));
                }
            }
            return null;
        }

    }
}
