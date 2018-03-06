#if !ONPREMISES
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPHubSite")]
    [CmdletHelp(@"Sets hubsite properties",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHubSite -Identity https://tenant.sharepoint.com/sites/myhubsite -Title ""My New Title""", 
        Remarks = "Sets the title of the hubsite", 
        SortOrder = 1)]
    public class SetHubSite : PnPAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
        [Alias("HubSite")]
        public HubSitePipeBind Identity { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public string LogoUrl { get; set; }

        [Parameter(Mandatory = false)]
        public string Description { get; set; }

        protected override void ExecuteCmdlet()
        {
            var hubSiteProperties = base.Tenant.GetHubSitePropertiesByUrl(this.Identity.Url);
            ClientContext.Load(hubSiteProperties);
            if (MyInvocation.BoundParameters.ContainsKey("Title"))
            {
                hubSiteProperties.Title = Title;
            }
            if (MyInvocation.BoundParameters.ContainsKey("LogoUrl"))
            {
                hubSiteProperties.LogoUrl = LogoUrl;
            }
            if (MyInvocation.BoundParameters.ContainsKey("Description"))
            {
                hubSiteProperties.Description = this.Description;
            }
            hubSiteProperties.Update();
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif