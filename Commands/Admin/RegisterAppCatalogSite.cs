#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Register, "PnPAppCatalogSite")]
    [CmdletHelp("Creates a new App Catalog Site and sets this site as the Tenant App Catalog",
        Category = CmdletHelpCategory.TenantAdmin, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Register-PnPAppCatalogSite -Url https://yourtenant.sharepoint.com/sites/appcatalog -Owner admin@domain.com -TimeZoneId 4",
        Remarks = @"This will create a new appcatalog site if no app catalog is already present. Use -Force to create a new appcatalog site if one has already been registered. If using the same URL as an existing one and Force is present, the current/existing appcatalog site will be deleted.", SortOrder = 1)]
    public class RegisterAppCatalogSite : PnPAdminCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "The full url of the app catalog site to be created, e.g. https://yourtenant.sharepoint.com/sites/appcatalog")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = "The login account of the user designated to be the admin for the site, e.g. user@domain.com")]
        public string Owner;

        [Parameter(Mandatory = true, HelpMessage = "Use Get-PnPTimeZoneId to retrieve possible timezone values")]
        public int TimeZoneId;

        [Parameter(Mandatory = false, HelpMessage ="If specified, and an app catalog is already present, a new app catalog site will be created. If the same URL is used the existing/current app catalog site will be deleted first.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            WriteWarning("Notice that this cmdlet can take considerate time to finish executing.");
            Tenant.EnsureAppCatalogAsync(Url, Owner, TimeZoneId, Force).GetAwaiter().GetResult();
        }
    }
}
#endif
