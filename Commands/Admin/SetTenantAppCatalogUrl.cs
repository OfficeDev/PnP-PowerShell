#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantAppCatalogUrl", SupportsShouldProcess = true)]
    [CmdletHelp(@"Sets the url of the tenant scoped app catalog",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantAppCatalogUrl -Url https://yourtenant.sharepoint.com/sites/appcatalog",
        Remarks = @"Sets the tenant scoped app catalog to the provided site collection url", SortOrder = 1)]
    public class SetTenantAppCatalogUrl : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The url of the site to set as the tenant scoped app catalog")]
        public string Url;

        protected override void ExecuteCmdlet()
        {
            var settings = TenantSettings.GetCurrent(ClientContext);
            settings.SetCorporateCatalog(Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif