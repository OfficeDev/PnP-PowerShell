#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.SiteDesigns
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteDesignTask", SupportsShouldProcess = false)]
    [CmdletHelp(@"This cmdlet retrieves a scheduled site design script.",
       Category = CmdletHelpCategory.TenantAdmin,
        Description = @"Used to retrieve a scheduled site design script. It takes the ID of the scheduled site design and the URL for the site where the site design is scheduled to be applied. ",
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteDesignTask -Identity 501z8c32-4147-44d4-8607-26c2f67cae82",
       Remarks = "This example retrieves a site design task given the provided site design task id",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteDesignTask",
       Remarks = "This example retrieves all site design tasks currently scheduled on the current site.",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteDesignTask -WebUrl ""https://contoso.sharepoint.com/sites/project""",
       Remarks = "This example retrieves all site design tasks currently scheduled on the provided site.",
       SortOrder = 3)]
    public class GetSiteDesignTask : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The ID of the site design to apply.")]
        public TenantSiteDesignTaskPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the site collection where the site design will be applied. If not specified the design will be applied to the site you connected to with Connect-PnPOnline.")]
        public string WebUrl;

        protected override void ExecuteCmdlet()
        {
            var url = SelectedWeb.EnsureProperty(w => w.Url);
            var tenantUrl = UrlUtilities.GetTenantAdministrationUrl(ClientContext.Url);
            using (var tenantContext = ClientContext.Clone(tenantUrl))
            {
                if (Identity != null)
                {
                    var task = Tenant.GetSiteDesignTask(tenantContext, Identity.Id);
                    ClientContext.Load(task);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(task);
                }
                else
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
                    var tasks = tenant.GetSiteDesignTasks(webUrl);
                    WriteObject(tasks, true);
                }
            }
        }
    }
}
#endif