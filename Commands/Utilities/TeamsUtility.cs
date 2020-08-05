using OfficeDevPnP.Core.Entities;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Principals;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
#if !PNPPSCORE
using System.Web;
#endif

namespace PnP.PowerShell.Commands.Utilities
{
    internal static class TeamsUtility
    {
        private const int PageSize = 100;

        #region Team
        public static async Task<List<Group>> GetGroupsWithTeamAsync(HttpClient httpClient, string accessToken)
        {
            List<Group> groups = new List<Group>();
            string url = string.Empty;
            var collection = await GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, $"beta/groups?$filter=resourceProvisioningOptions/Any(x:x eq 'Team')&$select=Id,DisplayName,MailNickName,Description,Visibility&$top={PageSize}", accessToken);
            if (collection != null)
            {
                groups.AddRange(collection.Items);
                while (!string.IsNullOrEmpty(collection.NextLink))
                {
                    collection = await GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, collection.NextLink, accessToken);
                    groups.AddRange(collection.Items);
                }
            }
            return groups;
        }

        public static async Task<Group> GetGroupWithTeamAsync(HttpClient httpClient, string accessToken, string mailNickname)
        {
            return await GraphHelper.GetAsync<Group>(httpClient, $"beta/groups?$filter=(resourceProvisioningOptions/Any(x:x eq 'Team') and mailNickname eq '{mailNickname}')&$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken);

        }
        public static async Task<List<Team>> GetTeamsAsync(string accessToken, HttpClient httpClient)
        {
            List<Team> teams = new List<Team>();

            var groups = await GetGroupsWithTeamAsync(httpClient, accessToken);
            foreach (var group in groups)
            {
                Team team = await ParseTeamJsonAsync(accessToken, httpClient, group.Id);

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

        public static async Task<Team> GetTeamAsync(string accessToken, HttpClient httpClient, string groupId)
        {
            // get the group
            var group = await GraphHelper.GetAsync<Group>(httpClient, $"v1.0/groups/{groupId}?$select=Id,DisplayName,MailNickName,Description,Visibility", accessToken);

            Team team = await ParseTeamJsonAsync(accessToken, httpClient, group.Id);
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

        public static async Task<HttpResponseMessage> DeleteTeamAsync(string accessToken, HttpClient httpClient, string groupId)
        {
            return await GraphHelper.DeleteAsync(httpClient, $"v1.0/groups/{groupId}", accessToken);
        }

        private static async Task<Team> ParseTeamJsonAsync(string accessToken, HttpClient httpClient, string groupId)
        {
            // Get Settings
            try
            {
                var team = await GraphHelper.GetAsync<Team>(httpClient, $"v1.0/teams/{groupId}", accessToken);
                if (team != null)
                {
                    team.GroupId = groupId;
                    return team;
                }
                else
                {
                    return null;
                }
            }
            catch (ApplicationException ex)
            {
#if !PNPPSCORE
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
                return null;
#endif
            }
        }

        public static async Task<Team> NewTeamAsync(string accessToken, HttpClient httpClient, string groupId, string displayName, string description, string classification, string mailNickname, string owner, GroupVisibility visibility, TeamCreationInformation teamCI)
        {
            Group group = null;
            Team returnTeam = null;
            // Create group
            if (string.IsNullOrEmpty(groupId))
            {
                group = await CreateGroupAsync(accessToken, httpClient, displayName, description, classification, mailNickname, owner, visibility);
            }
            else
            {
                group = await GraphHelper.GetAsync<Group>(httpClient, $"v1.0/groups/{groupId}", accessToken);
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
                var teamSettings = await GraphHelper.PutAsync(httpClient, $"v1.0/groups/{group.Id}/team", team, accessToken);
                if (teamSettings != null)
                {
                    returnTeam = await TeamsUtility.GetTeamAsync(accessToken, httpClient, group.Id);
                }
            }
            return returnTeam;
        }

        private static async Task<Group> CreateGroupAsync(string accessToken, HttpClient httpClient, string displayName, string description, string classification, string mailNickname, string owner, GroupVisibility visibility)
        {
            Group group = new Group();
            // get the owner if no owner was specified
            var ownerId = string.Empty;
            if (string.IsNullOrEmpty(owner))
            {
                var user = await GraphHelper.GetAsync<User>(httpClient, "v1.0/me?$select=Id", accessToken);
                ownerId = user.Id;
            }
            else
            {
                var user = await GraphHelper.GetAsync<User>(httpClient, $"v1.0/users/{owner}?$select=Id", accessToken);
                if (user != null)
                {
                    ownerId = user.Id;
                }
                else
                {
                    // find the user in the organization
                    var collection = await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, "v1.0/myorganization/users?$filter=mail eq '{owner}'&$select=Id", accessToken);
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
            group.MailNickname = mailNickname ?? await CreateAliasAsync(httpClient, accessToken);
            group.GroupTypes = new List<string>() { "Unified" };
            group.SecurityEnabled = false;
            group.Owners = new List<string>() { $"https://graph.microsoft.com/v1.0/users/{ownerId}" };
            group.Members = new List<string>() { $"https://graph.microsoft.com/v1.0/users/{ownerId}" };
            group.Visibility = visibility == GroupVisibility.NotSpecified ? GroupVisibility.Private : visibility;

            return await GraphHelper.PostAsync<Group>(httpClient, "v1.0/groups", group, accessToken);

        }

        private static async Task<string> CreateAliasAsync(HttpClient httpClient, string accessToken)
        {
            var guid = Guid.NewGuid().ToString();
            var teamName = string.Empty;
            // check if the group exists
            do
            {
                var teamNameTemp = $"msteams_{guid.Substring(0, 8)}{guid.Substring(9, 4)}";
                var collection = await GraphHelper.GetAsync<GraphCollection<Group>>(httpClient, $"v1.0/groups?$filter=groupTypes/any(c:c+eq+'Unified') and (mailNickname eq '{teamNameTemp}')", accessToken);
                if (collection != null)
                {
                    if (!collection.Items.Any()) teamName = teamNameTemp;
                }

            } while (teamName == string.Empty);
            return teamName;
        }

        public static async Task<Team> UpdateTeamAsync(HttpClient httpClient, string accessToken, string groupId, Team team)
        {
            return await GraphHelper.PatchAsync<Team>(httpClient, accessToken, $"v1.0/teams/{groupId}", team);
        }

        public static async Task<Group> UpdateGroupAsync(HttpClient httpClient, string accessToken, string groupId, Group group)
        {
            return await GraphHelper.PatchAsync<Group>(httpClient, accessToken, $"v1.0/groups/{groupId}", group);
        }

        public static async Task SetTeamPictureAsync(HttpClient httpClient, string accessToken, string groupId, byte[] bytes, string contentType)
        {
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            await GraphHelper.PutAsync<string>(httpClient, $"v1.0/groups/{groupId}/photo/$value", accessToken, byteArrayContent);
        }

        public static async Task<HttpResponseMessage> SetTeamArchivedStateAsync(HttpClient httpClient, string accessToken, string groupId, bool archived, bool? setSiteReadOnly)
        {
            if (archived)
            {
                StringContent content = new StringContent(JsonSerializer.Serialize(setSiteReadOnly.HasValue ? new { shouldSetSpoSiteReadOnlyForMembers = setSiteReadOnly } : null));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await GraphHelper.PostAsync(httpClient, $"v1.0/teams/{groupId}/archive", accessToken, content);
            }
            else
            {
                StringContent content = new StringContent("");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await GraphHelper.PostAsync(httpClient, $"v1.0/teams/{groupId}/unarchive", accessToken, content);
            }
        }
        #endregion

        #region Users
        public static async Task AddUserAsync(HttpClient httpClient, string accessToken, string groupId, string upn, string role)
        {
            var user = await GraphHelper.GetAsync<User>(httpClient, $"v1.0/users/{upn}", accessToken);

            // check if the user is a member
            bool isMember = false;
            try
            {
                var members = await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, $"v1.0/groups/{groupId}/members?$filter=Id eq '{user.Id}'&$select=Id", accessToken);
                isMember = members.Items.Any();
            }
            catch (GraphException)
            { }

            bool isOwner = false;
            try
            {
                var owners = await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, $"v1.0/groups/{groupId}/owners?$filter=Id eq '{user.Id}'&$select=Id", accessToken);
                isOwner = owners.Items.Any();
            }
            catch (GraphException)
            {

            }

            var value = new Dictionary<string, object>
            {
                {
                    "@odata.id",
                    $"https://graph.microsoft.com/v1.0/directoryObjects/{user.Id}"
                }
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(value));
            stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            if (role == "Owner")
            {
                if (!isMember)
                {
                    await GraphHelper.PostAsync(httpClient, $"v1.0/groups/{groupId}/members/$ref", accessToken, stringContent);
                }
                await GraphHelper.PostAsync(httpClient, $"v1.0/groups/{groupId}/owners/$ref", accessToken, stringContent);
            }
            else
            {
                await GraphHelper.PostAsync(httpClient, $"v1.0/groups/{groupId}/members/$ref", accessToken, stringContent);
            }
        }

        public static async Task<List<User>> GetUsersAsync(HttpClient httpClient, string accessToken, string groupId, string role)
        {
            var selectedRole = role != null ? role.ToLower() : null;
            var owners = new List<User>();
            var guests = new List<User>();
            var members = new List<User>();
            if (selectedRole != "guest")
            {
                owners = (await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, $"v1.0/groups/{groupId}/owners?$select=Id,displayName,userPrincipalName,userType", accessToken)).Items.Select(t => new User()
                {
                    Id = t.Id,
                    DisplayName = t.DisplayName,
                    UserPrincipalName = t.UserPrincipalName,
                    UserType = "Owner"
                }).ToList();
            }
            if (selectedRole != "owner")
            {
                var users = (await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, $"v1.0/groups/{groupId}/members?$select=Id,displayName,userPrincipalName,userType", accessToken)).Items;
                HashSet<string> hashSet = new HashSet<string>(owners.Select(u => u.Id));
                foreach (var user in users)
                {
                    if (!hashSet.Contains(user.Id))
                    {
                        if (user.UserType != null && user.UserType.ToLower().Equals("guest"))
                        {
                            guests.Add(new User() { DisplayName = user.DisplayName, Id = user.Id, UserPrincipalName = user.UserPrincipalName, UserType = "Guest" });
                        }
                        else
                        {
                            members.Add(new User() { DisplayName = user.DisplayName, Id = user.Id, UserPrincipalName = user.UserPrincipalName, UserType = "Member" });
                        }
                    }
                }
            }
            var finalList = new List<User>();
            if (string.IsNullOrEmpty(selectedRole))
            {
                finalList.AddRange(owners);
                finalList.AddRange(members);
                finalList.AddRange(guests);
            }
            else if (selectedRole == "owner")
            {
                finalList.AddRange(owners);
            }
            else if (selectedRole == "member")
            {
                finalList.AddRange(members);
            }
            else if (selectedRole == "guest")
            {
                finalList.AddRange(guests);
            }
            return finalList;
        }

        public static async Task<IEnumerable<User>> GetUsersAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, string role)
        {
            List<User> users = new List<User>();
            var selectedRole = role != null ? role.ToLower() : null;

            var collection = await GraphHelper.GetAsync<GraphCollection<TeamChannelMember>>(httpClient, $"beta/teams/{groupId}/channels/{channelId}/members", accessToken);
            if (collection != null && collection.Items.Any())
            {
                users.AddRange(collection.Items.Select(m => new User() { DisplayName = m.DisplayName, Id = m.UserId, UserPrincipalName = m.email, UserType = m.Roles[0].ToLower() }));
            }
            while (collection.NextLink != null)
            {
                collection = await GraphHelper.GetAsync<GraphCollection<TeamChannelMember>>(httpClient, collection.NextLink, accessToken);
                if (collection != null && collection.Items.Any())
                {
                    users.AddRange(collection.Items.Select(m => new User() { DisplayName = m.DisplayName, Id = m.UserId, UserPrincipalName = m.email, UserType = m.Roles[0].ToLower() }));
                }
            }
            if (selectedRole != null)
            {
                return users.Where(u => u.UserType == selectedRole);
            }
            else
            {
                return users;
            }
        }

        public static async Task DeleteUserAsync(HttpClient httpClient, string accessToken, string groupId, string upn, string role)
        {
            var user = await GraphHelper.GetAsync<User>(httpClient, $"v1.0/users/{upn}?$select=Id", accessToken);
            if (user != null)
            {
                // check if the user is an owner
                var owners = await GraphHelper.GetAsync<GraphCollection<User>>(httpClient, $"v1.0/groups/{groupId}/owners?$select=Id", accessToken);
                if (owners.Items.Any() && owners.Items.FirstOrDefault(u => u.Id.Equals(user.Id, StringComparison.OrdinalIgnoreCase)) != null)
                {
                    if (owners.Items.Count() == 1)
                    {
                        throw new PSInvalidOperationException("Last owner cannot be removed");
                    }
                    await GraphHelper.DeleteAsync(httpClient, $"v1.0/groups/{groupId}/owners/{user.Id}/$ref", accessToken);
                }
                if (!role.Equals("owner", StringComparison.OrdinalIgnoreCase))
                {
                    await GraphHelper.DeleteAsync(httpClient, $"v1.0/groups/{groupId}/members/{user.Id}/$ref", accessToken);
                }
            }
        }

        #endregion

        #region Channel
        public static async Task<IEnumerable<TeamChannel>> GetChannelsAsync(string accessToken, HttpClient httpClient, string groupId)
        {
            var collection = await GraphHelper.GetAsync<GraphCollection<TeamChannel>>(httpClient, $"beta/teams/{groupId}/channels", accessToken);
            if (collection != null)
            {
                return collection.Items;
            }
            else
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> DeleteChannelAsync(string accessToken, HttpClient httpClient, string groupId, string channelId)
        {
            return await GraphHelper.DeleteAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}", accessToken);
        }

        public static async Task<TeamChannel> AddChannelAsync(string accessToken, HttpClient httpClient, string groupId, string displayName, string description, bool isPrivate, string ownerUPN)
        {
            var channel = new TeamChannel()
            {
                Description = description,
                DisplayName = displayName,
            };
            if (isPrivate)
            {
                channel.MembershipType = "private";
            }
            if (isPrivate)
            {
                channel.Type = "#Microsoft.Teams.Core.channel";
                var user = await GraphHelper.GetAsync<User>(httpClient, $"v1.0/users/{ownerUPN}", accessToken);
                channel.Members = new List<TeamChannelMember>();
                channel.Members.Add(new TeamChannelMember() { Roles = new List<string> { "owner" }, UserIdentifier = $"https://graph.microsoft.com/beta/users/('{user.Id}')" });
                return await GraphHelper.PostAsync<TeamChannel>(httpClient, $"beta/teams/{groupId}/channels", channel, accessToken);
            }
            else
            {
                return await GraphHelper.PostAsync<TeamChannel>(httpClient, $"v1.0/teams/{groupId}/channels", channel, accessToken);
            }
        }

        public static async Task PostMessageAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, TeamChannelMessage message)
        {
            await GraphHelper.PostAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/messages", message, accessToken);
        }

        public static async Task<List<TeamChannelMessage>> GetMessagesAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, bool includeDeleted = false)
        {
            List<TeamChannelMessage> messages = new List<TeamChannelMessage>();
            var collection = await GraphHelper.GetAsync<GraphCollection<TeamChannelMessage>>(httpClient, $"beta/teams/{groupId}/channels/{channelId}/messages", accessToken);
            if (collection != null)
            {
                messages.AddRange(collection.Items);
                while (collection != null && !string.IsNullOrEmpty(collection.NextLink))
                {
                    collection = await GraphHelper.GetAsync<GraphCollection<TeamChannelMessage>>(httpClient, collection.NextLink, accessToken);
                    if (collection != null)
                    {
                        messages.AddRange(collection.Items);
                    }
                }
            }
            if (includeDeleted)
            {
                return messages;
            }
            else
            {
                return messages.Where(m => !m.DeletedDateTime.HasValue).ToList();
            }
        }

        public static async Task<TeamChannel> UpdateChannelAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, TeamChannel channel)
        {
            return await GraphHelper.PatchAsync(httpClient, accessToken, $"beta/teams/{groupId}/channels/{channelId}", channel);
        }
        #endregion

        #region Tabs
        public static async Task<IEnumerable<TeamTab>> GetTabsAsync(string accessToken, HttpClient httpClient, string groupId, string channelId)
        {
            var collection = await GraphHelper.GetAsync<GraphCollection<TeamTab>>(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs", accessToken);
            if (collection != null)
            {
                return collection.Items;
            }
            return null;
        }

        public static async Task<TeamTab> GetTabAsync(string accessToken, HttpClient httpClient, string groupId, string channelId, string tabId)
        {
            return await GraphHelper.GetAsync<TeamTab>(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs/{tabId}", accessToken);
        }

        public static async Task<HttpResponseMessage> DeleteTabAsync(string accessToken, HttpClient httpClient, string groupId, string channelId, string tabId)
        {
            return await GraphHelper.DeleteAsync(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs/{tabId}", accessToken);
        }

        public static async Task UpdateTabAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, TeamTab tab)
        {
            tab.Configuration = null;
            await GraphHelper.PatchAsync(httpClient, accessToken, $"v1.0/teams/{groupId}/channels/{channelId}/tabs/{tab.Id}", tab);
        }

        public static async Task<TeamTab> AddTabAsync(HttpClient httpClient, string accessToken, string groupId, string channelId, string displayName, TeamTabType tabType, string teamsAppId, string entityId, string contentUrl, string removeUrl, string websiteUrl)
        {
            TeamTab tab = new TeamTab();
            switch (tabType)
            {
                case TeamTabType.Custom:
                    {
                        tab.TeamsAppId = teamsAppId;
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = entityId;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = removeUrl;
                        tab.Configuration.WebsiteUrl = websiteUrl;
                        break;
                    }
                case TeamTabType.DocumentLibrary:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.files.sharepoint";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = "";
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = null;
                        break;
                    }
                case TeamTabType.WebSite:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.web";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = null;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = contentUrl;
                        break;
                    }
                case TeamTabType.Word:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.file.staticviewer.word";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = entityId;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = null;
                        break;
                    }
                case TeamTabType.Excel:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.file.staticviewer.excel";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = entityId;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = null;
                        break;
                    }
                case TeamTabType.PowerPoint:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.file.staticviewer.powerpoint";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = entityId;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = null;
                        break;
                    }
                case TeamTabType.PDF:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.file.staticviewer.pdf";
                        tab.Configuration = new TeamTabConfiguration();
                        tab.Configuration.EntityId = entityId;
                        tab.Configuration.ContentUrl = contentUrl;
                        tab.Configuration.RemoveUrl = null;
                        tab.Configuration.WebsiteUrl = null;
                        break;
                    }
                case TeamTabType.Wiki:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.wiki";
                        break;
                    }
                case TeamTabType.Planner:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.planner";
                        break;
                    }
                case TeamTabType.MicrosoftStream:
                    {
                        tab.TeamsAppId = "com.microsoftstream.embed.skypeteamstab";
                        break;
                    }
                case TeamTabType.MicrosoftForms:
                    {
                        tab.TeamsAppId = "81fef3a6-72aa-4648-a763-de824aeafb7d";
                        break;
                    }
                case TeamTabType.OneNote:
                    {
                        tab.TeamsAppId = "0d820ecd-def2-4297-adad-78056cde7c78";
                        break;
                    }
                case TeamTabType.PowerBI:
                    {
                        tab.TeamsAppId = "com.microsoft.teamspace.tab.powerbi";
                        break;
                    }
                case TeamTabType.SharePointPageAndList:
                    {
                        tab.TeamsAppId = "2a527703-1f6f-4559-a332-d8a7d288cd88";
                        break;
                    }
            }
            tab.DisplayName = displayName;
            tab.TeamsApp = $"https://graph.microsoft.com/v1.0/appCatalogs/teamsApps/{tab.TeamsAppId}";
            return await GraphHelper.PostAsync<TeamTab>(httpClient, $"v1.0/teams/{groupId}/channels/{channelId}/tabs", tab, accessToken);
        }
        #endregion

        #region Apps
        public static async Task<IEnumerable<TeamApp>> GetAppsAsync(string accessToken, HttpClient httpClient)
        {
            var collection = await GraphHelper.GetAsync<GraphCollection<TeamApp>>(httpClient, $"v1.0/appCatalogs/teamsApps", accessToken);
            if (collection != null)
            {
                return collection.Items;
            }
            return null;
        }

        public static async Task<TeamApp> AddAppAsync(HttpClient httpClient, string accessToken, byte[] bytes)
        {
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/zip");
            var response = await GraphHelper.PostAsync(httpClient, "v1.0/appCatalogs/teamsApps", accessToken, byteArrayContent);
            if (!response.IsSuccessStatusCode)
            {
                if (GraphHelper.TryGetGraphException(response, out GraphException exception))
                {
                    throw exception;
                }
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonSerializer.Deserialize<TeamApp>(content, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            return null;
        }

        public static async Task<HttpResponseMessage> UpdateAppAsync(HttpClient httpClient, string accessToken, byte[] bytes, string appId)
        {
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/zip");
            return await GraphHelper.PutAsync(httpClient, $"v1.0/appCatalogs/teamsApps/{appId}", accessToken, byteArrayContent);
        }

        public static async Task<HttpResponseMessage> DeleteAppAsync(HttpClient httpClient, string accessToken, string appId)
        {
            return await GraphHelper.DeleteAsync(httpClient, $"v1.0/appCatalogs/teamsApps/{appId}", accessToken);
        }
        #endregion
    }
}