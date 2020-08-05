#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.SiteDesigns
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteDesignTask", SupportsShouldProcess = true)]
    [CmdletHelp(@"Similar to Invoke-PnPSiteDesign, this command is used to apply a published site design to a specified site collection target. It schedules the operation, allowing for the application of larger site scripts (Invoke-PnPSiteDesign is limited to 30 actions and subactions).",
       Category = CmdletHelpCategory.TenantAdmin,
        Description = @"This command is used to apply a published site design to a specified site collection target. It schedules the operation, allowing for the application of larger site scripts (Invoke-PnPSiteDesign is limited to 30 actions and subactions).
This command is intended to replace Invoke-PnPSiteDesign and is useful when you need to apply a large number of actions or multiple site scripts.",
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPSiteDesignTask -SiteDesignId 501z8c32-4147-44d4-8607-26c2f67cae82",
       Remarks = "This example applies a site design the currently connected to site. Executing the commands will schedule the site design to be queued and run against the designated site collection.",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPSiteDesignTask -SiteDesignId 501z8c32-4147-44d4-8607-26c2f67cae82 -WebUrl ""https://contoso.sharepoint.com/sites/project""",
       Remarks = "This example applies a site design to the designated site. Executing the commands will schedule the site design to be queued and run against the designated site collection.",
       SortOrder = 2)]
    public class AddSiteDesignTask : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The ID of the site design to apply.")]
        public GuidPipeBind SiteDesignId;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the site collection where the site design will be applied. If not specified the design will be applied to the site you connected to with Connect-PnPOnline.")]
        public string WebUrl;

        protected override void ExecuteCmdlet()
        {
            var url = SelectedWeb.EnsureProperty(w => w.Url);
            var tenantUrl = UrlUtilities.GetTenantAdministrationUrl(ClientContext.Url);
            using (var tenantContext = ClientContext.Clone(tenantUrl))
            {
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
                try
                {
                    var designTask = Tenant.AddSiteDesignTask(tenantContext, webUrl, SiteDesignId.Id);
                    tenantContext.Load(designTask);
                    tenantContext.ExecuteQueryRetry();
                    WriteObject(designTask);
                }
                catch (System.Net.WebException ex)
                {
                    if (ex.Status == System.Net.WebExceptionStatus.ProtocolError)
                    {
                        var webResponse = ex.Response as System.Net.HttpWebResponse;
                        if (null != webResponse && 
                            webResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            WriteObject("The server threw an exception (see below). It seems you may not have access to the server or you are executing this script outside of the tenant admin URL eg. yourtenant-admin.sharepoint.com. Please connect to the tenant admin URL first and try again.");
                        }
                    }
                    WriteObject( ex );
                }
            }
        }
    }
}
#endif