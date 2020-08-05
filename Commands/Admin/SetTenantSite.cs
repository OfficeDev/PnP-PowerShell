using System;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Collections.Generic;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;
using Microsoft.Online.SharePoint.TenantAdministration;
using System.Net;
using System.Threading;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantSite")]
    [CmdletHelp(@"Updates settings of a site collection",
        DetailedDescription = "Allows settings of a site collection to be updated",
        SupportedPlatform = CmdletSupportedPlatform.All,
        Category = CmdletHelpCategory.TenantAdmin)]
#if !ONPREMISES
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title ""Contoso Website"" -Sharing Disabled",
      Remarks = @"This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title ""Contoso Website"" -StorageWarningLevel 8000 -StorageMaximumLevel 10000",
      Remarks = @"This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website', set the storage warning level to 8GB and set the storage maximum level to 10GB.", SortOrder = 2)]
#endif
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners ""user@contoso.onmicrosoft.com""",
      Remarks = @"This will add user@contoso.onmicrosoft.com as an additional site collection owner at 'https://contoso.sharepoint.com/sites/sales'.", SortOrder = 3)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners @(""user1@contoso.onmicrosoft.com"", ""user2@contoso.onmicrosoft.com"")",
      Remarks = @"This will add user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as additional site collection owners at 'https://contoso.sharepoint.com/sites/sales'.", SortOrder = 4)]
    [CmdletExample(
      Code = @"PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -NoScriptSite:$false",
      Remarks = @"This will enable script support for the site 'https://contoso.sharepoint.com/sites/sales' if disabled.", SortOrder = 5)]
    public class SetTenantSite : PnPAdminCmdlet
    {
        private const string ParameterSet_LOCKSTATE = "Set Lock State";
        private const string ParameterSet_PROPERTIES = "Set Properties";

        [Parameter(Mandatory = true, HelpMessage = "Specifies the URL of the site", Position = 0, ValueFromPipeline = true)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the title of the site", ParameterSetName = ParameterSet_PROPERTIES)]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the language of this site collection. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: (Get-PnPWeb -Includes RegionalSettings.InstalledLanguages).RegionalSettings.InstalledLanguages.", ParameterSetName = ParameterSet_PROPERTIES)]
        public uint LocaleId;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the site administrator can upgrade the site collection", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter AllowSelfServiceUpgrade;

        [Parameter(Mandatory = false, HelpMessage = "Specifies owner(s) to add as site collection administrators. They will be added as additional site collection administrators. Existing administrators will stay. Can be both users and groups.", ParameterSetName = ParameterSet_PROPERTIES)]
        public List<string> Owners;

        [Parameter(Mandatory = false, HelpMessage = "Determines whether the Add And Customize Pages right is denied on the site collection. For more information about permission levels, see User permissions and permission levels in SharePoint.", ParameterSetName = ParameterSet_PROPERTIES)]
        [Alias("NoScriptSite")]
        public SwitchParameter DenyAddAndCustomizePages;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Specifies what the sharing capabilities are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly", ParameterSetName = ParameterSet_PROPERTIES)]
        [Alias("Sharing")]
        public SharingCapabilities SharingCapability;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.", ParameterSetName = ParameterSet_PROPERTIES)]
        public long StorageMaximumLevel;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter", ParameterSetName = ParameterSet_PROPERTIES)]
        public long StorageWarningLevel;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.", ParameterSetName = ParameterSet_PROPERTIES)]
        [Obsolete("Sandboxed solutions are obsolete")]
        public double UserCodeMaximumLevel;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter", ParameterSetName = ParameterSet_PROPERTIES)]
        [Obsolete("Sandboxed solutions are obsolete")]
        public double UserCodeWarningLevel;

        [Parameter(Mandatory = false, HelpMessage = "Sets the lockstate of a site", ParameterSetName = ParameterSet_LOCKSTATE)]
        public SiteLockState? LockState;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the default link permission for the site collection. None - Respect the organization default link permission. View - Sets the default link permission for the site to ""view"" permissions. Edit - Sets the default link permission for the site to ""edit"" permissions", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingPermissionType DefaultLinkPermission;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the default link type for the site collection. None - Respect the organization default sharing link type. AnonymousAccess - Sets the default sharing link for this site to an Anonymous Access or Anyone link. Internal - Sets the default sharing link for this site to the ""organization"" link or company shareable link. Direct - Sets the default sharing link for this site to the ""Specific people"" link", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingLinkType DefaultSharingLinkType;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies a list of email domains that is allowed for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, ""contoso.com fabrikam.com"".", ParameterSetName = ParameterSet_PROPERTIES)]
        public string SharingAllowedDomainList;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies a list of email domains that is blocked for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, ""contoso.com fabrikam.com"".", ParameterSetName = ParameterSet_PROPERTIES)]
        public string SharingBlockedDomainList;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if non web viewable files can be downloaded.", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter BlockDownloadOfNonViewableFiles;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the external sharing mode for domains.", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingDomainRestrictionModes SharingDomainRestrictionMode = SharingDomainRestrictionModes.None;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if comments on site pages are enabled", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter CommentsOnSitePagesDisabled;

        [Parameter(Mandatory = false, HelpMessage = @"-", ParameterSetName = ParameterSet_PROPERTIES)]
        public AppViewsPolicy DisableAppViews;

        [Parameter(Mandatory = false, HelpMessage = @"-", ParameterSetName = ParameterSet_PROPERTIES)]
        public CompanyWideSharingLinksPolicy DisableCompanyWideSharingLinks;

        [Parameter(Mandatory = false, HelpMessage = @"-", ParameterSetName = ParameterSet_PROPERTIES)]
        public FlowsPolicy DisableFlows;

        [Parameter(Mandatory = false, HelpMessage = "Wait for the operation to complete")]
        public SwitchParameter Wait;
#endif
        protected override void ExecuteCmdlet()
        {
#if ONPREMISES
            SetSiteProperties();
#else
            Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;

            if (LockState.HasValue)
            {
                Tenant.SetSiteLockState(Url, LockState.Value, Wait, Wait ? timeoutFunction : null);
                WriteWarning("You changed the lockstate of a site. This change is not guaranteed to be effective immediately. Please wait a few minutes for this to take effect.");
            }
            if (!LockState.HasValue)
            {
                SetSiteProperties(timeoutFunction);
            }
#endif
        }

#if ONPREMISES
        private void SetSiteProperties()
#else
        private void SetSiteProperties(Func<TenantOperationMessage, bool> timeoutFunction)
#endif
        {
            var props = GetSiteProperties(Url);
            var updateRequired = false;

            if (ParameterSpecified(nameof(Title)))
            {
                props.Title = Title;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(DenyAddAndCustomizePages)))
            {
                props.DenyAddAndCustomizePages = DenyAddAndCustomizePages ? DenyAddAndCustomizePagesStatus.Enabled : DenyAddAndCustomizePagesStatus.Disabled;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(LocaleId)))
            {
                props.Lcid = LocaleId;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(AllowSelfServiceUpgrade)))
            {
                props.AllowSelfServiceUpgrade = AllowSelfServiceUpgrade;
                updateRequired = true;
            }

#if !ONPREMISES
            if (ParameterSpecified(nameof(SharingAllowedDomainList)))
            {
                props.SharingAllowedDomainList = SharingAllowedDomainList;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(SharingBlockedDomainList)))
            {
                props.SharingBlockedDomainList = SharingBlockedDomainList;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(SharingDomainRestrictionMode)))
            {
                props.SharingDomainRestrictionMode = SharingDomainRestrictionMode;
                updateRequired = true;
            }
#pragma warning disable CS0618 // Type or member is obsolete
                if (ParameterSpecified(nameof(UserCodeMaximumLevel)))
                {
                    props.UserCodeMaximumLevel = UserCodeMaximumLevel;
                    updateRequired = true;
                }
                if (ParameterSpecified(nameof(UserCodeWarningLevel)))
                {
                    props.UserCodeWarningLevel = UserCodeWarningLevel;
                    updateRequired = true;
                }
#pragma warning restore CS0618 // Type or member is obsolete
            if (ParameterSpecified(nameof(StorageMaximumLevel)))
            {
                props.StorageMaximumLevel = StorageMaximumLevel;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(StorageWarningLevel)))
            {
                props.StorageWarningLevel = StorageWarningLevel;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(SharingCapability)))
            {
                props.SharingCapability = SharingCapability;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(DefaultLinkPermission)))
            {
                props.DefaultLinkPermission = DefaultLinkPermission;
                updateRequired = true;
            }
            if (ParameterSpecified(nameof(DefaultSharingLinkType)))
            {
                props.DefaultSharingLinkType = DefaultSharingLinkType;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(BlockDownloadOfNonViewableFiles)))
            {
                props.AllowDownloadingNonWebViewableFiles = !BlockDownloadOfNonViewableFiles;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(CommentsOnSitePagesDisabled)))
            {
                props.CommentsOnSitePagesDisabled = CommentsOnSitePagesDisabled;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(DisableAppViews)))
            {
                props.DisableAppViews = DisableAppViews;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(DisableCompanyWideSharingLinks)))
            {
                props.DisableCompanyWideSharingLinks = DisableCompanyWideSharingLinks;
                updateRequired = true;
            }

            if (ParameterSpecified(nameof(DisableFlows)))
            {
                props.DisableFlows = DisableFlows;
                updateRequired = true;
            }
#endif

            if (updateRequired)
            {
                var op = props.Update();
                ClientContext.Load(op, i => i.IsComplete, i => i.PollingInterval);
                ClientContext.ExecuteQueryRetry();

#if !ONPREMISES
                if (Wait)
                {
                    WaitForIsComplete(ClientContext, op, timeoutFunction, TenantOperationMessage.SettingSiteProperties);
                }
#endif
            }

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
        }

        private SiteProperties GetSiteProperties(string url)
        {
            return Tenant.GetSitePropertiesByUrl(url, true);
        }

#if !ONPREMISES
        private bool TimeoutFunction(TenantOperationMessage message)
        {
            if (message == TenantOperationMessage.SettingSiteProperties || message == TenantOperationMessage.SettingSiteLockState)
            {
                Host.UI.Write(".");
            }
            return Stopping;
        }

        private bool WaitForIsComplete(ClientContext context, SpoOperation op, Func<TenantOperationMessage, bool> timeoutFunction = null, TenantOperationMessage operationMessage = TenantOperationMessage.None)
        {
            bool succeeded = true;
            while (!op.IsComplete)
            {
                if (timeoutFunction != null && timeoutFunction(operationMessage))
                {
                    succeeded = false;
                    break;
                }
                Thread.Sleep(op.PollingInterval);

                op.RefreshLoad();
                if (!op.IsComplete)
                {
                    try
                    {
                        context.ExecuteQueryRetry();
                    }
                    catch (WebException)
                    {
                        // Context connection gets closed after action completed.
                        // Calling ExecuteQuery again returns an error which can be ignored
                    }
                }
            }
            return succeeded;
        }
#endif
    }
}