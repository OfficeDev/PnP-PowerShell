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
        Code = @"PS:> Get-PnPTenantTheme -Name ""MyTheme"" | Set-PnPWebTheme",
        Remarks = @"Sets the theme named ""MyTheme"" to the current web",
        SortOrder = 2)]
    public class SetWebTheme : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Color Palette Url based on the site or server relative url")]
        public ThemePipeBind Theme;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the web to apply the theme to. If not specified it will default to the current web based upon the URL specified with Connect-PnPOnline.")]
        public string WebUrl;

        protected override void ExecuteCmdlet()
        {
            var url = SelectedWeb.EnsureProperty(w => w.Url);
            var tenantUrl = UrlUtilities.GetTenantAdministrationUrl(ClientContext.Url);
            using (var tenantContext = ClientContext.Clone(tenantUrl))
            {
                var tenant = new Tenant(tenantContext);
                var webUrl = url;
                if (!string.IsNullOrEmpty(WebUrl))
                {
                    try
                    {
                        var uri = new Uri(WebUrl);
                        webUrl = WebUrl;
                    }
                    catch
                    {
                        ThrowTerminatingError(new ErrorRecord(new System.Exception("Invalid URL"), "INVALIDURL", ErrorCategory.InvalidArgument, WebUrl));
                    }
                }

                tenant.SetWebTheme(Theme.Name, webUrl);
                tenantContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif
