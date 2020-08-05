using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public class TeamCreationInformation
    {
        public string GroupId { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GroupVisibility Visibility { get; set; }

        public string Classification { get; set; }

        public bool? Archived { get; set; }

        public bool? AllowGiphy { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TeamGiphyContentRating GiphyContentRating { get; set; }

        public bool? AllowStickersAndMemes { get; set; }

        public bool? AllowCustomMemes { get; set; }

        public bool? AllowGuestCreateUpdateChannels { get; set; }

        public bool? AllowGuestDeleteChannels { get; set; }

        public bool? AllowCreateUpdateChannels { get; set; }

        public bool? AllowDeleteChannels { get; set; }

        public bool? AllowAddRemoveApps { get; set; }

        public bool? AllowCreateUpdateRemoveTabs { get; set; }

        public bool? AllowCreateUpdateRemoveConnectors { get; set; }

        public bool? AllowUserEditMessages { get; set; }

        public bool? AllowUserDeleteMessages { get; set; }

        public bool? AllowOwnerDeleteMessages { get; set; }

        public bool? AllowTeamMentions { get; set; }

        public bool? AllowChannelMentions { get; set; }

        public bool? ShowInTeamsSearchAndSuggestions { get; set; }

        public TeamCreationInformation()
        {
        }

        public TeamCreationInformation(Team team)
        {
            GroupId = team.GroupId;
            DisplayName = team.DisplayName;
            Description = team.Description;
            Visibility = team.Visibility.Value;
            Archived = team.IsArchived;
            Classification = team.Classification;
            AllowGiphy = team.FunSettings.AllowGiphy;
            GiphyContentRating = team.FunSettings.GiphyContentRating;
            AllowStickersAndMemes = team.FunSettings.AllowStickersAndMemes;
            AllowCustomMemes = team.FunSettings.AllowCustomMemes;
            AllowGuestCreateUpdateChannels = team.GuestSettings.AllowCreateUpdateChannels;
            AllowGuestDeleteChannels = team.GuestSettings.AllowDeleteChannels;
            AllowCreateUpdateChannels = team.MemberSettings.AllowCreateUpdateChannels;
            AllowDeleteChannels = team.MemberSettings.AllowDeleteChannels;
            AllowAddRemoveApps = team.MemberSettings.AllowAddRemoveApps;
            AllowCreateUpdateRemoveTabs = team.MemberSettings.AllowCreateUpdateRemoveTabs;
            AllowCreateUpdateRemoveConnectors = team.MemberSettings.AllowCreateUpdateRemoveConnectors;
            AllowUserEditMessages = team.MessagingSettings.AllowUserEditMessages;
            AllowUserDeleteMessages = team.MessagingSettings.AllowUserDeleteMessages;
            AllowOwnerDeleteMessages = team.MessagingSettings.AllowOwnerDeleteMessages;
            AllowTeamMentions = team.MessagingSettings.AllowTeamMentions;
            AllowChannelMentions = team.MessagingSettings.AllowChannelMentions;
            ShowInTeamsSearchAndSuggestions = team.DiscoverySettings.ShowInTeamsSearchAndSuggestions;
        }

        public Team ToTeam()
        {
            return new Team
            {
                FunSettings = new TeamFunSettings
                {
                    AllowGiphy = AllowGiphy,
                    AllowCustomMemes = AllowCustomMemes,
                    GiphyContentRating = GiphyContentRating,
                    AllowStickersAndMemes = AllowStickersAndMemes
                },
                GuestSettings = new TeamGuestSettings
                {
                    AllowCreateUpdateChannels = AllowGuestCreateUpdateChannels,
                    AllowDeleteChannels = AllowGuestDeleteChannels
                },
                MemberSettings = new TeamMemberSettings
                {
                    AllowCreateUpdateChannels = AllowCreateUpdateChannels,
                    AllowDeleteChannels = AllowDeleteChannels,
                    AllowAddRemoveApps = AllowAddRemoveApps,
                    AllowCreateUpdateRemoveTabs = AllowCreateUpdateRemoveTabs,
                    AllowCreateUpdateRemoveConnectors = AllowCreateUpdateRemoveConnectors
                },
                MessagingSettings = new TeamMessagingSettings
                {
                    AllowUserEditMessages = AllowUserEditMessages,
                    AllowUserDeleteMessages = AllowUserDeleteMessages,
                    AllowOwnerDeleteMessages = AllowOwnerDeleteMessages,
                    AllowTeamMentions = AllowTeamMentions,
                    AllowChannelMentions = AllowChannelMentions
                },
                DiscoverySettings = new TeamDiscoverySettings
                {
                    ShowInTeamsSearchAndSuggestions = ShowInTeamsSearchAndSuggestions
                }
            };
        }

        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
