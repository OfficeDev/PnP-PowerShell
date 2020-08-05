#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Remove, "PnPTenantCdnOrigin")]
    [CmdletHelp("Removes an origin from the Public or Private content delivery network (CDN).",
        DetailedDescription = @"Removes an origin from the Public or Private content delivery network (CDN).

You must be a SharePoint Online global administrator to run the cmdlet.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPTenantCdnOrigin -OriginUrl /sites/site/subfolder -CdnType Public",
        Remarks = @"This example removes the specified origin from the public CDN", SortOrder = 1)]
    public class RemoveTenantCdnOrigin : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The origin to remove.")]
        public string OriginUrl;

        [Parameter(Mandatory = true, HelpMessage = "The cdn type to remove the origin from.")]
        public SPOTenantCdnType CdnType = SPOTenantCdnType.Public;

        protected override void ExecuteCmdlet()
        {
            Tenant.RemoveTenantCdnOrigin(CdnType, OriginUrl);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif
