#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPKnowledgeHubSite")]
    [CmdletHelp("Sets the Knowledge Hub Site for your tenant",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Set-PnPKnowledgeHubSite -KnowledgeHubSiteUrl https://yoursite.sharepoint.com/sites/knowledge",
     Remarks = @"Sets the Knowledge Hub Site for your tenant", SortOrder = 1)]
    public class SetKnowledgeHubSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true)]
        public string KnowledgeHubSiteUrl;

        protected override void ExecuteCmdlet()
        {
            Tenant.SetKnowledgeHubSite(KnowledgeHubSiteUrl);
            Tenant.Context.ExecuteQueryRetry();
        }
    }
}
#endif