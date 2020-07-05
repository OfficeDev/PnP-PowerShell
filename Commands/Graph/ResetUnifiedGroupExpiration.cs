#if !ONPREMISES && !NETSTANDARD2_1
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Reset, "PnPUnifiedGroupExpiration")]
    [CmdletHelp("Renews the Office 365 Group by extending its expiration with the number of days defined in the group expiration policy set on the Azure Active Directory",
        DetailedDescription = "Renews the Office 365 Group by extending its expiration with the number of days defined in the group expiration policy set on the Azure Active Directory",
        Category = CmdletHelpCategory.Graph,        
        OutputTypeLink = "https://docs.microsoft.com/graph/api/group-renew",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Reset-PnPUnifiedGroupExpiration",
       Remarks = "Renews the Office 365 Group by extending its expiration with the number of days defined in the group expiration policy set on the Azure Active Directory",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All | MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class ResetUnifiedGroupExpiration : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Office 365 Group")]
        public UnifiedGroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(AccessToken);
            UnifiedGroupsUtility.RenewUnifiedGroup(group.GroupId, AccessToken);
        }
    }
}
#endif