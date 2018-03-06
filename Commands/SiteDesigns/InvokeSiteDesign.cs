#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsLifecycle.Invoke, "PnPSiteDesign", SupportsShouldProcess = true)] 
    [CmdletHelp(@"Apply a Site Design to an existing site. * Requires Tenant Administration Rights *",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Invoke-PnPSiteDesign -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Applies the specified site design to the current site.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Invoke-PnPSiteDesign -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd -WebUrl https://contoso.sharepoint.com/sites/mydemosite",
        Remarks = "Applies the specified site design to the specified site.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesign | ?{$_.Title -eq ""Demo""} | Invoke-PnPSiteDesign",
        Remarks = "Applies the specified site design to the specified site.",
        SortOrder = 2)]
    public class InvokeSiteDesign : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The Site Design Id or an actual Site Design object to apply")]
        public TenantSiteDesignPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the web to apply the site design to. If not specified it will default to the current web based upon the URL specified with Connect-PnPOnline.")]
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
                        var uri = new System.Uri(WebUrl);
                        webUrl = WebUrl;
                    }
                    catch
                    {
                        ThrowTerminatingError(new ErrorRecord(new System.Exception("Invalid URL"), "INVALIDURL", ErrorCategory.InvalidArgument, WebUrl));
                    }
                }
                TenantSiteDesign design = Identity.GetTenantSiteDesign(tenant);
                if (design != null)
                {
                    
                    var results = tenant.ApplySiteDesign(SelectedWeb.Url, design.Id);
                    tenantContext.Load(results);
                    tenantContext.ExecuteQueryRetry();
                    WriteObject(results, true);
                }
            }
        }
    }
}
#endif