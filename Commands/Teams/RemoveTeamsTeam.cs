#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPTeamsTeam")]
    [CmdletHelp("Removes a Microsoft Teams Team instance",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPTeamsTeam -Identity 5beb63c5-0571-499e-94d5-3279fdd9b6b5",
       Remarks = "Removes the specified Team",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Remove-PnPTeamsTeam -Identity testteam",
       Remarks = "Removes the specified Team. If there are multiple teams with the same display name it will not proceed deleting the team.",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveTeamsTeam : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Either the group id or the mailnickname of the group to remove.")]
        public TeamsTeamPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var groupId = Identity.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                if (Force || ShouldContinue("Removing the team will remove all messages in all channels in the team.", Properties.Resources.Confirm))
                {
                    var response = TeamsUtility.DeleteTeamAsync(AccessToken, HttpClient, groupId).GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        if (GraphHelper.TryGetGraphException(response, out GraphException ex))
                        {
                            if (ex.Error != null)
                            {
                                throw new PSInvalidOperationException(ex.Error.Message);
                            }
                        }
                        else
                        {
                            WriteError(new ErrorRecord(new Exception($"Team remove failed"), "REMOVEFAILED", ErrorCategory.InvalidResult, this));
                        }
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