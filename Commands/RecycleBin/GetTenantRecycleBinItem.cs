#if !ONPREMISES
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantRecycleBinItem", DefaultParameterSetName = "All")]
    [CmdletHelp("Returns the items in the tenant scoped recycle bin",
        DetailedDescription = "This command will return all the items in the tenant recycle bin for the Office 365 tenant you're connected to. Be sure to connect to the SharePoint Online Admin endpoint (https://tenant-admin.sharepoint.com) in order for this command to work.",
        Category = CmdletHelpCategory.RecycleBin,
        OutputType = typeof(DeletedSiteProperties),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.deletedsiteproperties.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantRecycleBinItem",
        Remarks = "Returns all site collections in the tenant scoped recycle bin",
        SortOrder = 1)]
    public class GetTenantRecycleBinItems : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var deletedSites = Tenant.GetDeletedSiteProperties(0);
            ClientContext.Load(deletedSites, c => c.IncludeWithDefaultProperties(s => s.Url, s => s.SiteId, s => s.DaysRemaining, s => s.Status));
            ClientContext.ExecuteQueryRetry();
            if (deletedSites.AreItemsAvailable)
            {
                WriteObject(deletedSites, true);
            }

        }
    }
}
#endif