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
    [Cmdlet(VerbsCommon.Set, "PnPTenantCdnPolicy")]
    [CmdletHelp("Sets the CDN Policies for the specified CDN (Public | Private).",
        DetailedDescription = @"Sets the CDN Policies for the specified CDN (Public | Private).",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantCdnPolicies -CdnType Public -PolicyType IncludeFileExtensions -PolicyValue ""CSS,EOT,GIF,ICO,JPEG,JPG,JS,MAP,PNG,SVG,TTF,WOFF""",
        Remarks = @"This example sets the IncludeFileExtensions policy to the specified value.", SortOrder = 1)]
    public class SetTenantCdnPolicy : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The type of cdn to retrieve the policies from")]
        public SPOTenantCdnType CdnType;

        [Parameter(Mandatory = true, HelpMessage = "The type of the policy to set")]
        public SPOTenantCdnPolicyType PolicyType { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The value of the policy to set")]
        public string PolicyValue { get; set; }

        protected override void ExecuteCmdlet()
        {
            Tenant.SetTenantCdnPolicy(CdnType, PolicyType, PolicyValue);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif