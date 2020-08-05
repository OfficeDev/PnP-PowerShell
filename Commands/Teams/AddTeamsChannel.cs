#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
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
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsChannel -Team MyTeam -DisplayName \"My Channel\" -Private -OwnerUPN user@domain.com",
       Remarks = "Adds a new private channel to the specified Teams instance",
       SortOrder = 3)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddTeamsChannel : PnPGraphCmdlet
    {
        private const string ParameterSET_PRIVATE = "Private channel";
        private const string ParameterSET_PUBLIC = "Public channel";

        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ParameterSetName = ParameterSET_PUBLIC)]
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ParameterSetName = ParameterSET_PRIVATE)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "The display name of the new channel. Letters, numbers and spaces are allowed.", ParameterSetName = ParameterSET_PUBLIC)]
        [Parameter(Mandatory = true, HelpMessage = "The display name of the new channel. Letters, numbers and spaces are allowed.", ParameterSetName = ParameterSET_PRIVATE)]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "An optional description of the channel.", ParameterSetName = ParameterSET_PUBLIC)]
        [Parameter(Mandatory = false, HelpMessage = "An optional description of the channel.", ParameterSetName = ParameterSET_PRIVATE)]
        public string Description;

        [Parameter(Mandatory = true, HelpMessage = "Specify to mark the channel as private.", ParameterSetName = ParameterSET_PRIVATE)]
        public SwitchParameter Private;

        [Parameter(Mandatory = true, HelpMessage = "The UPN/email of the owner", ParameterSetName = ParameterSET_PRIVATE)]
        public string OwnerUPN;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                try
                {
                    var channel = TeamsUtility.AddChannelAsync(AccessToken, HttpClient, groupId, DisplayName, Description, Private, OwnerUPN).GetAwaiter().GetResult();
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