#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPTeamsChannel")]
    [CmdletHelp("Adds a channel to an existing Microsoft Teams instance.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsChannel -Team 4efdf392-8225-4763-9e7f-4edeb7f721aa -DisplayName \"My Channel\"",
       Remarks = "Adds a new channel to the specified Teams instance",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsChannel -Team MyTeam -DisplayName \"My Channel\"",
       Remarks = "Adds a new channel to the specified Teams instance",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddTeamsChannel : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true)]
        public string DisplayName;

        [Parameter(Mandatory = false)]
        public string Description;

        protected override void ExecuteCmdlet()
        {
            Model.Teams.TeamChannel channel = null;

            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                channel = TeamsUtility.AddChannel(AccessToken, HttpClient, groupId, DisplayName, Description);
                WriteObject(channel);
            }
            
        }
    }
}
#endif