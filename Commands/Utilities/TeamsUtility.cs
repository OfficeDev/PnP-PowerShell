using SharePointPnP.PowerShell.Commands.Model.Teams;
using SharePointPnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
#if !NETSTANDARD2_1
using System.Web;
#endif

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    internal static class TeamsUtility
    {
        private const int PageSize = 100;

        #region Team
        public static List<Group> GetGroupsWithTeam(HttpClient httpClient, string accessToken)
        {
            List<Group> groups = new List<Group>();
            string url = string.Empty;
            var collection = GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, $"beta/groups?$filter=resourceProvisioningOptions/Any(x:x eq 'Team')&$select=Id,DisplayName,MailNickName,Description,Visibility&$top={PageSize}", accessToken).GetAwaiter().GetResult(); ;
            if (collection != null)
            {
                groups.AddRange(collection.Items);
                while (!string.IsNullOrEmpty(collection.NextLink))
                {
                    collection = GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, collection.NextLink, accessToken).GetAwaiter().GetResult();
                    groups.AddRange(collection.Items);
                }
            }
            return groups;
        }

        public static Group GetGroupWithTeam(HttpClient httpClient, string accessToken, string mailNickname)
        {
            return GraphHelper.GetAsync<Group>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and mailNickname eq '{mailNickname}')&$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken).GetAwaiter().GetResult();

        }
        public static List<Team> GetTeams(string accessToken, HttpClient httpClient)
        {
            List<Team> teams = new List<Team>();

            var groups = GetGroupsWithTeam(httpClient, accessToken);
            foreach (var group in groups)
            {
                Team team = ParseTeamJson(accessToken, httpClient, group.Id);

                if (team != null)
                {
                    team.DisplayName = group.DisplayName;
                    team.MailNickname = group.MailNickname;
                    team.Visibility = group.Visibility;
                    teams.Add(team);
                }
            }
            return teams;
        }

        public static Team GetTeam(string accessToken, HttpClient httpClient, string groupId)
        {
            // get the group
            var group = GraphHelper.GetAsync<Group>(httpClient, $"v1.0/groups/{groupId}?$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken).GetAwaiter().GetResult();

            Team team = ParseTeamJson(accessToken, httpClient, group.Id);
            if (team != null)
            {
                team.DisplayName = group.DisplayName;
                team.MailNickname = group.MailNickname;
                team.Visibility = group.Visibility;
                return team;
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteTeam(string accessToken, HttpClient httpClient, string groupId)
        {
            return GraphHelper.DeleteAsync(httpClient, $"v1.0/groups/{groupId}", accessToken).GetAwaiter().GetResult();
        }

        private static Team ParseTeamJson(string accessToken, HttpClient httpClient, string groupId)
        {
            // Get Settings
            try
            {
                var team = GraphHelper.GetAsync<Team>(httpClient, $"v1.0/teams/{groupId}", accessToken).GetAwaiter().GetResult();
                if (team != null)
                {
                    team.GroupId = groupId;
                    return team;
                }
                else
                {
                    return null;
                }

                //team = GetTeamChannels(configuration, accessToken, groupId, team, scope);
                //team = GetTeamApps(accessToken, groupId, team, scope);
                //team = GetTeamSecurity(accessToken, groupId, team, scope);
                //GetTeamPhoto(configuration, accessToken, groupId, team, scope);
            }
            catch (ApplicationException ex)
            {
#if !NETSTANDARD2_1
                if (ex.InnerException is HttpException)
                {
                    if (((HttpException)ex.InnerException).GetHttpCode() == 404)
                    {
                        // no team, swallow
                        return null;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
#else
                // untested change
                if (ex.Message.StartsWith("404"))
                {
                    // no team, swallow
                }
                else
                {
                    throw ex;
                }
#endif
                return null;
            }
        }

        public static Team NewTeam(string accessToken, HttpClient httpClient, string groupId, string displayName, string description, string classification, string mailNickname, string owner, GroupVisibility visibility, TeamCreationInformation teamCI)
        {
            Group group = null;
            Team returnTeam = null;
            // Create group
            if (string.IsNullOrEmpty(groupId))
            {
                group = CreateGroup(accessToken, httpClient, displayName, description, classification, mailNickname, owner, visibility);
            }
            else
            {
                group = GraphHelper.GetAsync<Group>(httpClient, $"v1.0/groups/{groupId}", accessToken).GetAwaiter().GetResult();
                if (group == null)
                {
                    throw new PSArgumentException($"Cannot find group with id {groupId}");
                }
                teamCI.Visibility = group.Visibility;
                teamCI.Description = group.Description;
            }
            if (group != null)
            {
                Team team = teamCI.ToTeam();
                var teamSettings = GraphHelper.PutAsync(httpClient, $"v1.0/groups/{group.Id}/team", team, accessToken).GetAwaiter().GetResult();
                if (teamSettings != null)
                {
                    returnTeam = TeamsUtility.GetTeam(accessToken, httpClient, group.Id);
                }
            }
            return returnTeam;
        }

        private static Group CreateGroup(string accessToken, HttpClient httpClient, string displayName, string description, string classification, string mailNickname, string owner, GroupVisibility visibility)
        {
            Group group = new Group();
            // get the owner if no owner was specified
            var ownerId = string.Empty;
            if (string.IsNullOrEmpty(owner))
            {
                var user = GraphHelper.GetAsync<User>(httpClient, "v1.0/me?$select=Id", accessToken).GetAwaiter().GetResult();
                ownerId = user.Id;
            }
            else
            {
                var user = GraphHelper.GetAsync<User>(httpClient, $"v1.0/users/{owner}?$select=Id", accessToken).GetAwaiter().GetResult();
                if (user != null)
                {
                    ownerId = user.Id;
                }
                else
                {
                    // find the user in the organization
                    var collection = GraphHelper.GetAsync<GraphCollection<User>>(httpClient, "v1.0/myorganization/users?$filter=mail eq '{owner}'&$select=Id", accessToken).GetAwaiter().GetResult();
                    if (collection != null)
                    {
                        if (collection.Items.Any())
                        {
                            ownerId = collection.Items.First().Id;
                        }
                    }
                }
            }

            group.DisplayName = displayName;
            group.Description = description;
            group.Classification = classification;
            group.MailEnabled = true;
            group.MailNickname = mailNickname ?? CreateAlias(httpClient, accessToken);
            group.GroupTypes = new List<string>() { "Unified" };
            group.SecurityEnabled = false;
            group.Owners = new List<string>() { $"https://graph.microsoft.com/v1.0/users/{ownerId}" };
            group.Members = new List<string>() { $"https://graph.microsoft.com/v1.0/users/{ownerId}" };
            group.Visibility = visibility == GroupVisibility.NotSpecified ? GroupVisibility.Private : visibility;

            return GraphHelper.PostAsync<Group>(httpClient, "v1.0/groups", group, accessToken).GetAwaiter().GetResult();

        }

        private static string CreateAlias(HttpClient httpClient, string accessToken)
        {
            var guid = Guid.NewGuid().ToString();
            var teamName = string.Empty;
            // check if the group exists
            do
            {
                var teamNameTemp = $"msteams_{guid.Substring(0, 8)}{guid.Substring(9, 4)}";
                var collection = GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, $"v1.0/groups?$filter=groupTypes/any(c:c+eq+'Unified') and (mailNickname eq '{teamNameTemp}')", accessToken).GetAwaiter().GetResult();
                if (collection != null)
                {
                    if (!collection.Items.Any()) teamName = teamNameTemp;
                }

            } while (teamName == string.Empty);
            return teamName;
        }
        #endregion

        #region Channel
        public static IEnumerable<TeamChannel> GetChannels(string accessToken, HttpClient httpClient, string groupId)
        {
            var collection = GraphHelper.GetAsync<GraphCollection<TeamChannel>>(httpClient, $"v1.0/teams/{groupId}/channels", accessToken).GetAwaiter().GetResult();
            if (collection != null)
            {
                return collection.Items;
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteChannel(string accessToken, HttpClient httpClient, string groupId, string displayName)
        {
            // find the channel
            var channels = GetChannels(accessToken, httpClient, groupId);
            var channel = channels.FirstOrDefault(c => c.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase));
            if (channel != null)
            {
                return GraphHelper.DeleteAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channel.Id}", accessToken).GetAwaiter().GetResult();
            }
            else
            {
                return false;
            }
        }

        public static TeamChannel AddChannel(string accessToken, HttpClient httpClient, string groupId, string displayName, string description)
        {
            var channel = new TeamChannel()
            {
                Description = description,
                DisplayName = displayName,
            };
            return GraphHelper.PostAsync<TeamChannel>(httpClient, $"v1.0/teams/{groupId}/channels", channel, accessToken).GetAwaiter().GetResult();
        }

        public static void PostMessage(HttpClient httpClient, string accessToken, string groupId, string channelId, TeamChannelMessage message)
        {
            GraphHelper.PostAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/messages", message, accessToken).GetAwaiter().GetResult();
        }

        public static List<TeamChannelMessage> GetMessages(HttpClient httpClient, string accessToken, string groupId, string channelId, bool includeDeleted = false)
        {
            List<TeamChannelMessage> messages = new List<TeamChannelMessage>();
            var collection = GraphHelper.GetAsync<GraphCollection<TeamChannelMessage>>(httpClient, $"beta/teams/{groupId}/channels/{channelId}/messages", accessToken).GetAwaiter().GetResult();
            if(collection != null)
            {
                messages.AddRange(collection.Items);
                while(collection != null && !string.IsNullOrEmpty(collection.NextLink))
                {
                    collection = GraphHelper.GetAsync<GraphCollection<TeamChannelMessage>>(httpClient, collection.NextLink, accessToken).GetAwaiter().GetResult();
                    if (collection != null)
                    {
                        messages.AddRange(collection.Items);
                    }
                }
            }
            if (includeDeleted)
            {
                return messages;
            } else
            {
                return messages.Where(m => !m.DeletedDateTime.HasValue).ToList();
            }
        }
        #endregion

        #region Tabs
        public static IEnumerable<TeamTab> GetTabs(string accessToken, HttpClient httpClient, string groupId, string channelId)
        {
            var collection = GraphHelper.GetAsync<GraphCollection<TeamTab>>(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs", accessToken).GetAwaiter().GetResult();
            if (collection != null)
            {
                return collection.Items;
            }
            return null;
        }

        public static TeamTab GetTab(string accessToken, HttpClient httpClient, string groupId, string channelId, string tabId)
        {
            return GraphHelper.GetAsync<TeamTab>(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs/{tabId}", accessToken).GetAwaiter().GetResult();
        }

        public static bool DeleteTab(string accessToken, HttpClient httpClient, string groupId, string channelId, string tabId)
        {
            return GraphHelper.DeleteAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs/{tabId}", accessToken).GetAwaiter().GetResult();
        }

        #endregion

        #region Apps
        public static IEnumerable<TeamApp> GetApps(string accessToken, HttpClient httpClient)
        {
            var collection = GraphHelper.GetAsync<GraphCollection<TeamApp>>(httpClient, $"v1.0/appCatalogs/teamsApps", accessToken).GetAwaiter().GetResult();
            if (collection != null)
            {
                return collection.Items;
            }
            return null;
        }
        #endregion
    }
}