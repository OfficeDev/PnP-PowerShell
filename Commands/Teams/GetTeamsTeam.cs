#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsTeam")]
    [CmdletHelp("Gets one Microsoft Teams Team or a list of Teams.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTeam",
       Remarks = "Retrieves all the Microsoft Teams instances",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsTeam -Identity $groupId",
       Remarks = "Retrieves a specific Microsoft Teams instance",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_Read_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class GetTeamsTeam : PnPGraphCmdlet
    {
        private const string ParameterSet_GroupId = "Retrieve a specific Team";

        [Parameter(Mandatory = false, HelpMessage = "Specify the group id of the team to retrieve.")]
        public TeamsTeamPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                var groupId = Identity.GetGroupId(HttpClient, AccessToken);
                if (groupId != null)
                {
                    WriteObject(TeamsUtility.GetTeam(AccessToken, HttpClient, groupId));
                }
                else
                {
                    throw new PSArgumentException("Team not found");
                }
            }
            else
            {
                WriteObject(TeamsUtility.GetTeams(AccessToken, HttpClient), true);
            }
        }
    }
}
#endif