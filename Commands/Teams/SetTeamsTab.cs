#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamsTab")]
    [CmdletHelp("Updates Teams Tab settings",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsTab -Team \"MyTeam\" -Channel \"My Channel\" -Identity \"Wiki\" -DisplayName \"Channel Wiki\"",
       Remarks = "Updates the tab named 'Wiki' and changes the display name of the tab to 'Channel Wiki'",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SetTeamsTab : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the channel id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = false, HelpMessage = "Identity of the tab.")]
        public TeamsTabPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The new name of the tab.")]
        public string DisplayName;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var channelId = Channel.GetId(HttpClient, AccessToken, groupId);
                if (channelId != null)
                {
                    var tab = Identity.GetTab(HttpClient, AccessToken, groupId, channelId);
                    if (tab != null)
                    {
                        if (ParameterSpecified(nameof(DisplayName)) && tab.DisplayName != DisplayName)
                        {
                            tab.DisplayName = DisplayName;
                        }
                        TeamsUtility.UpdateTabAsync(HttpClient, AccessToken, groupId, channelId, tab).GetAwaiter().GetResult();
                    }
                    else
                    {
                        throw new PSArgumentException("Tab not found");
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