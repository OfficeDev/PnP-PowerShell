#if !ONPREMISES
using System.Management.Automation;
using System.Threading;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsData.Restore, "PnPTenantRecycleBinItem")]
    [CmdletHelp("Restores a site collection from the tenant scoped recycle bin", 
        DetailedDescription = @"The Reset-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be restored to its original location.", 
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso",
        Remarks = @"This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait",
        Remarks = @"This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location and will wait with executing further PowerShell commands until the site collection restore has completed", SortOrder = 2)]
    public class RestoreTenantRecycleBinItem : SPOAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Url of the site collection to restore from the tenant recycle bin", ValueFromPipeline = false)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "If provided, the PowerShell execution will halt until the site restore process has completed", ValueFromPipeline = false)]
        public SwitchParameter Wait = false;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to restore the site collection from the tenant recycle bin")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(string.Format(Resources.ResetTenantRecycleBinItem, Url), Resources.Confirm))
            {
                var spOperation = Tenant.RestoreDeletedSite(Url);
                Tenant.Context.Load(spOperation, spo => spo.PollingInterval, spo => spo.IsComplete);
                Tenant.Context.ExecuteQueryRetry();

                if (Wait)
                {
                    while (!spOperation.IsComplete)
                    {
                        Thread.Sleep(spOperation.PollingInterval);
                        Tenant.Context.Load(spOperation, spo => spo.PollingInterval, spo => spo.IsComplete);
                        Tenant.Context.ExecuteQueryRetry();
                        Host.UI.Write(".");
                        if (Stopping)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
#endif