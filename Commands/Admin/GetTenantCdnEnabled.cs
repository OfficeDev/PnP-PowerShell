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

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantCdnEnabled")]
    [CmdletHelp("Retrieves if the Office 365 Content Delivery Network has been enabled.",
        DetailedDescription = @"Enables or disabled the public or private Office 365 Content Delivery Network (CDN).",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantCdnEnabled -CdnType Public",
        Remarks = @"Returns the enabled status of the specified CDN", SortOrder = 1)]
    public class GetTenantCdnEnabled : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The type of cdn to retrieve the origins from")]
        public SPOTenantCdnType CdnType;

        protected override void ExecuteCmdlet()
        {
            var result = Tenant.GetTenantCdnEnabled(CdnType);
            ClientContext.ExecuteQueryRetry();
            WriteObject(result);
        }
    }
}
#endif