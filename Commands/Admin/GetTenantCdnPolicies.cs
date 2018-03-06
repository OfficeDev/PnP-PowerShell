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
    [Cmdlet(VerbsCommon.Get, "PnPTenantCdnPolicies")]
    [CmdletHelp("Returns the CDN Policies for the specified CDN (Public | Private).",
        DetailedDescription = @"Enables or disabled the public or private Office 365 Content Delivery Network (CDN).",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantCdnPolicies -CdnType Public",
        Remarks = @"Returns the policies for the specified CDN type", SortOrder = 1)]
    public class GetTenantCdnPolicies : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The type of cdn to retrieve the policies from")]
        public SPOTenantCdnType CdnType;

        protected override void ExecuteCmdlet()
        {
            var result = Tenant.GetTenantCdnPolicies(CdnType);
            ClientContext.ExecuteQueryRetry();

            WriteObject(Parse(result),true);
        }

        private Dictionary<Microsoft.Online.SharePoint.TenantAdministration.SPOTenantCdnPolicyType, string> Parse(IList<string> entries)
        {
            var returnDict = new Dictionary<SPOTenantCdnPolicyType, string>();
            foreach(var entry in entries)
            {
                var entryArray = entry.Split(new[] { ';' });
                returnDict.Add((SPOTenantCdnPolicyType)Enum.Parse(typeof(SPOTenantCdnPolicyType), entryArray[0]), entryArray[1]);
            }
            return returnDict;
        }
    }
}
#endif