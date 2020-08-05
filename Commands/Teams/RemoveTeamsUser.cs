#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPTeamsUser")]
    [CmdletHelp("Removes users from a team.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPTeamsUser -Team MyTeam -User john@doe.com",
       Remarks = "Removes the user specified from both owners and members of the team.",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsUser -Team MyTeam -User john@doe.com -Owner",
       Remarks = "Removes the user john@doe.com from the owners of the team, but retains the user as a member.",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveTeamsUser : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the UPN (e.g. john@doe.com)")]
        public string User;

        [Parameter(Mandatory = false, HelpMessage = @"Specify the role of the user you are removing from the team. Accepts ""Owner"" and ""Member"" as possible values.
        If specified as ""Member"" then the specified user is removed from the Team completely even if they were the owner of the Team. If ""Owner"" is specified in the -Role parameter then the
        specified user is removed as an owner of the team but stays as a team member. Defaults to ""Member"". Note: The last owner cannot be removed from the team.")]
        [ValidateSet(new[] { "Owner", "Member" })]
        public string Role = "Member";

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                try
                {
                    if (Force || ShouldContinue($"Remove user with UPN {User}?", Properties.Resources.Confirm))
                    {
                        TeamsUtility.DeleteUserAsync(HttpClient, AccessToken, groupId, User, Role).GetAwaiter().GetResult();
                    }
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