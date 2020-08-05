#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPTeamsUser")]
    [CmdletHelp("Adds a channel to an existing Microsoft Teams instance.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsUser -Team MyTeam -User john@doe.com -Role Owner",
       Remarks = "Adds a user as an owner to the team",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsUser -Team MyTeam -User john@doe.com -Role Member",
       Remarks = "Adds a user as a member to the team",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddTeamsUser : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the UPN (e.g. john@doe.com)")]
        public string User;

        [Parameter(Mandatory = true, HelpMessage = "Specify the role of the user")]
        [ValidateSet(new[] { "Owner", "Member" })]
        public string Role;
        protected override void ExecuteCmdlet()
        {
            Model.Teams.TeamChannel channel = null;

            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                try
                {
                    TeamsUtility.AddUserAsync(HttpClient, AccessToken, groupId, User, Role).GetAwaiter().GetResult();
                    WriteObject(channel);
                }
                catch (GraphException ex)
                {
                    if (ex.Error != null)
                    {
                        throw new PSInvalidOperationException(ex.Error.Message);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                throw new PSArgumentException("Group not found");
            }

        }
    }
}
#endif