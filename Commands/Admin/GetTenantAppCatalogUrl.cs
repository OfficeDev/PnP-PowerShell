#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantAppCatalogUrl", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieves the url of the tenant scoped app catalog",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantAppCatalogUrl", 
        Remarks = "Returns the url of the tenant scoped app catalog site collection", SortOrder = 1)]
    public class GetTenantAppCatalogUrl : PnPSharePointCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var settings = TenantSettings.GetCurrent(ClientContext);
            settings.EnsureProperties(s => s.CorporateCatalogUrl);
            WriteObject(settings.CorporateCatalogUrl);
        }
    }
}
#endif