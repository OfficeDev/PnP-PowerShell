#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamsChannel")]
    [CmdletHelp("Updates an existing Teams Channel",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsChannel -Team \"MyTeam\" -Channel \"MyChannel\" -DisplayName \"My Channel\"",
       Remarks = "Updates the channel called 'MyChannel' to have the display name set to 'My Channel'",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SetTeamsChannel : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the channel id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsChannelPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Changes the display name of the specified channel.")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "Changes the description of the specified channel.")]
        public string Description;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var teamChannel = Identity.GetChannel(HttpClient, AccessToken, groupId);
                if (teamChannel != null)
                {
                    if (ParameterSpecified(nameof(DisplayName)) && teamChannel.DisplayName != DisplayName)
                    {
                        teamChannel.DisplayName = DisplayName;
                    } else
                    {
                        teamChannel.DisplayName = null;
                    }
                    if (ParameterSpecified(nameof(Description)) && teamChannel.Description != Description)
                    {
                        teamChannel.Description = Description;
                    } else
                    {
                        teamChannel.Description = null;
                    }
                    teamChannel.MembershipType = null;
                    try 
                    {
                        var updated = TeamsUtility.UpdateChannelAsync(HttpClient, AccessToken, groupId, teamChannel.Id, teamChannel).GetAwaiter().GetResult();
                        WriteObject(updated);
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
                    throw new PSArgumentException("Channel not found");
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