#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPHubSite")]
    [CmdletHelp(@"Retrieve all or a specific hubsite.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPHubSite", Remarks = "Returns all hubsite properties", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPHubSite -Identity https://contoso.sharepoint.com/sites/myhubsite", Remarks = "Returns the properties of the specified hubsite", SortOrder = 2)]
    public class GetHubSite : PnPAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public HubSitePipeBind Identity { get; set; }

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                var hubSiteProperties = Identity.GetHubSite(Tenant);
                ClientContext.Load(hubSiteProperties);
                ClientContext.ExecuteQueryRetry();
                WriteObject(hubSiteProperties);
            }
            else
            {
                var hubSitesProperties = base.Tenant.GetHubSitesProperties();
                ClientContext.Load(hubSitesProperties);
                ClientContext.ExecuteQueryRetry();
                WriteObject(hubSitesProperties, true);
            }
        }
    }
}
#endif