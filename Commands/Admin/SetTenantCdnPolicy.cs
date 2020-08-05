#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using PnP.PowerShell.Commands.Enums;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantCdnPolicy")]
    [CmdletHelp("Sets the CDN Policies for the specified CDN (Public | Private).",
        DetailedDescription = @"Sets the CDN Policies for the specified CDN (Public | Private).",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantCdnPolicy -CdnType Public -PolicyType IncludeFileExtensions -PolicyValue ""CSS,EOT,GIF,ICO,JPEG,JPG,JS,MAP,PNG,SVG,TTF,WOFF""",
        Remarks = @"This example sets the IncludeFileExtensions policy to the specified value.", SortOrder = 1)]
    public class SetTenantCdnPolicy : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The type of cdn to retrieve the policies from")]
        public SPOTenantCdnType CdnType;

        [Parameter(Mandatory = true, HelpMessage = "The type of the policy to set")]
        public SPOTenantCdnPolicyType PolicyType;

        [Parameter(Mandatory = true, HelpMessage = "The value of the policy to set")]
        public string PolicyValue;

        protected override void ExecuteCmdlet()
        {
            Tenant.SetTenantCdnPolicy(CdnType, PolicyType, PolicyValue);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif