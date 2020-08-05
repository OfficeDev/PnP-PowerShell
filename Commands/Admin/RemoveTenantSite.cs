using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using System;
using OfficeDevPnP.Core;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPTenantSite", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = false)]
    [CmdletHelp("Removes a site collection",
        "Removes a site collection which is listed in your tenant administration site.",
        SupportedPlatform = CmdletSupportedPlatform.All,
         Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
         Code = @"PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso",
         Remarks =
             @"This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso'  and put it in the recycle bin.",
         SortOrder = 1)]
#if !ONPREMISES
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
#endif
    public class RemoveSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the full URL of the site collection that needs to be deleted")]
        public string Url;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Do not add to the tenant scoped recycle bin when selected.")]
        [Alias("SkipTrash")]
        public SwitchParameter SkipRecycleBin;

        [Parameter(Mandatory = false, HelpMessage = "OBSOLETE: If true, will wait for the site to be deleted before processing continues")]
        [Obsolete("The cmdlet will always wait for the site to be deleted first")]
        public SwitchParameter Wait;

        [Parameter(Mandatory = false, HelpMessage = "If specified, will search for the site in the Recycle Bin and remove it from there.")]
        [Obsolete("Use Clear-PnPTenantRecycleBinItem instead.")]
        public SwitchParameter FromRecycleBin;
#endif

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (!Url.ToLower().StartsWith("https://") && !Url.ToLower().StartsWith("http://"))
            {
                Uri uri = BaseUri;
                Url = $"{uri.ToString().TrimEnd('/')}/{Url.TrimStart('/')}";
            }

            bool dodelete = true;
#if !ONPREMISES
            // Check if not deleting the root web
            var siteUri = new Uri(Url);
            if ($"{siteUri.Scheme}://{siteUri.Host}".Equals(Url, StringComparison.OrdinalIgnoreCase) && !Force)
            {
                dodelete = false;
                dodelete = ShouldContinue("You are trying to delete the root site collection. Be aware that you need to contact Office 365 Support in order to create a new root site collection. Also notice that some CSOM and REST operations require the root site collection to be present. Removing this site can affect all your remote processing code, even when accessing non-root site collections.", Resources.Confirm);
            }
#endif
            if (dodelete && (Force || ShouldContinue(string.Format(Resources.RemoveSiteCollection0, Url), Resources.Confirm)))
            {
#if !ONPREMISES

                Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;
#endif

#if ONPREMISES
                Tenant.DeleteSiteCollection(Url);
#else
#pragma warning disable 618
                if (!FromRecycleBin)
#pragma warning restore 618
                {
                    Tenant.DeleteSiteCollection(Url, !ParameterSpecified(nameof(SkipRecycleBin)), timeoutFunction);
                }
                else
                {
                    Tenant.DeleteSiteCollectionFromRecycleBin(Url, true, timeoutFunction);
                }
#endif
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