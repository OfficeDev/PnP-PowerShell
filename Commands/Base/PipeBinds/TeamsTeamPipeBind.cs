using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;

namespace PnP.PowerShell.Commands.Base.PipeBinds
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
                var collection = GraphHelper.GetAsync<GraphCollection<Model.Teams.Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and mailNickname eq '{_stringValue}')&$select=Id", accessToken).GetAwaiter().GetResult();
                if (collection != null && collection.Items.Any())
                {
                    return collection.Items.First().Id;
                }
                else
                {
                    // find the team by displayName
                    var byDisplayNamecollection = GraphHelper.GetAsync<GraphCollection<Model.Teams.Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and displayName eq '{_stringValue}')&$select=Id", accessToken).GetAwaiter().GetResult();
                    if (byDisplayNamecollection != null && byDisplayNamecollection.Items.Any())
                    {
                        if (byDisplayNamecollection.Items.Count() == 1)
                        {
                            return byDisplayNamecollection.Items.First().Id;
                        }
                        else
                        {
                            throw new PSArgumentException("We found more matches based on the identity value you entered. Use Get-PnPTeamsTeam to find the correct instance and use the GroupId from a team to select the correct team instead.");
                        }
                    }
                    return null;
                }
            }
        }

        public Team GetTeam(HttpClient httpClient, string accessToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(_id))
                {
                    return GraphHelper.GetAsync<Team>(httpClient, $"v1.0/teams/{_id}", accessToken).GetAwaiter().GetResult();
                }
                else
                {
                    var collection = GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and displayName eq '{_stringValue}')&$select=Id", accessToken).GetAwaiter().GetResult();
                    if (collection != null && collection.Items.Any())
                    {
                        if (collection.Items.Count() == 1)
                        {
                            return GraphHelper.GetAsync<Team>(httpClient, $"v1.0/teams/{collection.Items.First().Id}", accessToken).GetAwaiter().GetResult();
                        }
                        else
                        {
                            throw new PSArgumentException("We found more matches based on the identity value you entered. Use Get-PnPTeamsTeam to find the correct instance and use the GroupId from a team to select the correct team instead.");
                        }
                    }
                    else
                    {
                        collection = GraphHelper.GetAsync<GraphCollection<Model.Teams.Group>>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and mailNickname eq '{_stringValue}')&$select=Id", accessToken).GetAwaiter().GetResult();
                        if (collection != null && collection.Items.Count() == 1)
                        {
                            return GraphHelper.GetAsync<Team>(httpClient, $"v1.0/teams/{collection.Items.First().Id}", accessToken).GetAwaiter().GetResult();
                        }
                    }
                }
            }
            catch (GraphException ex)
            {
                throw new PSInvalidOperationException(ex.Error.Message);
            }
            return null;
        }

    }
}
