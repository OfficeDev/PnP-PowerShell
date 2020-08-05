#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPOrgNewsSite")]
    [CmdletHelp("Returns the list of all the configured organizational news sites.",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Get-PnPOrgNewsSite",
     Remarks = @"Returns the list of all the configured organizational news sites.", SortOrder = 1)]
    public class GetOrgNewsSite : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var results = Tenant.GetOrgNewsSites();
            ClientContext.ExecuteQueryRetry();
            WriteObject(results, true);
        }
    }
}
#endif