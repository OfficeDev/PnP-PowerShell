#if !ONPREMISES
using System.Management.Automation;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsData.Restore, "PnPDeletedMicrosoft365Group")]
    [Alias("Restore-PnPDeletedUnifiedGroup")]
    [CmdletHelp("Restores one deleted Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = "PS:> Restore-PnPDeletedMicrosoft365Group -Identity 38b32e13-e900-4d95-b860-fb52bc07ca7f",
        Remarks = "Restores a deleted Microsoft 365 Group based on its ID",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $group = Get-PnPDeletedMicrosoft365Group -Identity 38b32e13-e900-4d95-b860-fb52bc07ca7f
PS:> Restore-PnPDeletedMicrosoft365Group -Identity $group",
        Remarks = "Restores the provided deleted Microsoft 365 Group",
        SortOrder = 2)]
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/directory-deleteditems-restore")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RestoreDeletedMicrosoft365Group : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the deleted Microsoft 365 Group")]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity?.GetDeletedGroup(AccessToken);

            if (group != null)
            {
                UnifiedGroupsUtility.RestoreDeletedUnifiedGroup(group.GroupId, AccessToken);
            }
        }
    }
}
#endif