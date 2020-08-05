#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsUser")]
    [CmdletHelp("Returns owners, members or guests from a team.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsUser -Team MyTeam",
       Remarks = "Returns all owners, members or guests from the specified team.",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsUser -Team MyTeam -Role Owner",
       Remarks = "Returns all owners from the specified team.",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsUser -Team MyTeam -Role Member",
       Remarks = "Returns all members from the specified team.",
       SortOrder = 3)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsUser -Team MyTeam -Role Guest",
       Remarks = "Returns all guestss from the specified team.",
       SortOrder = 4)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_Read_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class GetTeamsUser : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = false, HelpMessage = "Optional specify the channel id, or display name of the channel to list the users for")]
        public TeamsChannelPipeBind Channel;

        [Parameter(Mandatory = false, HelpMessage = "Specify to filter on the role of the user")]
        [ValidateSet(new[] { "Owner", "Member", "Guest" })]
        public string Role;
        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                try
                {
                    if (ParameterSpecified(nameof(Channel)))
                    {
                        var channelId = Channel.GetId(HttpClient, AccessToken, groupId);
                        if (!string.IsNullOrEmpty(channelId))
                        {
                            WriteObject(TeamsUtility.GetUsersAsync(HttpClient, AccessToken, groupId, channelId, Role).GetAwaiter().GetResult(), true);
                        }
                    }
                    else
                    {
                        WriteObject(TeamsUtility.GetUsersAsync(HttpClient, AccessToken, groupId, Role).GetAwaiter().GetResult(), true);
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