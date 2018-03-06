#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPWebTheme")]
    [CmdletHelp("Sets the theme of the current web.", DetailedDescription = "Sets the theme of the current web. * Requires Tenant Administration Rights *", Category = CmdletHelpCategory.Branding, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWebTheme -Theme MyTheme", 
        Remarks = @"Sets the theme named ""MyTheme"" to the current web", 
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantTheme -Name ""MyTheme"" | Set-PnPTheme",
        Remarks = @"Sets the theme named ""MyTheme"" to the current web",
        SortOrder = 2)]
    public class SetWebTheme : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Color Palette Url based on the site or server relative url")]
        public ThemePipeBind Theme;

        protected override void ExecuteCmdlet()
        {
            var url = SelectedWeb.EnsureProperty(w => w.Url);
            var tenantUrl = UrlUtilities.GetTenantAdministrationUrl(ClientContext.Url);
            using (var tenantContext = ClientContext.Clone(tenantUrl))
            {
                var tenant = new Tenant(tenantContext);

                tenant.SetWebTheme(Theme.Name, url);
                tenantContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif