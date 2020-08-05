#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Remove, "PnPOrgNewsSite")]    
    [CmdletHelp("Removes a given site from the list of organizational news sites.",
        DetailedDescription = @"Removes a given site from the list of organizational news sites based on its URL in your Sharepoint Online Tenant.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPOrgNewsSite -OrgNewsSiteUrl https://tenant.sharepoint.com/sites/mysite",
        Remarks = @"This example removes the specified site from list of organization's news sites.", SortOrder = 1)]
    public class RemoveOrgNewsSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The site to be removed from list of organization's news sites")]
        public SitePipeBind OrgNewsSiteUrl;

        protected override void ExecuteCmdlet()
        {
            Tenant.RemoveOrgNewsSite(OrgNewsSiteUrl.Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif