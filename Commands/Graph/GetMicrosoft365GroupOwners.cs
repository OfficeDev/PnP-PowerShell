#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPMicrosoft365GroupOwners")]
    [Alias("Get-PnPUnifiedGroupOwners")]
    [CmdletHelp("Gets owners of a particular Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/group-list-owners",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPMicrosoft365GroupOwners -Identity $groupId",
       Remarks = "Retrieves all the owners of a specific Microsoft 365 Group based on its ID",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPMicrosoft365GroupOwners -Identity $group",
       Remarks = "Retrieves all the owners of a specific Microsoft 365 Group based on the group's object instance",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_Read_All | MicrosoftGraphApiPermission.User_Read_All | MicrosoftGraphApiPermission.Group_ReadWrite_All | MicrosoftGraphApiPermission.User_ReadWrite_All)]
    public class GetMicrosoft365GroupOwners : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group.")]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            if (group != null)
            {
                // Get Owners of the group
                List<UnifiedGroupUser> owners = UnifiedGroupsUtility.GetUnifiedGroupOwners(group, AccessToken);
                WriteObject(owners);
            }
        }
    }
}
#endif