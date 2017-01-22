#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using System;
using OfficeDevPnP.Core;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPTenantSite", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [CmdletAlias("Remove-SPOTenantSite")]
    [CmdletHelp("Office365 only: Removes a site collection from the current tenant",
         Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
         Code = @"PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso",
         Remarks =
             @"This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso'  and put it in the recycle bin.",
         SortOrder = 1)]
    [CmdletExample(
         Code = @"PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -Force -SkipRecycleBin",
         Remarks =
             @"This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' with force and it will skip the recycle bin.",
         SortOrder = 2)]
    [CmdletExample(
         Code = @"PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -FromRecycleBin",
         Remarks =
             @"This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the recycle bin.",
         SortOrder = 3)]
    public class RemoveSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the full URL of the site collection that needs to be deleted")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "Do not add to the tenant scoped recycle bin when selected.")]
        [Alias("SkipTrash")]
        public SwitchParameter SkipRecycleBin;

        [Parameter(Mandatory = false, HelpMessage = "OBSOLETE: If true, will wait for the site to be deleted before processing continues")]
        [Obsolete("The cmdlet will always wait for the site to be deleted first")]
        public SwitchParameter Wait;

        [Parameter(Mandatory = false, HelpMessage = "If specified, will search for the site in the Recycle Bin and remove it from there.")]
        [Obsolete("Use Clear-PnPTenantRecycleBinItem instead.")]
        public SwitchParameter FromRecycleBin;
        
        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")] public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (!Url.ToLower().StartsWith("https://") && !Url.ToLower().StartsWith("http://"))
            {
                Uri uri = BaseUri;
                Url = $"{uri.ToString().TrimEnd('/')}/{Url.TrimStart('/')}";
            }

            if (Force || ShouldContinue(string.Format(Resources.RemoveSiteCollection0, Url), Resources.Confirm))
            {
                Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;

#pragma warning disable 618
                if (!FromRecycleBin)
#pragma warning restore 618
                {
                    
                    Tenant.DeleteSiteCollection(Url, !MyInvocation.BoundParameters.ContainsKey("SkipRecycleBin"), timeoutFunction);
                }
                else
                {
                    Tenant.DeleteSiteCollectionFromRecycleBin(Url, true, timeoutFunction);
                }

            }
        }

        private bool TimeoutFunction(TenantOperationMessage message)
        {
            switch (message)
            {
                case TenantOperationMessage.DeletingSiteCollection:
                case TenantOperationMessage.RemovingDeletedSiteCollectionFromRecycleBin:
                    Host.UI.Write(".");
                    break;
            }
            return Stopping;
        }
    }
}
#endif