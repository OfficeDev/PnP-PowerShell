#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Utilities.REST;
using PnP.PowerShell.Core.Attributes;
using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamsTeamArchivedState")]
    [CmdletHelp("Sets the archived state of a team.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsTeamArchivedState -Identity \"My Team\" -Archived $true",
       Remarks = "Archives the team as identified.",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsTeamArchivedState -Identity \"My Team\" -Archived $false",
       Remarks = "Unarchives the team as identified.",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsTeamArchivedState -Identity \"My Team\" -Archived $true -SetSiteReadOnlyForMembers $true",
       Remarks = "Archives the team as identified and sets the underlying SharePoint Online Site Collection as read only for members",
       SortOrder = 3)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    [CmdletTokenType(TokenType = TokenType.Delegate)]
    public class SetTeamsTeamArchivedState : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the name, id or external id of the app.")]
        public TeamsTeamPipeBind Identity;

        [Parameter(Mandatory = true)]
        public bool Archived;

        [Parameter(Mandatory = false)]
        public bool? SetSiteReadOnlyForMembers;

        protected override void ExecuteCmdlet()
        {
            if (!Archived && SetSiteReadOnlyForMembers.HasValue)
            {
                throw new PSArgumentException("You can only modify the read only state of a site when archiving a team");
            }
            var groupId = Identity.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var team = Identity.GetTeam(HttpClient, AccessToken);
                if (Archived == team.IsArchived)
                {
                    throw new PSInvalidOperationException($"Team {team.DisplayName} {(Archived ? "has already been" : "is not")} archived");
                }
                var response = TeamsUtility.SetTeamArchivedStateAsync(HttpClient, AccessToken, groupId, Archived, SetSiteReadOnlyForMembers).GetAwaiter().GetResult();
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
                        throw new PSInvalidOperationException("Setting archived state failed.");
                    }
                }
                else
                {
                    WriteObject($"Team {(Archived ? "archived" : "unarchived")}.");

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