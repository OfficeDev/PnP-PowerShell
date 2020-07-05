#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsTab")]
    [CmdletHelp("Gets one or all tabs in a channel.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTab -Team 5beb63c5-0571-499e-94d5-3279fdd9b6b5 -Channel 19:796d063b63e34497aeaf092c8fb9b44e@thread.skype",
       Remarks = "Retrieves the tabs for the specified Microsoft Teams instance and channel",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTab -Team 5beb63c5-0571-499e-94d5-3279fdd9b6b5 -Channel 19:796d063b63e34497aeaf092c8fb9b44e@thread.skype -Identity \"Wiki\"",
       Remarks = "Retrieves a tab with the display name 'Wiki' from the specified team and channel",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTab -Team \"My Team\" -Channel \"My Channel\"",
       Remarks = "Retrieves the tabs for the specified Microsoft Teams instance and channel",
       SortOrder = 3)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTab \"My Team\" -Channel \"My Channel\" -Identity \"Wiki\"",
       Remarks = "Retrieves a tab with the display name 'Wiki' from the specified team and channel",
       SortOrder = 4)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_Read_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class GetTeamsTab : PnPGraphCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "Specify the group id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the channel id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = false, HelpMessage = "Identity")]
        public TeamsTabPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var channelId = Channel.GetId(HttpClient, AccessToken, groupId);
                if (!string.IsNullOrEmpty(channelId))
                {
                    if (ParameterSpecified(nameof(Identity)))
                    {

                        if (string.IsNullOrEmpty(Identity.Id))
                        {
                            WriteObject(Identity.GetTab(HttpClient, AccessToken, groupId, channelId));
                        }
                        else
                        {
                            WriteObject(Identity.GetTabById(HttpClient, AccessToken, groupId, channelId));
                        }
                    }
                    else
                    {
                        WriteObject(TeamsUtility.GetTabs(AccessToken, HttpClient, groupId, channelId));
                    }
                }
                else
                {
                    throw new PSArgumentException("Channel not found");
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