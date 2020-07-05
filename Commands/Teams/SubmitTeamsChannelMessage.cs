#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Model.Teams;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsLifecycle.Submit, "PnPTeamsChannelMessage")]
    [CmdletHelp("Sends a message to a Microsoft Teams Channel.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Submit-PnPTeamsChannelMessage -Team MyTestTeam -Channel \"My Channel\" -Message \"A new message\"",
       Remarks = "Sends \"A new message\" to the specified channel",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Submit-PnPTeamsChannelMessage -Team MyTestTeam -Channel \"My Channel\" -Message \"<strong>A bold new message</strong>\" -ContentType Html",
       Remarks = "Sends the message, formatted as html to the specified channel",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SubmitTeamsChannelMessage : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = true, HelpMessage = "The message to send to the channel.")]
        public string Message;

        [Parameter(Mandatory = false, HelpMessage = "Specify to set the content type of the message, either Text or Html.")]
        public TeamChannelMessageContentType ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Specify to make this an important message.")]
        public SwitchParameter Important;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var channel = Channel.GetChannel(HttpClient, AccessToken, groupId);
                if (channel != null)
                {
                    var channelMessage = new TeamChannelMessage();
                    channelMessage.Importance = Important ? "high" : "normal";
                    channelMessage.Body.Content = Message;
                    channelMessage.Body.ContentType = ContentType;

                    TeamsUtility.PostMessage(HttpClient, AccessToken, groupId, channel.Id, channelMessage);
                }
            }
            
        }
    }
}
#endif