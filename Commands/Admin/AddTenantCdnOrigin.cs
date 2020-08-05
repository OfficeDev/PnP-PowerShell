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
    [Cmdlet(VerbsCommon.Add, "PnPTenantCdnOrigin")]
    [CmdletHelp("Adds a new origin to the public or private content delivery network (CDN).",
        DetailedDescription = @"Add a new origin to the public or private CDN, on either Tenant level or on a single Site level. Effectively, a tenant admin points out to a document library, or a folder in the document library and requests that content in that library should be retrievable by using a CDN.

You must be a SharePoint Online global administrator and a site collection administrator to run the cmdlet.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Add-PnPTenantCdnOrigin -OriginUrl /sites/site/subfolder -CdnType Public",
        Remarks = @"This example configures a public CDN on site level.", SortOrder = 1)]
    public class AddTenantCdnOrigin : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Specifies a path to the doc library to be configured. It can be provided in two ways: relative path, or a mask.

Relative-Relative path depends on the OriginScope. If the originScope is Tenant, a path must be a relative path under the tenant root. If the originScope is Site, a path must be a relative path under the given Site. The path must point to the valid Document Library or a folder with a document library.")]
        public string OriginUrl;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the CDN type. The valid values are: public or private.")]
        public SPOTenantCdnType CdnType;

        protected override void ExecuteCmdlet()
        {
            Tenant.AddTenantCdnOrigin(CdnType, OriginUrl);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif