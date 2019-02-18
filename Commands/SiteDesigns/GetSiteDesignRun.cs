#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.SiteDesigns
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteDesignRun", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieves a list of site designs applied to a specified site collection.",
       Category = CmdletHelpCategory.TenantAdmin,
        Description = @"Retrieves a list of site designs applied to a specified site collection. If the WebUrl parameter is not specified we show the list of designs applied to the current site. The returned output includes the ID of the scheduled job, the web and site IDs, and the site design ID, version, and title.",
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteDesignRun",
       Remarks = "This example returns a list of the site designs applied to the current site. Providing a specific site design ID will return the details for just that applied site design.",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteDesignRun -WebUrl https://mytenant.sharepoint.com/sites/project",
       Remarks = "This example returns a list of the site designs applied to the specified site. Providing a specific site design ID will return the details for just that applied site design.",
       SortOrder = 2)]
    public class GetSiteDesignRun : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The ID of the site design to apply.")]
        public GuidPipeBind SiteDesignId;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the site collection where the site design will be applied. If not specified the design will be applied to the site you connected to with Connect-PnPOnline.")]
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
                var designRun = tenant.GetSiteDesignRun(webUrl, SiteDesignId != null ? SiteDesignId.Id : Guid.Empty);
                tenantContext.Load(designRun);
                tenantContext.ExecuteQueryRetry();
                WriteObject(designRun, true);
            }
        }
    }
}
#endif