#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Clear, "PnPTenantRecycleBinItem")]
    [CmdletHelp("Permanently deletes a site collection from the tenant scoped recycle bin", 
        DetailedDescription = @"The Clear-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be permanently deleted from the recycle bin as well.", 
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso",
        Remarks = @"This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait",
        Remarks = @"This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin and will wait with executing further PowerShell commands until the operation has completed", SortOrder = 2)]
    public class ClearTenantRecycleBinItem : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Url of the site collection to permanently delete from the tenant recycle bin", ValueFromPipeline = false)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "If provided, the PowerShell execution will halt until the operation has completed", ValueFromPipeline = false)]
        public SwitchParameter Wait = false;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to permanently delete the site collection from the tenant recycle bin")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(string.Format(Resources.ClearTenantRecycleBinItem, Url), Resources.Confirm))
            {
                Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;

                Tenant.DeleteSiteCollectionFromRecycleBin(Url, Wait, Wait ? timeoutFunction : null);
            }
        }

        private bool TimeoutFunction(TenantOperationMessage message)
        {
            if (message == TenantOperationMessage.RemovingDeletedSiteCollectionFromRecycleBin)
            {
                this.Host.UI.Write(".");
            }
            return Stopping;
        }
    }
}
#endif