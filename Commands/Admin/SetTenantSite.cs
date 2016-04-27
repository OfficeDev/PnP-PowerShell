#if !CLIENTSDKV15
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOTenantSite")]
    [CmdletHelp(@"Office365 only: Uses the tenant API to set site information.", 
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
      Code = @"PS:> Set-SPOTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -Sharing Disabled",
      Remarks = @"This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.", SortOrder = 1)]        
    public class SetTenantSite : SPOAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specifies the URL of the site", Position=0, ValueFromPipeline=true)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the title of the site")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Specifies what the sharing capablilites are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly")]
        public SharingCapabilities? Sharing = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.")]
        public long? StorageMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter")]
        public long? StorageWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.")]
        public double? UserCodeMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter")]
        public double? UserCodeWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the site administrator can upgrade the site collection")]
        public SwitchParameter? AllowSelfServiceUpgrade = null;

        protected override void ExecuteCmdlet()
        {
            Tenant.SetSiteProperties(Url, title:Title, sharingCapability: Sharing, storageMaximumLevel: StorageMaximumLevel, allowSelfServiceUpgrade: AllowSelfServiceUpgrade, userCodeMaximumLevel: UserCodeMaximumLevel, userCodeWarningLevel: UserCodeWarningLevel);
        }
    }

}
#endif