#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using SharePointPnP.PowerShell.Commands.Enums;
using System.Collections.Generic;
using SharePointPnP.PowerShell.Commands.Model;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPTenant")]
    [CmdletHelp(@"Returns organization-level site collection properties",
        DetailedDescription = @"Returns organization-level site collection properties such as StorageQuota, StorageQuotaAllocated, ResourceQuota,
ResourceQuotaAllocated, and SiteCreationMode.

Currently, there are no parameters for this cmdlet.

You must be a SharePoint Online global administrator to run the cmdlet.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenant",
        Remarks = @"This example returns all tenant settings", SortOrder = 1)]
    public class GetTenant : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            ClientContext.Load(Tenant);
            ClientContext.ExecuteQueryRetry();
            WriteObject(new SPOTenant(Tenant));
        }
    }
}
#endif