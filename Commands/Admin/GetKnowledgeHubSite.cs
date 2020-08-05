#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPKnowledgeHubSite")]
    [CmdletHelp("Gets the Knowledge Hub Site URL for your tenant",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Get-PnPKnowledgeHubSite",
     Remarks = @"Returns the Knowledge Hub Site Url for your tenant", SortOrder = 1)]
    public class GetKnowledgeHubSite : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var results = Tenant.GetKnowledgeHubSite();
            Tenant.Context.ExecuteQueryRetry();
            WriteObject(results.Value);
        }
    }
}
#endif