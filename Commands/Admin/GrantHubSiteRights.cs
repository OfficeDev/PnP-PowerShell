#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsSecurity.Grant, "PnPHubSiteRights")]
    [CmdletHelp(@"Retrieve all or a specific hubsite.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity", Remarks = "Returns all site storage entities/farm properties", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Key MyKey", Remarks = "Returns the storage entity/farm property with the given key.", SortOrder = 2)]
    public class GrantHubSiteRights : PnPAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
        [Alias("HubSite")]
        public HubSitePipeBind Identity { get; set; }

        [Parameter(Mandatory = true)]
        public string[] Principals { get; set; }

        [Parameter(Mandatory = true)]
        public SPOHubSiteUserRights Rights { get; set; }

        protected override void ExecuteCmdlet()
        {
            base.Tenant.GrantHubSiteRights(Identity.Url, Principals, Rights);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif