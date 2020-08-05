#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model.Teams;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.New, "PnPTeamsTeam")]
    [CmdletHelp("Creates a new Team in Microsoft Teams. The cmdlet will create a Microsoft 365 group and then add a team to the group.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTeam",
       Remarks = "Retrieves all the Microsoft Teams instances",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTeam -GroupId $groupId",
       Remarks = "Retrieves a specific Microsoft Teams instance",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTeam -Visibility Public",
       Remarks = "Retrieves all Microsoft Teams instances which are public visible",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class NewTeamsTeam : PnPGraphCmdlet
    {
        private const string ParameterSet_EXISTINGGROUP = "For an existing group";
        private const string ParameterSet_NEWGROUP = "For a new group";

        [Parameter(Mandatory = true, ParameterSetName =ParameterSet_EXISTINGGROUP, HelpMessage = "Specify a GroupId to convert to a Team. If specified, you cannot provide the other values that are already specified by the existing group, namely: Visibility, Alias, Description, or DisplayName.")]
        public string GroupId;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_NEWGROUP, HelpMessage = "Team display name. Characters Limit - 256.")]
        [ValidateLength(1, 256)]
        public string DisplayName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NEWGROUP, HelpMessage = "The MailNickName parameter specifies the alias for the associated Microsoft 365 Group. This value will be used for the mail enabled object and will be used as PrimarySmtpAddress for this Microsoft 365 Group.The value of the MailNickName parameter has to be unique across your tenant.")]
        public string MailNickName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NEWGROUP, HelpMessage = "Team description. Characters Limit - 1024.")]
        [ValidateLength(0, 1024)]
        public string Description;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "An admin who is allowed to create on behalf of another user should use this flag to specify the desired owner of the group.This user will be added as both a member and an owner of the group. If not specified, the user who creates the team will be added as both a member and an owner.")]
        public string Owner;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Boolean value that determines whether or not members (not only owners) are allowed to add apps to the team.")]
        public bool? AllowAddRemoveApps;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Boolean value that determines whether or not channels in the team can be @ mentioned so that all users who follow the channel are notified.")]
        public bool? AllowChannelMentions;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not just owners) are allowed to create channels.")]
        public bool? AllowCreateUpdateChannels;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not only owners) can manage connectors in the team.")]
        public bool? AllowCreateUpdateRemoveConnectors;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not only owners) can manage tabs in channels.") ]
        public bool? AllowCreateUpdateRemoveTabs;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members can use the custom memes functionality in teams.")]
        public bool? AllowCustomMemes;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not only owners) can delete channels in the team.")]
        public bool? AllowDeleteChannels;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not giphy can be used in the team.")]
        public bool? AllowGiphy;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not guests can create channels in the team.")]
        public bool? AllowGuestCreateUpdateChannels;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not guests can delete in the team.")]
        public bool? AllowGuestDeleteChannels;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not owners can delete messages that they or other members of the team have posted.")]
        public bool? AllowOwnerDeleteMessages;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether stickers and memes usage is allowed in the team.")]
        public bool? AllowStickersAndMemes;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether the entire team can be @ mentioned (which means that all users will be notified)")]
        public bool? AllowTeamMentions;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members can delete messages that they have posted.")]
        public bool? AllowUserDeleteMessages;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not users can edit messages that they have posted.")]
        public bool? AllowUserEditMessages;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines the level of sensitivity of giphy usage that is allowed in the team. Accepted values are \"Strict\" or \"Moderate\"")]
        public Model.Teams.TeamGiphyContentRating GiphyContentRating;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NEWGROUP, HelpMessage = "Set to Public to allow all users in your organization to join the group by default. Set to Private to require that an owner approve the join request.")]
        public TeamVisibility Visibility;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not private teams should be searchable from Teams clients for users who do not belong to that team. Set to $false to make those teams not discoverable from Teams clients.")]
        public bool? ShowInTeamsSearchAndSuggestions;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Classification;


        protected override void ExecuteCmdlet()
        {
            var teamCI = new TeamCreationInformation()
            {
                AllowAddRemoveApps = AllowAddRemoveApps,
                AllowChannelMentions = AllowChannelMentions,
                AllowCreateUpdateChannels = AllowCreateUpdateChannels,
                AllowCreateUpdateRemoveConnectors = AllowCreateUpdateRemoveConnectors,
                AllowCreateUpdateRemoveTabs = AllowCreateUpdateRemoveTabs,
                AllowCustomMemes = AllowCustomMemes,
                AllowDeleteChannels = AllowDeleteChannels,
                AllowGiphy = AllowGiphy,
                AllowGuestCreateUpdateChannels = AllowGuestCreateUpdateChannels,
                AllowGuestDeleteChannels = AllowGuestDeleteChannels,
                AllowOwnerDeleteMessages = AllowOwnerDeleteMessages,
                AllowStickersAndMemes = AllowStickersAndMemes,
                AllowTeamMentions = AllowTeamMentions,
                AllowUserDeleteMessages = AllowUserDeleteMessages,
                AllowUserEditMessages = AllowUserEditMessages,
                Classification = Classification,
                Description = Description,
                DisplayName = DisplayName,
                GiphyContentRating = GiphyContentRating,
                GroupId = GroupId,
                ShowInTeamsSearchAndSuggestions = ShowInTeamsSearchAndSuggestions,
                Visibility = (GroupVisibility)Enum.Parse(typeof(GroupVisibility), Visibility.ToString()),
            };
            WriteObject(TeamsUtility.NewTeamAsync(AccessToken, HttpClient, GroupId, DisplayName, Description, Classification, MailNickName, Owner, (GroupVisibility)Enum.Parse(typeof(GroupVisibility), Visibility.ToString()), teamCI).GetAwaiter().GetResult());
        }
    }
}
#endif