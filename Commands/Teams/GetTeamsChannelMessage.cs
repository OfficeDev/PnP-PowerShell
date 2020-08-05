#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
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
                    WriteObject(TeamsUtility.GetMessagesAsync(HttpClient, AccessToken, groupId, channel.Id, IncludeDeleted).GetAwaiter().GetResult(), true);
                } else
                {
                    throw new PSArgumentException("Channel not found");
                }
            } else
            {
                throw new PSArgumentException("Team not found");
            }

        }
    }
}
#endif