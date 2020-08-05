#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Clear, "PnPMicrosoft365GroupOwner")]
    [Alias("Clear-PnPUnifiedGroupOwner")]
    [CmdletHelp("Removes all current owners from a particular Microsoft 365 Group (aka Unified Group)",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Clear-PnPMicrosoft365GroupOwner -Identity ""Project Team""",
       Remarks = @"Removes all the current owners from the Microsoft 365 Group named ""Project Team""",
       SortOrder = 1)]
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/group-delete-owners")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All | MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    public class ClearMicrosoft365GroupOwner : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group to remove all owners from")]
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
                UnifiedGroupsUtility.ClearUnifiedGroupOwners(group.GroupId, AccessToken);
            }
        }
    }
}
#endif