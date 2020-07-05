#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPTeamsTab")]
    [CmdletHelp("Removes a Microsoft Teams tab in a channel.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPTeamsTab -GroupId 5beb63c5-0571-499e-94d5-3279fdd9b6b5 -ChannelId 19:796d063b63e34497aeaf092c8fb9b44e@thread.skype -Identity Wiki",
       Remarks = "Removes the tab with the display name 'Wiki' from the channel",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Remove-PnPTeamsTab -GroupId 5beb63c5-0571-499e-94d5-3279fdd9b6b5 -ChannelId 19:796d063b63e34497aeaf092c8fb9b44e@thread.skype -Identity fcef815d-2e8e-47a5-b06b-9bebba5c7852",
       Remarks = "Removes a tab with the specified id from the channel",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveTeamsTab : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the channel id or display name of the channel to use.")]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = true, HelpMessage = "Specify the id of the tab ")]
        public TeamsTabPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue("Removing the tab will remove the settings of this tab too.", Properties.Resources.Confirm))
            {
                var groupId = Team.GetGroupId(HttpClient, AccessToken);
                if (groupId != null)
                {
                    var channelId = Channel.GetId(HttpClient, AccessToken, groupId);
                    if (channelId != null)
                    {
                        var tabId = string.Empty;
                        if (string.IsNullOrEmpty(Identity.Id))
                        {
                            tabId = Identity.Id.ToString();
                        }
                        else
                        {
                            var tab = Identity.GetTab(HttpClient, AccessToken, groupId, channelId);
                            if (tab != null)
                            {
                                tabId = tab.Id;
                            }
                            else
                            {
                                throw new PSArgumentException("Cannot find tab");
                            }
                        }
                        if (!TeamsUtility.DeleteTab(AccessToken, HttpClient, groupId, channelId, tabId))
                        {
                            throw new PSInvalidOperationException("Tab remove failed");
                        }
                    }
                    else
                    {
                        throw new PSArgumentException("Cannot find channel");
                    }
                }
                else
                {
                    throw new PSArgumentException("Team not found", nameof(Team));
                }
            }
        }
    }
}
#endif