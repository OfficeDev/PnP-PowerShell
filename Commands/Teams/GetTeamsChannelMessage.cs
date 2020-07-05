#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Model.Teams;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsChannelMessage")]
    [CmdletHelp("Sends a message to a Microsoft Teams Channel.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Submit-PnPTeamsChannelMessage -Team MyTestTeam -Channel \"My Channel\" -Message \"A new message\"",
       Remarks = "Sends \"A new message\" to the specified channel",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class GetTeamsChannelMessage : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = false, HelpMessage = "Specify to include deleted messages")]
        public SwitchParameter IncludeDeleted;
        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var channel = Channel.GetChannel(HttpClient, AccessToken, groupId);
                if (channel != null)
                {
                    WriteObject(TeamsUtility.GetMessages(HttpClient, AccessToken, groupId, channel.Id, IncludeDeleted), true);
                }
            }

        }
    }
}
#endif