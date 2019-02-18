#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPSite")]
    [CmdletHelp("Sets Site Collection properties.",
        Category = CmdletHelpCategory.Sites,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSite -Classification ""HBI""",
        Remarks = "Sets the current site classification to HBI",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSite -Classification $null",
        Remarks = "Unsets the current site classification",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSite -DisableFlows",
        Remarks = "Disables Flows for this site",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSite -DisableFlows:$false",
        Remarks = "Enables Flows for this site",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSite -SiteLogoPath c:\images\mylogo.png",
        Remarks = "Sets the logo if the site is a modern team site",
        SortOrder = 4)]
    public class SetSite : PnPCmdlet
    {

#if !ONPREMISES
        private const string ParameterSet_LOCKSTATE = "Set Lock State";
        private const string ParameterSet_PROPERTIES = "Set Properties";
#endif

        [Parameter(Mandatory = false)]
        [Alias("Url")]
        public string Identity;

        [Parameter(Mandatory = false, HelpMessage = "The classification to set", ParameterSetName = ParameterSet_PROPERTIES)]
        public string Classification;

        [Parameter(Mandatory = false, HelpMessage = "Disables flows for this site", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter DisableFlows;

        [Parameter(Mandatory = false, HelpMessage = "Sets the logo if the site is modern team site. If you want to set the logo for a classic site, use Set-PnPWeb -SiteLogoUrl", ParameterSetName = ParameterSet_PROPERTIES)]
        public string LogoFilePath;

        [Parameter(Mandatory = false, HelpMessage = "Specifies what the sharing capablilites are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingCapabilities? Sharing = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.", ParameterSetName = ParameterSet_PROPERTIES)]
        public long? StorageMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter", ParameterSetName = ParameterSet_PROPERTIES)]
        public long? StorageWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.", ParameterSetName = ParameterSet_PROPERTIES)]
        [Obsolete("Sandboxed solution code has been deprecated")]
        public double? UserCodeMaximumLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter", ParameterSetName = ParameterSet_PROPERTIES)]
        [Obsolete("Sandboxed solution code has been deprecated")]
        public double? UserCodeWarningLevel = null;

        [Parameter(Mandatory = false, HelpMessage = "Sets the lockstate of a site", ParameterSetName = ParameterSet_LOCKSTATE)]
        public SiteLockState? LockState;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the site administrator can upgrade the site collection", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter? AllowSelfServiceUpgrade = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if a site allows custom script or not. See https://support.office.com/en-us/article/Turn-scripting-capabilities-on-or-off-1f2c515f-5d7e-448a-9fd7-835da935584f for more information.", ParameterSetName = ParameterSet_PROPERTIES)]
        [Alias("DenyAndAddCustomizePages")]
        public SwitchParameter NoScriptSite;

        [Parameter(Mandatory = false, HelpMessage = "Specifies owner(s) to add as site collection adminstrators. They will be added as additional site collection administrators. Existing administrators will stay. Can be both users and groups.", ParameterSetName = ParameterSet_PROPERTIES)]
        public List<string> Owners;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if comments on site pages are enabled or disabled", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter CommentsOnSitePagesDisabled;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the default link permission for the site collection. None - Respect the organization default link permission. View - Sets the default link permission for the site to ""view"" permissions. Edit - Sets the default link permission for the site to ""edit"" permissions", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingPermissionType? DefaultLinkPermission;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the default link type for the site collection. None - Respect the organization default sharing link type. AnonymousAccess - Sets the default sharing link for this site to an Anonymous Access or Anyone link. Internal - Sets the default sharing link for this site to the ""organization"" link or company shareable link. Direct - Sets the default sharing link for this site to the ""Specific people"" link", ParameterSetName = ParameterSet_PROPERTIES)]
        public SharingLinkType? DefaultSharingLinkType;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_PROPERTIES)]
        public AppViewsPolicy? DisableAppViews;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_PROPERTIES)]
        public CompanyWideSharingLinksPolicy? DisableCompanyWideSharingLinks;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies to prevent non-owners from inviting new users to the site", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter DisableSharingForNonOwners;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the language of this site collection.", ParameterSetName = ParameterSet_PROPERTIES)]
        public uint? LocaleId;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the language of this site collection.", ParameterSetName = ParameterSet_PROPERTIES)]
        public string NewUrl;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the Geo/Region restrictions of this site.", ParameterSetName = ParameterSet_PROPERTIES)]
        public RestrictedToRegion? RestrictedToGeo;

        [Parameter(Mandatory = false, HelpMessage = @"Disables or enables the Social Bar for Site Collection.", ParameterSetName = ParameterSet_PROPERTIES)]
        public SwitchParameter SocialBarOnSitePagesDisabled;

        [Parameter(Mandatory = false, HelpMessage = "Wait for the operation to complete", ParameterSetName = ParameterSet_LOCKSTATE)]
        public SwitchParameter Wait;

        protected override void ExecuteCmdlet()
        {
            var context = ClientContext;
            var site = ClientContext.Site;
            var siteUrl = ClientContext.Url;

            var executeQueryRequired = false;

            if (!string.IsNullOrEmpty(Identity))
            {
                context = ClientContext.Clone(Identity);
                site = context.Site;
                siteUrl = context.Url;
            }


            if (MyInvocation.BoundParameters.ContainsKey("Classification"))
            {
                site.Classification = Classification;
                executeQueryRequired = true;
            }
            if (MyInvocation.BoundParameters.ContainsKey("LogoFilePath"))
            {
                var webTemplate = ClientContext.Web.EnsureProperty(w => w.WebTemplate);
                if (webTemplate == "GROUP")
                {
                    if (!System.IO.Path.IsPathRooted(LogoFilePath))
                    {
                        LogoFilePath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, LogoFilePath);
                    }
                    if (System.IO.File.Exists(LogoFilePath))
                    {
                        var bytes = System.IO.File.ReadAllBytes(LogoFilePath);
#if !NETSTANDARD2_0
                        var mimeType = System.Web.MimeMapping.GetMimeMapping(LogoFilePath);
#else
                        var mimeType = "";
                        if(LogoFilePath.EndsWith("gif",StringComparison.InvariantCultureIgnoreCase))
                        {
                            mimeType = "image/gif";
                        }
                        if(LogoFilePath.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase))
                        {
                            mimeType = "image/jpeg";
                        }
                        if(LogoFilePath.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
                        {
                            mimeType = "image/png";
                        }
#endif
                        var result = OfficeDevPnP.Core.Sites.SiteCollection.SetGroupImage(context, bytes, mimeType).GetAwaiter().GetResult();
                    }
                    else
                    {
                        throw new System.Exception("Logo file does not exist");
                    }
                }
                else
                {
                    throw new System.Exception("Not an Office365 group enabled site.");
                }
            }
            if (executeQueryRequired)
            {
                context.ExecuteQueryRetry();
            }

            if (IsTenantProperty())
            {
                var tenantAdminUrl = UrlUtilities.GetTenantAdministrationUrl(context.Url);
                context = context.Clone(tenantAdminUrl);

                executeQueryRequired = false;
                Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;
                Tenant tenant = new Tenant(context);
                var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, false);
                if (LockState.HasValue)
                {
                    tenant.SetSiteLockState(siteUrl, LockState.Value, Wait, Wait ? timeoutFunction : null);
                    WriteWarning("You changed the lockstate of this site. This change is not guaranteed to be effective immediately. Please wait a few minutes for this to take effect.");
                }

                if (Owners != null && Owners.Count > 0)
                {
                    var admins = new List<UserEntity>();
                    foreach (var owner in Owners)
                    {
                        var userEntity = new UserEntity { LoginName = owner };
                        admins.Add(userEntity);
                    }
                    tenant.AddAdministrators(admins, new Uri(siteUrl));
                }
                if (Sharing.HasValue)
                {
                    siteProperties.SharingCapability = Sharing.Value;
                    executeQueryRequired = true;
                }
                if (StorageMaximumLevel.HasValue)
                {
                    siteProperties.StorageMaximumLevel = StorageMaximumLevel.Value;
                    executeQueryRequired = true;
                }
                if (StorageWarningLevel.HasValue)
                {
                    siteProperties.StorageWarningLevel = StorageWarningLevel.Value;
                    executeQueryRequired = true;
                }
#pragma warning disable CS0618 // Type or member is obsolete
                if (UserCodeWarningLevel.HasValue)
                {
                    siteProperties.UserCodeWarningLevel = UserCodeWarningLevel.Value;
                    executeQueryRequired = true;
                }
                if (UserCodeMaximumLevel.HasValue)
                {
                    siteProperties.UserCodeMaximumLevel = UserCodeMaximumLevel.Value;
                    executeQueryRequired = true;
                }
#pragma warning restore CS0618 // Type or member is obsolete

                if (AllowSelfServiceUpgrade.HasValue)
                {
                    siteProperties.AllowSelfServiceUpgrade = AllowSelfServiceUpgrade.Value;
                    executeQueryRequired = true;
                }
                if (NoScriptSite.IsPresent)
                {
                    siteProperties.DenyAddAndCustomizePages = (NoScriptSite == true ? DenyAddAndCustomizePagesStatus.Enabled : DenyAddAndCustomizePagesStatus.Disabled);
                    executeQueryRequired = true;
                }
                if (CommentsOnSitePagesDisabled.IsPresent)
                {
                    siteProperties.CommentsOnSitePagesDisabled = CommentsOnSitePagesDisabled;
                    executeQueryRequired = true;
                }
                if (DefaultLinkPermission.HasValue)
                {
                    siteProperties.DefaultLinkPermission = DefaultLinkPermission.Value;
                    executeQueryRequired = true;
                }
                if (DefaultSharingLinkType.HasValue)
                {
                    siteProperties.DefaultSharingLinkType = DefaultSharingLinkType.Value;
                    executeQueryRequired = true;
                }
                if (DisableAppViews.HasValue)
                {
                    siteProperties.DisableAppViews = DisableAppViews.Value;
                    executeQueryRequired = true;
                }
                if (DisableCompanyWideSharingLinks.HasValue)
                {
                    siteProperties.DisableCompanyWideSharingLinks = DisableCompanyWideSharingLinks.Value;
                    executeQueryRequired = true;
                }
                if (DisableFlows.IsPresent)
                {
                    siteProperties.DisableFlows = DisableFlows ? FlowsPolicy.Disabled : FlowsPolicy.NotDisabled;
                    executeQueryRequired = true;
                }
                if (LocaleId.HasValue)
                {
                    siteProperties.Lcid = LocaleId.Value;
                    executeQueryRequired = true;
                }
                if (!string.IsNullOrEmpty(NewUrl))
                {
                    siteProperties.NewUrl = NewUrl;
                    executeQueryRequired = true;
                }
                if (RestrictedToGeo.HasValue)
                {
                    siteProperties.RestrictedToRegion = RestrictedToGeo.Value;
                    executeQueryRequired = true;
                }
                if (SocialBarOnSitePagesDisabled.IsPresent)
                {
                    siteProperties.SocialBarOnSitePagesDisabled = SocialBarOnSitePagesDisabled;
                    executeQueryRequired = true;
                }
                if (executeQueryRequired)
                {
                    siteProperties.Update();
                    tenant.Context.ExecuteQueryRetry();
                }

                if (DisableSharingForNonOwners.IsPresent)
                {
                    Office365Tenant office365Tenant = new Office365Tenant(context);
                    context.Load(office365Tenant);
                    context.ExecuteQueryRetry();
                    office365Tenant.DisableSharingForNonOwnersOfSite(siteUrl);
                    context.ExecuteQuery();
                }
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

        private bool IsTenantProperty() => LockState.HasValue ||
                (Owners != null && Owners.Count > 0) ||
                Sharing.HasValue ||
                StorageMaximumLevel.HasValue ||
                StorageWarningLevel.HasValue ||
#pragma warning disable CS0618 // Type or member is obsolete
                UserCodeMaximumLevel.HasValue ||
                UserCodeWarningLevel.HasValue ||
#pragma warning restore CS0618 // Type or member is obsolete
                AllowSelfServiceUpgrade.HasValue ||
                NoScriptSite.IsPresent ||
                CommentsOnSitePagesDisabled.IsPresent ||
                DefaultLinkPermission.HasValue ||
                DefaultSharingLinkType.HasValue ||
                DisableAppViews.HasValue ||
                DisableFlows.IsPresent ||
                DisableSharingForNonOwners.IsPresent ||
                LocaleId.HasValue ||
                !string.IsNullOrEmpty(NewUrl) ||
                RestrictedToGeo.HasValue ||
                SocialBarOnSitePagesDisabled.IsPresent;
    }
}
#endif