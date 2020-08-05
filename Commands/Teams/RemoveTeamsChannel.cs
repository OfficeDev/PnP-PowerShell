#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Teams
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
        public TeamsChannelPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue("Removing the channel will also remove all the messages in the channel.", Properties.Resources.Confirm))
            {
                var groupId = Team.GetGroupId(HttpClient, AccessToken);
                if (groupId != null)
                {
                    var channel = Identity.GetChannel(HttpClient, AccessToken, groupId);
                    if (channel != null)
                    {
                        var response = TeamsUtility.DeleteChannelAsync(AccessToken, HttpClient, groupId, channel.Id).GetAwaiter().GetResult();
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
                                WriteError(new ErrorRecord(new Exception($"Channel remove failed"), "REMOVEFAILED", ErrorCategory.InvalidResult, this));
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
    }
}
#endif