#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model.Teams;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamsTeam")]
    [CmdletHelp("Updates an existing Team.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsChannel -Team \"MyTeam\" -DisplayName \"My Team\"",
       Remarks = "Updates the team called 'MyTeam' to have the display name set to 'My Team'",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SetTeamsTeam : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Changes the display name of the specified team.")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "Changes the description of the specified team.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "Changes the visibility of the specified team.")]
        public TeamVisibility Visibility;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Boolean value that determines whether or not members (not only owners) are allowed to add apps to the team.")]
        public bool? AllowAddRemoveApps;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Boolean value that determines whether or not channels in the team can be @ mentioned so that all users who follow the channel are notified.")]
        public bool? AllowChannelMentions;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not just owners) are allowed to create channels.")]
        public bool? AllowCreateUpdateChannels;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not only owners) can manage connectors in the team.")]
        public bool? AllowCreateUpdateRemoveConnectors;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not members (and not only owners) can manage tabs in channels.")]
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

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Setting that determines whether or not private teams should be searchable from Teams clients for users who do not belong to that team. Set to $false to make those teams not discoverable from Teams clients.")]
        public bool? ShowInTeamsSearchAndSuggestions;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Classification;
        protected override void ExecuteCmdlet()
        {
            var groupId = Identity.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                try
                {
                    var team = TeamsUtility.GetTeamAsync(AccessToken, HttpClient, groupId).GetAwaiter().GetResult();
                    var updateGroup = false;
                    var group = new Group();
                    if (team != null)
                    {
                        if (ParameterSpecified(nameof(DisplayName)) && team.DisplayName != DisplayName)
                        {
                            updateGroup = true;
                            group.DisplayName = DisplayName;
                        }
                        else
                        {
                            team.DisplayName = null;
                        }
                        if (ParameterSpecified(nameof(Description)) && team.Description != Description)
                        {
                            updateGroup = true;
                            group.Description = Description;
                        }
                        else
                        {
                            team.Description = null;
                        }
                        if((GroupVisibility)Enum.Parse(typeof(GroupVisibility), Visibility.ToString()) != team.Visibility)
                        {
                            group.Visibility = (GroupVisibility)Enum.Parse(typeof(GroupVisibility), Visibility.ToString());
                            updateGroup = true;
                        }
                        team.IsArchived = null; // cannot update this value;

                        if(updateGroup)
                        {
                            TeamsUtility.UpdateGroupAsync(HttpClient, AccessToken, groupId, group).GetAwaiter().GetResult();
                        }

                        var teamCI = new TeamCreationInformation();
                        teamCI.AllowAddRemoveApps = ParameterSpecified(nameof(AllowAddRemoveApps)) ? AllowAddRemoveApps : null;
                        teamCI.AllowChannelMentions = ParameterSpecified(nameof(AllowChannelMentions)) ? AllowChannelMentions : null;
                        teamCI.AllowCreateUpdateChannels = ParameterSpecified(nameof(AllowCreateUpdateChannels)) ? AllowCreateUpdateChannels : null;
                        teamCI.AllowCreateUpdateRemoveConnectors = ParameterSpecified(nameof(AllowCreateUpdateRemoveConnectors)) ? AllowCreateUpdateRemoveConnectors : null;
                        teamCI.AllowCreateUpdateRemoveTabs = ParameterSpecified(nameof(AllowCreateUpdateRemoveTabs)) ? AllowCreateUpdateRemoveTabs : null;
                        teamCI.AllowCustomMemes = ParameterSpecified(nameof(AllowCustomMemes)) ? AllowCustomMemes : null;
                        teamCI.AllowDeleteChannels = ParameterSpecified(nameof(AllowDeleteChannels)) ? AllowDeleteChannels : null;
                        teamCI.AllowGiphy = ParameterSpecified(nameof(AllowGiphy)) ? AllowGiphy : null;
                        teamCI.AllowGuestCreateUpdateChannels = ParameterSpecified(nameof(AllowGuestCreateUpdateChannels)) ? AllowGuestCreateUpdateChannels : null;
                        teamCI.AllowGuestDeleteChannels = ParameterSpecified(nameof(AllowGuestDeleteChannels)) ? AllowGuestDeleteChannels : null;
                        teamCI.AllowOwnerDeleteMessages = ParameterSpecified(nameof(AllowOwnerDeleteMessages)) ? AllowOwnerDeleteMessages : null;
                        teamCI.AllowStickersAndMemes = ParameterSpecified(nameof(AllowStickersAndMemes)) ? AllowStickersAndMemes : null;
                        teamCI.AllowTeamMentions = ParameterSpecified(nameof(AllowTeamMentions)) ? AllowTeamMentions : null;
                        teamCI.AllowUserDeleteMessages = ParameterSpecified(nameof(AllowUserDeleteMessages)) ? AllowUserDeleteMessages : null;
                        teamCI.AllowUserEditMessages = ParameterSpecified(nameof(AllowUserEditMessages)) ? AllowUserEditMessages : null;
                        teamCI.Classification = ParameterSpecified(nameof(Classification)) ? Classification : null;

                        var updated = TeamsUtility.UpdateTeamAsync(HttpClient, AccessToken, groupId, teamCI.ToTeam()).GetAwaiter().GetResult();
                        WriteObject(updated);
                    }
                }
                catch (GraphException ex)
                {
                    if (ex.Error != null)
                    {
                        throw new PSInvalidOperationException(ex.Error.Message);
                    }
                    else
                    {
                        throw ex;
                    }
                }

            }
            else
            {
                throw new PSArgumentException("Team not found");
            }

        }
    }
}
#endif