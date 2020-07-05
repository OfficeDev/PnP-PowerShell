#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Teams
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsChannel")]
    [CmdletHelp("Gets the channels for a specified Team.",
       Category = CmdletHelpCategory.Teams,
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
      Code = "PS:> Get-PnPTeamsChannel -Team a6c1e0d7-f579-4993-81ab-4b666f8edea8",
      Remarks = "Retrieves all channels for the specified team",
      SortOrder = 1)]
    [CmdletExample(
      Code = "PS:> Get-PnPTeamsChannel -Team a6c1e0d7-f579-4993-81ab-4b666f8edea8 -Identity \"Test Channel\"",
      Remarks = "Retrieves the channel called 'Test Channel'",
      SortOrder = 2)]
    [CmdletExample(
      Code = "PS:> Get-PnPTeamsChannel -Team a6c1e0d7-f579-4993-81ab-4b666f8edea8 -Identity \"19:796d063b63e34497aeaf092c8fb9b44e@thread.skype\"",
      Remarks = "Retrieves the channel specified by its channel id",
      SortOrder = 3)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_Read_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class GetTeamsChannel : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.")]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = false, HelpMessage = "The identity of the channel to retrieve.")]
        public TeamsChannelPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                if (ParameterSpecified(nameof(Identity)))
                {
                    WriteObject(Identity.GetChannel(HttpClient, AccessToken, groupId));
                }
                else
                {
                    WriteObject(TeamsUtility.GetChannels(AccessToken, HttpClient, groupId));
                }
            } else
            {
                throw new PSArgumentException("Cannot find team", nameof(Team));
            }
        }
    }
}
#endif