#if !ONPREMISES
using System.Management.Automation;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPDeletedMicrosoft365Group")]
    [Alias("Remove-PnPDeletedUnifiedGroup")]
    [CmdletHelp("Permanently removes one deleted Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = "PS:> Remove-PnPDeletedMicrosoft365Group -Identity 38b32e13-e900-4d95-b860-fb52bc07ca7f",
        Remarks = "Permanently removes a deleted Microsoft 365 Group based on its ID",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $group = Get-PnPDeletedMicrosoft365Group -Identity 38b32e13-e900-4d95-b860-fb52bc07ca7f
PS:> Remove-PnPDeletedMicrosoft365Group -Identity $group",
        Remarks = "Permanently removes the provided deleted Microsoft 365 Group",
        SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveDeletedMicrosoft365Group : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the deleted Microsoft 365 Group")]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity?.GetDeletedGroup(AccessToken);

            if (group != null)
            {
                UnifiedGroupsUtility.PermanentlyDeleteUnifiedGroup(group.GroupId, AccessToken);
            }
        }
    }
}
#endif