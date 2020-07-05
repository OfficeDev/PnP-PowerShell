#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Teams
{
    [Cmdlet(VerbsCommon.Remove, "PnPTeamsChannel")]
    [CmdletHelp("Removes a channel from a Microsoft Teams instance.",
       Category = CmdletHelpCategory.Teams,
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
      Code = "PS:> Remove-PnPTeamsChannel -Team 4efdf392-8225-4763-9e7f-4edeb7f721aa -DisplayName \"My Channel\"",
      Remarks = "Removes the channel specified from the team specified",
      SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveTeamsChannel : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true)]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue("Removing the channel will also remove all the messages in the channel.", Properties.Resources.Confirm))
            {
                var groupId = Team.GetGroupId(HttpClient, AccessToken);
                if (groupId != null)
                {
                    if (!TeamsUtility.DeleteChannel(AccessToken, HttpClient, groupId, DisplayName))
                    {
                        WriteError(new ErrorRecord(new Exception($"Channel remove failed"), "REMOVEFAILED", ErrorCategory.InvalidResult, this));
                    }
                } else
                {
                    throw new PSArgumentException("Team not found");
                }
            }
        }
    }
}
#endif