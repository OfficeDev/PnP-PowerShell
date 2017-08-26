#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Collections.Generic;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantSite")]
    [CmdletHelp(@"Uses the tenant API to set site information.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -Sharing Disabled",
      Remarks = @"This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -StorageWarningLevel 8000 -StorageMaximumLevel 10000",
      Remarks = @"This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website', set the storage warning level to 8GB and set the storage maximum level to 10GB.", SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners 'user@contoso.onmicrosoft.com'",
      Remarks = @"This will set user@contoso.onmicrosoft.com as a site collection owner at 'https://contoso.sharepoint.com/sites/sales'.", SortOrder = 3)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -NoScriptSite:$false",
      Remarks = @"This will enable script support for the site 'https://contoso.sharepoint.com/sites/sales' if disabled.", SortOrder = 4)]
    public class SetTenantSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specifies the URL of the site", Position = 0, ValueFromPipeline = true)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the title of the site")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Specifies what the sharing capablilites are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly")]
        public SharingCapabilities? Sharing = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.")]
        public long? StorageMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter")]
        public long? StorageWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.")]
        public double? UserCodeMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter")]
        public double? UserCodeWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the site administrator can upgrade the site collection")]
        public SwitchParameter? AllowSelfServiceUpgrade = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies owners to add as site collection adminstrators. Can be both users and groups.")]
        public List<string> Owners;

        [Parameter(Mandatory = false, HelpMessage = "Sets the lockstate of a site")]
        public SiteLockState LockState;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if a site allows custom script or not. See https://support.office.com/en-us/article/Turn-scripting-capabilities-on-or-off-1f2c515f-5d7e-448a-9fd7-835da935584f for more information.")]
        public SwitchParameter NoScriptSite;

        [Parameter(Mandatory = false, HelpMessage = "Wait for the operation to complete")]
        public SwitchParameter Wait;
        protected override void ExecuteCmdlet()
        {
            Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;

            Tenant.SetSiteProperties(Url, title: Title,
                sharingCapability: Sharing,
                storageMaximumLevel: StorageMaximumLevel,
                storageWarningLevel: StorageWarningLevel,
                allowSelfServiceUpgrade: AllowSelfServiceUpgrade,
                userCodeMaximumLevel: UserCodeMaximumLevel,
                userCodeWarningLevel: UserCodeWarningLevel,
                noScriptSite: NoScriptSite,
                wait: Wait, timeoutFunction: Wait ? timeoutFunction : null
                );

            if (Owners != null && Owners.Count > 0)
            {
                var admins = new List<UserEntity>();
                foreach (var owner in Owners)
                {
                    var userEntity = new UserEntity { LoginName = owner };
                    admins.Add(userEntity);
                }
                Tenant.AddAdministrators(admins, new Uri(Url));
            }
            if (MyInvocation.BoundParameters.ContainsKey("LockState"))
            {
                Tenant.SetSiteLockState(Url, LockState, Wait, Wait ? timeoutFunction : null);
            }
        }

        private bool TimeoutFunction(TenantOperationMessage message)
        {
            if (message == TenantOperationMessage.SettingSiteProperties || message == TenantOperationMessage.SettingSiteLockState)
            {
                Host.UI.Write(".");
            }
            return Stopping;
        }

    }
}
#endif