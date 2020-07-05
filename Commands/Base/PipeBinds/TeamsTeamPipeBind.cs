using SharePointPnP.PowerShell.Commands.Model.Teams;
using SharePointPnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsTeamPipeBind
    {
        private readonly string _id;
        private readonly string _stringValue;

        public TeamsTeamPipeBind()

        {
            _id = string.Empty;
            _stringValue = string.Empty;
        }

        public TeamsTeamPipeBind(string input)
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
                _stringValue = input;
            }
        }

        public TeamsTeamPipeBind(Team team)
        {
            _id = team.GroupId;
        }

        public string GetGroupId(HttpClient httpClient, string accessToken)
        {
            if (!string.IsNullOrEmpty(_id))
            {
                return _id.ToString();
            }
            else
            {
                var collection = GraphHelper.GetAsync<GraphCollection<Model.Teams.Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and mailNickname eq '{_stringValue}')&$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken).GetAwaiter().GetResult();
                if(collection != null && collection.Items.Any())
                { 
                    return collection.Items.First().Id;
                }
                else
                {
                    // find the team by displayName
                    var byDisplayNamecollection = GraphHelper.GetAsync<GraphCollection<Model.Teams.Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and displayName eq '{_stringValue}')&$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken).GetAwaiter().GetResult();
                    if (byDisplayNamecollection != null)
                    {
                        if (byDisplayNamecollection.Items.Count() == 1)
                        {
                            return byDisplayNamecollection.Items.First().Id;
                        } else
                        {
                            throw new PSArgumentException("We found more matches based on the identity value you entered. Use Get-PnPTeamsTeam to find the correct instance and use the GroupId from a team to select the correct team instead.");
                        }
                    }
                    return null;
                }
            }
        }

    }
}
