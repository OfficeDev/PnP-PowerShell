#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using System;
using Microsoft.Online.SharePoint.TenantManagement;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPTenant", DefaultParameterSetName = ParameterAttribute.AllParameterSets)]
    [CmdletHelp(@"Sets organization-level site collection properties",
        DetailedDescription = @"Sets organization-level site collection properties such as StorageQuota, StorageQuotaAllocated, ResourceQuota,
ResourceQuotaAllocated, and SiteCreationMode.

You must have the SharePoint Online admin or Global admin role to run the cmdlet.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantSite -Identity https://contoso.sharepoint.com/sites/team1 -LockState NoAccess
Set-PnPTenant -NoAccessRedirectUrl 'http://www.contoso.com'",
        Remarks = @"This example blocks access to https://contoso.sharepoint.com/sites/team1 and redirects traffic to http://www.contoso.com.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenant -ShowEveryoneExceptExternalUsersClaim $false",
        Remarks = @"This example hides the ""Everyone Except External Users"" claim in People Picker.", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenant -ShowAllUsersClaim $false",
        Remarks = @"This example hides the ""All Users"" claim group in People Picker.", SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenant -UsePersistentCookiesForExplorerView $true",
        Remarks = @"This example enables the use of special persisted cookie for Open with Explorer.", SortOrder = 3)]
    public class SetTenant : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Specifies the lower bound on the compatibility level for new sites.")]
        public int MinCompatibilityLevel;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Specifies the upper bound on the compatibility level for new sites.")]
        public int MaxCompatibilityLevel;

        [Parameter(Mandatory = false, HelpMessage = @"Enables external services for a tenant.
External services are defined as services that are not in the Office 365 datacenters.

The valid values are:
True (default) - External services are enabled for the tenant.
False - External services that are outside of the Office 365 datacenters cannot interact with SharePoint.")]
        public bool? ExternalServicesEnabled;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the URL of the redirected site for those site collections which have the locked state ""NoAccess""

The valid values are:
""""(default) - Blank by default, this will also remove or clear any value that has been set.
Full URL - Example: https://contoso.sharepoint.com/Pages/Locked.aspx")]
        public string NoAccessRedirectUrl;

        [Parameter(Mandatory = false, HelpMessage = @"Determines what level of sharing is available for the site.

The valid values are:
ExternalUserAndGuestSharing (default) - External user sharing (share by email) and guest link sharing are both enabled. Disabled - External user sharing (share by email) and guest link sharing are both disabled.
ExternalUserSharingOnly - External user sharing (share by email) is enabled, but guest link sharing is disabled.

For more information about sharing, see Manage external sharing for your SharePoint online environment (http://office.microsoft.com/en-us/office365-sharepoint-online-enterprise-help/manage-external-sharing-for-your-sharepoint-online-environment-HA102849864.aspx).")]
        public SharingCapabilities? SharingCapability;

        [Parameter(Mandatory = false, HelpMessage = @"Determines whether tenant users see the Start a Site menu option.

The valid values are:
True (default) - Tenant users will see the Start a Site menu option.
False - Start a Site is hidden from the menu.")]
        public bool? DisplayStartASiteOption;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies URL of the form to load in the Start a Site dialog.

The valid values are:
"""" (default) - Blank by default, this will also remove or clear any value that has been set.
Full URL - Example: ""https://contoso.sharepoint.com/path/to/form""" )]
        public string StartASiteFormUrl;

        [Parameter(Mandatory = false, HelpMessage = @"Enables the administrator to hide the Everyone claim in the People Picker.
When users share an item with Everyone, it is accessible to all authenticated users in the tenant's Azure Active Directory, including any active external users who have previously accepted invitations.

Note, that some SharePoint system resources such as templates and pages are required to be shared to Everyone and this type of sharing does not expose any user data or metadata.

The valid values are:
True (default) - The Everyone claim group is displayed in People Picker.
False - The Everyone claim group is hidden from the People Picker.")]
        public bool? ShowEveryoneClaim;

        [Parameter(Mandatory = false, HelpMessage = @"Enables the administrator to hide the All Users claim groups in People Picker.

When users share an item with ""All Users (x)"", it is accessible to all organization members in the tenant's Azure Active Directory who have authenticated with via this method. When users share an item with ""All Users (x)"" it is accessible to all organization members in the tenant that used NTLM to authentication with SharePoint.

Note, the All Users(authenticated) group is equivalent to the Everyone claim, and shows as Everyone.To change this, see - ShowEveryoneClaim.

The valid values are:
True(default) - The All Users claim groups are displayed in People Picker.
False - The All Users claim groups are hidden in People Picker.")]
        public bool? ShowAllUsersClaim;

        [Parameter(Mandatory = false, HelpMessage = @"Enables the administrator to hide the ""Everyone except external users"" claim in the People Picker.
When users share an item with ""Everyone except external users"", it is accessible to all organization members in the tenant's Azure Active Directory, but not to any users who have previously accepted invitations.

The valid values are:
True(default) - The Everyone except external users is displayed in People Picker.
False - The Everyone except external users claim is not visible in People Picker.")]
        public bool? ShowEveryoneExceptExternalUsersClaim;

        [Parameter(Mandatory = false, HelpMessage = @"Removes the search capability from People Picker. Note, recently resolved names will still appear in the list until browser cache is cleared or expired.

SharePoint Administrators will still be able to use starts with or partial name matching when enabled.

The valid values are:
False (default) - Starts with / partial name search functionality is available.
True - Disables starts with / partial name search functionality for all SharePoint users, except SharePoint Admins.")]
        public bool? SearchResolveExactEmailOrUPN;

        [Parameter(Mandatory = false, HelpMessage = @"When set to true this will disable the ability to use Modern Authentication that leverages ADAL across the tenant.

The valid values are:
False (default) - Modern Authentication is enabled/allowed.
True - Modern Authentication via ADAL is disabled.")]
        public bool? OfficeClientADALDisabled;

        [Parameter(Mandatory = false, HelpMessage = @"By default this value is set to $true.

Setting this parameter prevents Office clients using non-modern authentication protocols from accessing SharePoint Online resources.

A value of $true - Enables Office clients using non-modern authentication protocols(such as, Forms-Based Authentication (FBA) or Identity Client Runtime Library (IDCRL)) to access SharePoint resources.

A value of $false - Prevents Office clients using non-modern authentication protocols from accessing SharePoint Online resources.

Note:
This may also prevent third-party apps from accessing SharePoint Online resources.Also, this will also block apps using the SharePointOnlineCredentials class to access SharePoint Online resources.For additional information about SharePointOnlineCredentials, see SharePointOnlineCredentials class.")]
        public bool? LegacyAuthProtocolsEnabled;

        [Parameter(Mandatory = false, HelpMessage = @"Ensures that an external user can only accept an external sharing invitation with an account matching the invited email address.

Administrators who desire increased control over external collaborators should consider enabling this feature.

Note, this only applies to new external users accepting new sharing invitations. Also, the resource owner must share with an organizational or Microsoft account or the external user will be unable to access the resource.

The valid values are:
False (default) - When a document is shared with an external user, bob@contoso.com, it can be accepted by any user with access to the invitation link in the original e-mail.
True - User must accept this invitation with bob@contoso.com.")]
        public bool? RequireAcceptingAccountMatchInvitedAccount;

        [Parameter(Mandatory = false, HelpMessage = @"Creates a Shared with Everyone folder in every user's new OneDrive for Business document library.

The valid values are:
True (default) - The Shared with Everyone folder is created.
False - No folder is created when the site and OneDrive for Business document library is created.

The default behavior of the Shared with Everyone folder changed in August 2015.
For additional information about the change, see Provision the Shared with Everyone folder in OneDrive for Business (https://support.office.com/en-us/article/Provision-the-Shared-with-Everyone-folder-in-OneDrive-for-Business-6bb02c91-fd0b-42ba-9457-3921cb6dc5b2?ui=en-US&rs=en-US&ad=US)")]
        public bool? ProvisionSharedWithEveryoneFolder;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the home realm discovery value to be sent to Azure Active Directory (AAD) during the user sign-in process.

When the organization uses a third-party identity provider, this prevents the user from seeing the Azure Active Directory Home Realm Discovery web page and ensures the user only sees their company's Identity Provider's portal.
This value can also be used with Azure Active Directory Premium to customize the Azure Active Directory login page.

Acceleration will not occur on site collections that are shared externally.

This value should be configured with the login domain that is used by your company (that is, example@contoso.com).

If your company has multiple third-party identity providers, configuring the sign-in acceleration value will break sign-in for your organization.

The valid values are:
"""" (default) - Blank by default, this will also remove or clear any value that has been set.
Login Domain - For example: ""contoso.com""")]
        public string SignInAccelerationDomain;

        [Parameter(Mandatory = false, HelpMessage = @"Accelerates guest-enabled site collections as well as member-only site collections when the SignInAccelerationDomain parameter is set.

Note:
If enabled, your identity provider must be capable of authenticating guest users. If it is not, guest users will be unable to log in and access content that was shared with them.")]
        public bool? EnableGuestSignInAcceleration;

        [Parameter(Mandatory = false, HelpMessage = @"Lets SharePoint issue a special cookie that will allow this feature to work even when ""Keep Me Signed In"" is not selected.

""Open with Explorer"" requires persisted cookies to operate correctly.
When the user does not select ""Keep Me Signed in"" at the time of sign -in, ""Open with Explorer"" will fail.

This special cookie expires after 30 minutes and cannot be cleared by closing the browser or signing out of SharePoint Online.To clear this cookie, the user must log out of their Windows session.

The valid values are:
False(default) - No special cookie is generated and the normal Office 365 sign -in length / timing applies.
True - Generates a special cookie that will allow ""Open with Explorer"" to function if the ""Keep Me Signed In"" box is not checked at sign -in.")]
        public bool? UsePersistentCookiesForExplorerView;

        [Parameter(Mandatory = false, HelpMessage = @"When the feature is enabled, all external sharing invitations that are sent will blind copy the e-mail messages listed in the BccExternalSharingInvitationsList.

The valid values are:
False (default) - BCC for external sharing is disabled.
True - All external sharing invitations that are sent will blind copy the e-mail messages listed in the BccExternalSharingInvitationsList.")]
        public bool? BccExternalSharingInvitations;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies a list of e-mail addresses to be BCC'd when the BCC for External Sharing feature is enabled.
Multiple addresses can be specified by creating a comma separated list with no spaces.

The valid values are:
"""" (default) - Blank by default, this will also clear any value that has been set.
Single or Multiple e-mail addresses - joe@contoso.com or joe@contoso.com,bob@contoso.com")]
        public string BccExternalSharingInvitationsList;

        [Parameter(Mandatory = false)]
        public bool? UserVoiceForFeedbackEnabled;

        [Parameter(Mandatory = false)]
        public bool? PublicCdnEnabled;

        [Parameter(Mandatory = false)]
        public string PublicCdnAllowedFileTypes;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies all anonymous links that have been created (or will be created) will expire after the set number of days .

To remove the expiration requirement, set the value to zero (0).")]
        public int? RequireAnonymousLinksExpireInDays;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies a list of email domains that is allowed for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, ""contoso.com fabrikam.com"".

For additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business")]
        public string SharingAllowedDomainList;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies a list of email domains that is blocked or prohibited for sharing with the external collaborators. Use space character as the delimiter for entering multiple values. For example, ""contoso.com fabrikam.com"".

For additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business")]
        public string SharingBlockedDomainList;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the external sharing mode for domains.

The following values are: None AllowList BlockList

For additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business.")]
        public SharingDomainRestrictionModes? SharingDomainRestrictionMode;

        [Parameter(Mandatory = false, HelpMessage = @"Sets a default OneDrive for Business storage quota for the tenant. It will be used for new OneDrive for Business sites created.

A typical use will be to reduce the amount of storage associated with OneDrive for Business to a level below what the License entitles the users. For example, it could be used to set the quota to 10 gigabytes (GB) by default.

If value is set to 0, the parameter will have no effect.

If the value is set larger than the Maximum allowed OneDrive for Business quota, it will have no effect.")]
        public long? OneDriveStorageQuota;

        [Parameter(Mandatory = false, HelpMessage = @"Lets OneDrive for Business creation for administrator managed guest users. Administrator managed Guest users use credentials in the resource tenant to access the resources.

The valid values are the following:

$true-Administrator managed Guest users can be given OneDrives, provided needed licenses are assigned.

$false- Administrator managed Guest users can't be given OneDrives as functionality is turned off.")]
        public bool? OneDriveForGuestsEnabled;

        [Parameter(Mandatory = false, HelpMessage = @"Allows access from network locations that are defined by an administrator.

The values are $true and $false. The default value is $false which means the setting is disabled.

Before the IPAddressEnforcement parameter is set, make sure you add a valid IPv4 or IPv6 address to the IPAddressAllowList parameter.")]
        public bool? IPAddressEnforcement;

        [Parameter(Mandatory = false, HelpMessage = @"Configures multiple IP addresses or IP address ranges (IPv4 or IPv6).

Use commas to separate multiple IP addresses or IP address ranges. Verify there are no overlapping IP addresses and ensure IP ranges use Classless Inter-Domain Routing (CIDR) notation. For example, 172.16.0.0, 192.168.1.0/27.

Note:
The IPAddressAllowList parameter only lets administrators set IP addresses or ranges that are recognized as trusted. To only grant access from these IP addresses or ranges, set the IPAddressEnforcement parameter to $true.")]
        public string IPAddressAllowList;

        [Parameter(Mandatory = false)]
        public int? IPAddressWACTokenLifetime;

        [Parameter(Mandatory = false, HelpMessage = @"Note:
When set to $true, users aren't able to share with security groups or SharePoint groups.")]
        public bool? UseFindPeopleInPeoplePicker;

        [Parameter(Mandatory = false, HelpMessage = @"Lets administrators choose what type of link appears is selected in the “Get a link” sharing dialog box in OneDrive for Business and SharePoint Online.

For additional information about how to change the default link type, see Change the default link type when users get links for sharing.

Note:
Setting this value to “none” will default “get a link” to the most permissive link available (that is, if anonymous links are enabled, the default link will be anonymous access; if they are disabled then the default link will be internal.

The values are: None Direct Internal AnonymousAccess")]
        public SharingLinkType? DefaultSharingLinkType;

        [Parameter(Mandatory = false, HelpMessage = @"Lets administrators set policy on re-sharing behavior in OneDrive for Business.

Values:

On- Users with edit permissions can re-share.

Off- Only OneDrive for Business owner can share. The value of ODBAccessRequests defines whether a request to share gets sent to the owner.

Unspecified- Let each OneDrive for Business owner enable or disable re-sharing behavior on their OneDrive.")]
        public SharingState? ODBMembersCanShare;

        [Parameter(Mandatory = false, HelpMessage = @"Lets administrators set policy on access requests and requests to share in OneDrive for Business.

Values:

On- Users without permission to share can trigger sharing requests to the OneDrive for Business owner when they attempt to share. Also, users without permission to a file or folder can trigger access requests to the OneDrive for Business owner when they attempt to access an item they do not have permissions to.

Off- Prevent access requests and requests to share on OneDrive for Business.

Unspecified- Let each OneDrive for Business owner enable or disable access requests and requests to share on their OneDrive.")]
        public SharingState? ODBAccessRequests;

        [Parameter(Mandatory = false)]
        public bool? PreventExternalUsersFromResharing;

        [Parameter(Mandatory = false)]
        public bool? ShowPeoplePickerSuggestionsForGuestUsers;

        [Parameter(Mandatory = false)]
        public AnonymousLinkType? FileAnonymousLinkType;

        [Parameter(Mandatory = false)]
        public AnonymousLinkType? FolderAnonymousLinkType;

        [Parameter(Mandatory = false, HelpMessage = @"When this parameter is set to $true and another user re-shares a document from a user’s OneDrive for Business, the OneDrive for Business owner is notified by e-mail.

For additional information about how to configure notifications for external sharing, see Configure notifications for external sharing for OneDrive for Business.

The values are $true and $false.")]
        public bool? NotifyOwnersWhenItemsReshared;

        [Parameter(Mandatory = false, HelpMessage = @"When this parameter is set to $true and when an external user accepts an invitation to a resource in a user’s OneDrive for Business, the OneDrive for Business owner is notified by e-mail.

For additional information about how to configure notifications for external sharing, see Configure notifications for external sharing for OneDrive for Business.

The values are $true and $false.")]
        public bool? NotifyOwnersWhenInvitationsAccepted;

        [Parameter(Mandatory = false)]
        public bool? NotificationsInOneDriveForBusinessEnabled;

        [Parameter(Mandatory = false)]
        public bool? NotificationsInSharePointEnabled;

        [Parameter(Mandatory = false, HelpMessage = @"Permits the use of special characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.

Note:
The only two characters that can be managed at this time are the # and % characters.

The following are the valid values:

NoPreference- Support for feature will be enabled by Microsoft on your Office 365 tenant.

Allowed- Lets the # and % characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.

Disallowed- Disallows the # and % characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.")]
        public SpecialCharactersState? SpecialCharactersStateInFileFolderNames { get; set; }

        [Parameter(Mandatory = false)]
        public bool? OwnerAnonymousNotification;

        [Parameter(Mandatory = false)]
        public bool? CommentsOnSitePagesDisabled;

        [Parameter(Mandatory = false)]
        public bool? SocialBarOnSitePagesDisabled;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the number of days after a user's Active Directory account is deleted that their OneDrive for Business content will be deleted.

The value range is in days, between 30 and 3650. The default value is 30.")]
        public int? OrphanedPersonalSitesRetentionPeriod;

        [Parameter(Mandatory = false, HelpMessage = @"Prevents the Download button from being displayed on the Virus Found warning page.

Accepts a value of true (enabled) to hide the Download button or false (disabled) to display the Download button. By default this feature is set to false.

")]
        public bool? DisallowInfectedFileDownload;

        [Parameter(Mandatory = false)]
        public SharingPermissionType? DefaultLinkPermission;

        [Parameter(Mandatory = false)]
        public SPOConditionalAccessPolicyType? ConditionalAccessPolicy;

        [Parameter(Mandatory = false)]
        public bool? AllowDownloadingNonWebViewableFiles;

        [Parameter(Mandatory = false)]
        public bool? AllowEditing;

        [Parameter(Mandatory = false)]
        public bool? ApplyAppEnforcedRestrictionsToAdHocRecipients;

        [Parameter(Mandatory = false)]
        public bool? FilePickerExternalImageSearchEnabled;

        [Parameter(Mandatory = false)]
        public bool? EmailAttestationRequired;

        [Parameter(Mandatory = false)]
        public int? EmailAttestationReAuthDays;

        [Parameter(Mandatory = false, HelpMessage = "Defines if the default themes are visible or hidden")]
        public bool? HideDefaultThemes;

        [Parameter(Mandatory = false, HelpMessage = "Guids of out of the box modern web part id's to hide")]
        public Guid[] DisabledWebPartIds;

        [Parameter(Mandatory = false, HelpMessage = "Boolean indicating if Azure Information Protection (AIP) should be enabled on the tenant. For more information, see https://docs.microsoft.com/microsoft-365/compliance/sensitivity-labels-sharepoint-onedrive-files#use-powershell-to-enable-support-for-sensitivity-labels")]
        public bool? EnableAIPIntegration;

        protected override void ExecuteCmdlet()
        {
            ClientContext.Load(Tenant);
            ClientContext.ExecuteQueryRetry();

            bool isDirty = false;
            if (MinCompatibilityLevel != 0 && MaxCompatibilityLevel != 0)
            {
                var compatibilityRange = $"{MinCompatibilityLevel},{MaxCompatibilityLevel}";
                Tenant.CompatibilityRange = compatibilityRange;
                isDirty = true;
            }
            else if (MinCompatibilityLevel != 0 || MaxCompatibilityLevel != 0)
            {
                throw new ArgumentNullException("You must specify both Min & Max compatibility levels together");
            }
            if (NoAccessRedirectUrl != null)
            {
                Tenant.NoAccessRedirectUrl = NoAccessRedirectUrl;
                isDirty = true;
            }
            if (ExternalServicesEnabled.HasValue)
            {
                Tenant.ExternalServicesEnabled = ExternalServicesEnabled.Value;
                isDirty = true;
            }
            if (DisplayStartASiteOption.HasValue)
            {
                Tenant.DisplayStartASiteOption = DisplayStartASiteOption.Value;
                isDirty = true;
            }
            if (SharingCapability != null)
            {
                Tenant.SharingCapability = SharingCapability.Value;
                isDirty = true;
            }
            if (StartASiteFormUrl != null)
            {
                Tenant.StartASiteFormUrl = StartASiteFormUrl;
                isDirty = true;
            }
            if (ShowEveryoneClaim.HasValue)
            {
                Tenant.ShowEveryoneClaim = ShowEveryoneClaim.Value;
                isDirty = true;
            }
            if (ShowAllUsersClaim.HasValue)
            {
                Tenant.ShowAllUsersClaim = ShowAllUsersClaim.Value;
                isDirty = true;
            }
            if (OfficeClientADALDisabled.HasValue)
            {
                Tenant.OfficeClientADALDisabled = OfficeClientADALDisabled.Value;
                isDirty = true;
            }
            if (LegacyAuthProtocolsEnabled.HasValue)
            {
                Tenant.LegacyAuthProtocolsEnabled = LegacyAuthProtocolsEnabled.Value;
                isDirty = true;
            }
            if (ShowEveryoneExceptExternalUsersClaim.HasValue)
            {
                Tenant.ShowEveryoneExceptExternalUsersClaim = ShowEveryoneExceptExternalUsersClaim.Value;
                isDirty = true;
            }
            if (SearchResolveExactEmailOrUPN.HasValue)
            {
                Tenant.SearchResolveExactEmailOrUPN = SearchResolveExactEmailOrUPN.Value;
                isDirty = true;
            }
            if (RequireAcceptingAccountMatchInvitedAccount.HasValue)
            {
                Tenant.RequireAcceptingAccountMatchInvitedAccount = RequireAcceptingAccountMatchInvitedAccount.Value;
                isDirty = true;
            }
            if (ProvisionSharedWithEveryoneFolder.HasValue)
            {
                Tenant.ProvisionSharedWithEveryoneFolder = ProvisionSharedWithEveryoneFolder.Value;
                isDirty = true;
            }
            if (SignInAccelerationDomain != null && ShouldContinue($@"Please confirm that ""{SignInAccelerationDomain}"" is correct, and you have federated sign-in configured for that domain. Otherwise, your users will no longer be able to sign in. Do you want to continue?", "Confirm"))
            {
                Tenant.SignInAccelerationDomain = SignInAccelerationDomain;
                isDirty = true;
            }
            if (EnableGuestSignInAcceleration.HasValue)
            {
                if (string.IsNullOrWhiteSpace(Tenant.SignInAccelerationDomain))
                {
                    throw new InvalidOperationException("This setting cannot be changed until you set the SignInAcceleration Domain.");
                }
                if (ShouldContinue("Make sure that your federated sign-in supports guest users. If it doesn’t, your guest users will no longer be able to sign in after you set EnableGuestSignInAcceleration to $true.", "Confirm"))
                {
                    Tenant.EnableGuestSignInAcceleration = EnableGuestSignInAcceleration.Value;
                    isDirty = true;
                }
            }
            if (UsePersistentCookiesForExplorerView.HasValue)
            {
                Tenant.UsePersistentCookiesForExplorerView = UsePersistentCookiesForExplorerView.Value;
                isDirty = true;
            }
            if (BccExternalSharingInvitations.HasValue && (!BccExternalSharingInvitations.Value || (BccExternalSharingInvitations.Value && ShouldContinue("The recipients listed in BccExternalSharingInvitationsList will be blind copied on all external sharing invitations. Do you want to continue?", "Confirm"))))
            {
                Tenant.BccExternalSharingInvitations = BccExternalSharingInvitations.Value;
                isDirty = true;
            }
            if (!string.IsNullOrEmpty(BccExternalSharingInvitationsList))
            {
                Tenant.BccExternalSharingInvitationsList = BccExternalSharingInvitationsList;
                isDirty = true;
            }
            if (UserVoiceForFeedbackEnabled.HasValue)
            {
                Tenant.UserVoiceForFeedbackEnabled = UserVoiceForFeedbackEnabled.Value;
                isDirty = true;
            }
            if (PublicCdnEnabled != null)
            {
                Tenant.PublicCdnEnabled = PublicCdnEnabled.Value;
                isDirty = true;
            }
            if (!string.IsNullOrEmpty(PublicCdnAllowedFileTypes))
            {
                Tenant.PublicCdnAllowedFileTypes = PublicCdnAllowedFileTypes;
                isDirty = true;
            }
            if (RequireAnonymousLinksExpireInDays.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.SharingCapability);
                    if (Tenant.SharingCapability != SharingCapabilities.ExternalUserAndGuestSharing)
                    {
                        WriteWarning("Warning: anonymous links are not enabled on your tenant. Enable them with SharingCapability.");
                    }
                    if (RequireAnonymousLinksExpireInDays.Value != 0 && (RequireAnonymousLinksExpireInDays.Value < 1 || RequireAnonymousLinksExpireInDays.Value > 730))
                    {
                        throw new ArgumentException("RequireAnonymousLinksExpireInDays must have a value between 1 and 730");
                    }
                    int requireAnonymousLinksExpireInDays = Tenant.EnsureProperty(t => t.RequireAnonymousLinksExpireInDays);
                    if (requireAnonymousLinksExpireInDays != RequireAnonymousLinksExpireInDays.Value)
                    {
                        Tenant.RequireAnonymousLinksExpireInDays = RequireAnonymousLinksExpireInDays.Value;
                        isDirty = true;
                    }
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property RequireAnonymousLinksExpireInDays is not supported by your version of the service");
                }
            }
            if (SharingAllowedDomainList != null)
            {
                if (!Tenant.RequireAcceptingAccountMatchInvitedAccount)
                {
                    WriteWarning("We automatically enabled RequireAcceptingAccountMatchInvitedAccount because you selected to limit external sharing using domains.");
                    Tenant.RequireAcceptingAccountMatchInvitedAccount = true;
                }
                Tenant.SharingAllowedDomainList = SharingAllowedDomainList;
                isDirty = true;
                if ((SharingDomainRestrictionMode == null && Tenant.SharingDomainRestrictionMode != SharingDomainRestrictionModes.AllowList) || SharingDomainRestrictionMode == SharingDomainRestrictionModes.None)
                {
                    WriteWarning("You must set SharingDomainRestrictionMode to AllowList in order to have the list of domains you configured for SharingAllowedDomainList to take effect.");
                }
                else if (SharingDomainRestrictionMode == SharingDomainRestrictionModes.BlockList)
                {
                    WriteWarning("The list of domains in SharingAllowedDomainsList is ignored when you set the SharingDomainRestrictionMode to BlockList. Set the list of blocked domains using the SharingBlockedDomainsList parameter.");
                }
            }
            if (PreventExternalUsersFromResharing.HasValue)
            {
                Tenant.PreventExternalUsersFromResharing = PreventExternalUsersFromResharing.Value;
                isDirty = true;
            }
            if (NotifyOwnersWhenItemsReshared.HasValue)
            {
                Tenant.NotifyOwnersWhenItemsReshared = NotifyOwnersWhenItemsReshared.Value;
                isDirty = true;
            }
            if (NotifyOwnersWhenInvitationsAccepted.HasValue)
            {
                Tenant.NotifyOwnersWhenInvitationsAccepted = NotifyOwnersWhenInvitationsAccepted.Value;
                isDirty = true;
            }
            if (NotificationsInOneDriveForBusinessEnabled.HasValue)
            {
                Tenant.NotificationsInOneDriveForBusinessEnabled = NotificationsInOneDriveForBusinessEnabled.Value;
                isDirty = true;
            }
            if (NotificationsInSharePointEnabled.HasValue)
            {
                Tenant.NotificationsInSharePointEnabled = NotificationsInSharePointEnabled.Value;
                isDirty = true;
            }
            if (SpecialCharactersStateInFileFolderNames.HasValue)
            {
                Tenant.SpecialCharactersStateInFileFolderNames = SpecialCharactersStateInFileFolderNames.Value;
                isDirty = true;
            }
            if (OwnerAnonymousNotification.HasValue)
            {
                Tenant.OwnerAnonymousNotification = OwnerAnonymousNotification.Value;
                isDirty = true;
            }
            if (OrphanedPersonalSitesRetentionPeriod.HasValue)
            {
                if (OrphanedPersonalSitesRetentionPeriod.Value < 30 || OrphanedPersonalSitesRetentionPeriod > 3650)
                {
                    throw new ArgumentException("OrphanedPersonalSitesRetentionPeriod must have a value between 30 and 3650");
                }
                if (ShouldContinue("This will update the Retention Policy for All Orphaned OneDrive for Business sites.", "Confirm"))
                {
                    try
                    {
                        Tenant.OrphanedPersonalSitesRetentionPeriod = OrphanedPersonalSitesRetentionPeriod.Value;
                        isDirty = true;
                    }
                    catch (PropertyOrFieldNotInitializedException)
                    {
                        throw new InvalidOperationException("Setting the property OrphanedPersonalSitesRetentionPeriod is not supported by your version of the service");
                    }
                }
            }

            if (DisallowInfectedFileDownload.HasValue)
            {
                Tenant.DisallowInfectedFileDownload = DisallowInfectedFileDownload.Value;
                isDirty = true;
            }
            if (!string.IsNullOrEmpty(SharingBlockedDomainList))
            {
                if (!Tenant.RequireAcceptingAccountMatchInvitedAccount)
                {
                    WriteWarning("We automatically enabled RequireAcceptingAccountMatchInvitedAccount because you selected to limit external sharing using domains.");
                    Tenant.RequireAcceptingAccountMatchInvitedAccount = true;
                }
                Tenant.SharingBlockedDomainList = SharingBlockedDomainList;
                isDirty = true;
                if ((SharingDomainRestrictionMode == null && Tenant.SharingDomainRestrictionMode != SharingDomainRestrictionModes.BlockList) || SharingDomainRestrictionMode == SharingDomainRestrictionModes.None)
                {
                    WriteWarning("You must set SharingDomainRestrictionMode to BlockList in order to have the list of domains you configured for SharingBlockedDomainList to take effect");
                }
                else if (SharingDomainRestrictionMode == SharingDomainRestrictionModes.AllowList)
                {
                    WriteWarning("The list of domains in SharingBlockedDomainsList is ignored when you set the SharingDomainRestrictionMode to AllowList.Set the list of allowed domains using the SharingAllowedDomainsList parameter.");
                }
            }
            if (SharingDomainRestrictionMode.HasValue)
            {
                if (SharingDomainRestrictionMode == SharingDomainRestrictionModes.AllowList && string.IsNullOrEmpty(Tenant.SharingAllowedDomainList))
                {
                    throw new InvalidOperationException("You set the SharingDomainRestrictionMode to AllowList but there are no domains to allow. Specify the list of allowed domains using the SharingAllowedDomainList parameter.");
                }
                if (SharingDomainRestrictionMode == SharingDomainRestrictionModes.BlockList && string.IsNullOrEmpty(Tenant.SharingBlockedDomainList))
                {
                    throw new InvalidOperationException("You set the SharingDomainRestrictionMode to BlockList but there are no domains to block. Specify the list of blocked domains using the SharingBlockedDomainsList parameter.");
                }
                if (!Tenant.RequireAcceptingAccountMatchInvitedAccount)
                {
                    WriteWarning("We automatically enabled RequireAcceptingAccountMatchInvitedAccount because you selected to limit external sharing using domains.");
                    Tenant.RequireAcceptingAccountMatchInvitedAccount = true;
                }
                Tenant.SharingDomainRestrictionMode = SharingDomainRestrictionMode.Value;
                isDirty = true;
            }
            if (OneDriveStorageQuota.HasValue)
            {
                Tenant.OneDriveStorageQuota = OneDriveStorageQuota.Value;
                isDirty = true;
            }
            if (OneDriveForGuestsEnabled.HasValue)
            {
                string message = OneDriveForGuestsEnabled.Value ? "This will enable all users, including guests, to create OneDrive for Business sites. You must first assign OneDrive for Business licenses to the guests before they can create their OneDrive for Business sites." : "Guests will no longer be able to create new OneDrive for Business sites. Existing sites won’t be impacted.";
                if (ShouldContinue(message, "Confirm"))
                {
                    Tenant.OneDriveForGuestsEnabled = OneDriveForGuestsEnabled.Value;
                    isDirty = true;
                }
            }
            if (IPAddressEnforcement.HasValue)
            {
                if (IPAddressEnforcement == true && string.IsNullOrEmpty(Tenant.IPAddressAllowList))
                {
                    throw new InvalidOperationException("You are setting IPAddressEnforcement to true, but the allow list of IPAddresses is empty. Please set it using the IPAddressAllowList parameter");
                }
                Tenant.IPAddressEnforcement = IPAddressEnforcement.Value;
                isDirty = true;
            }
            if (!string.IsNullOrEmpty(IPAddressAllowList))
            {
                Tenant.IPAddressAllowList = IPAddressAllowList;
                isDirty = true;
                if ((IPAddressEnforcement == null && !Tenant.IPAddressEnforcement) || IPAddressEnforcement == false)
                {
                    WriteWarning("The list of IP Addresses you provided will not be enforced until you set IPAddressEnforcement to true");
                }
            }
            if (IPAddressWACTokenLifetime.HasValue)
            {
                if (!(IPAddressWACTokenLifetime >= 15) || !(IPAddressWACTokenLifetime <= 600))
                {
                    throw new InvalidOperationException("The value must be in the range 15-1440 minutes");
                }
                Tenant.IPAddressWACTokenLifetime = IPAddressWACTokenLifetime.Value;
                isDirty = true;
            }
            if (UseFindPeopleInPeoplePicker.HasValue)
            {
                Tenant.UseFindPeopleInPeoplePicker = UseFindPeopleInPeoplePicker.Value;
                isDirty = true;
            }
            if (ShowPeoplePickerSuggestionsForGuestUsers.HasValue)
            {
                Tenant.ShowPeoplePickerSuggestionsForGuestUsers = ShowPeoplePickerSuggestionsForGuestUsers.Value;
                isDirty = true;
            }
            if (DefaultSharingLinkType.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.DefaultSharingLinkType);
                    if (Tenant.DefaultSharingLinkType != DefaultSharingLinkType.Value)
                    {
                        if (DefaultSharingLinkType.Value == SharingLinkType.AnonymousAccess && Tenant.SharingCapability != SharingCapabilities.ExternalUserAndGuestSharing)
                        {
                            WriteWarning(@"Anonymous access links aren’t enabled for your organization. You must first enable them by running the command ""Set-PnPTenant -SharingCapability ExternalUserAndGuestSharing"" before you can set the DefaultSharingLinkType parameter to AnonymousAccess. We will not set the value in this case.");
                        }
                        else
                        {
                            Tenant.DefaultSharingLinkType = DefaultSharingLinkType.Value;
                        }
                    }
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property DefaultSharingLinkType is not supported by your version of the service");
                }


            }
            if (ODBMembersCanShare.HasValue)
            {
                Tenant.ODBMembersCanShare = ODBMembersCanShare.Value;
                isDirty = true;
            }
            if (ODBAccessRequests.HasValue)
            {
                Tenant.ODBAccessRequests = ODBAccessRequests.Value;
                isDirty = true;
            }
            if (FileAnonymousLinkType.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.FileAnonymousLinkType);
                    if (Tenant.FileAnonymousLinkType != FileAnonymousLinkType.Value)
                    {
                        if (Tenant.SharingCapability != SharingCapabilities.ExternalUserAndGuestSharing)
                        {
                            WriteWarning(@"Anonymous access links aren’t enabled for your organization. You must first enable them by running the command ""Set-PnPTenant -SharingCapability ExternalUserAndGuestSharing"" before you can set the FileAnonymousLinkType property. We will not set the value in this case.");
                        }
                        else
                        {
                            Tenant.FileAnonymousLinkType = FileAnonymousLinkType.Value;
                        }
                    }
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property FileAnonymousLinkType is not supported by your version of the service");
                }
            }
            if (FolderAnonymousLinkType.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.FolderAnonymousLinkType);
                    if (Tenant.FolderAnonymousLinkType != FolderAnonymousLinkType.Value)
                    {
                        if (Tenant.SharingCapability != SharingCapabilities.ExternalUserAndGuestSharing)
                        {
                            WriteWarning(@"Anonymous access links aren’t enabled for your organization. You must first enable them by running the command ""Set-PnPTenant -SharingCapability ExternalUserAndGuestSharing"" before you can set the FolderAnonymousLinkType property. We will not set the value in this case.");
                        }
                        else
                        {
                            Tenant.FolderAnonymousLinkType = FolderAnonymousLinkType.Value;
                        }
                    }
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property FolderAnonymousLinkType is not supported by your version of the service");
                }
            }
            if (CommentsOnSitePagesDisabled.HasValue)
            {
                try
                {
                    Tenant.CommentsOnSitePagesDisabled = CommentsOnSitePagesDisabled.Value;
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property CommentsOnSitePagesDisabled is not supported by your version of the service");
                }
            }
            if (SocialBarOnSitePagesDisabled.HasValue)
            {
                try
                {
                    Tenant.SocialBarOnSitePagesDisabled = SocialBarOnSitePagesDisabled.Value;
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property SocialBarOnSitePagesDisabled is not supported by your version of the service");
                }
            }
            if (DefaultLinkPermission.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.DefaultLinkPermission);
                    if (Tenant.DefaultLinkPermission != DefaultLinkPermission.Value)
                    {
                        Tenant.DefaultLinkPermission = DefaultLinkPermission.Value;
                    }
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property DefaultLinkPermission is not supported by your version of the service");
                }
            }
            if (ConditionalAccessPolicy.HasValue)
            {
                try
                {
                    Tenant.ConditionalAccessPolicy = ConditionalAccessPolicy.Value;
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property ConditionalAccessPolicy is not supported by your version of the service");
                }
            }
            if (AllowDownloadingNonWebViewableFiles.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.AllowDownloadingNonWebViewableFiles);
                    if (Tenant.ConditionalAccessPolicy == SPOConditionalAccessPolicyType.AllowLimitedAccess)
                    {
                        Tenant.AllowDownloadingNonWebViewableFiles = AllowDownloadingNonWebViewableFiles.Value;
                        isDirty = true;
                        if (!AllowDownloadingNonWebViewableFiles.Value)
                        {
                            WriteWarning("Users will not be able to download files that can't be viewed on the web. To allow download of files that can't be viewed on the web, run the cmdlet again and set AllowDownloadingNonWebViewableFiles to true.");
                        }
                    }
                    else if (ShouldContinue("To set this parameter, you need to set the Set-PnPTenant -ConditionalAccessPolicy to AllowLimitedAccess. Would you like to set it now?", "Confirm"))
                    {
                        Tenant.ConditionalAccessPolicy = SPOConditionalAccessPolicyType.AllowLimitedAccess;
                        Tenant.AllowDownloadingNonWebViewableFiles = AllowDownloadingNonWebViewableFiles.Value;
                        isDirty = true;
                        if (!AllowDownloadingNonWebViewableFiles.Value)
                        {
                            WriteWarning("Users will not be able to download files that can't be viewed on the web. To allow download of files that can't be viewed on the web, run the cmdlet again and set AllowDownloadingNonWebViewableFiles to true.");
                        }
                    }
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property AllowDownloadingNonWebViewableFiles is not supported by your version of the service");
                }
            }
            if (AllowEditing.HasValue)
            {
                try
                {
                    Tenant.EnsureProperty(t => t.ConditionalAccessPolicy);
                    if (Tenant.ConditionalAccessPolicy == SPOConditionalAccessPolicyType.AllowLimitedAccess)
                    {
                        Tenant.AllowEditing = AllowEditing.Value;
                        isDirty = true;
                    }
                    else if (ShouldContinue("To set this parameter, you need to set the Set-SPOTenant -ConditionalAccessPolicy to AllowLimitedAccess. Would you like to set it now?", "Confirm"))
                    {
                        Tenant.ConditionalAccessPolicy = SPOConditionalAccessPolicyType.AllowLimitedAccess;
                        Tenant.AllowEditing = AllowEditing.Value;
                        isDirty = true;
                    }
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property AllowEditing is not supported by your version of the service");
                }
            }
            if (ApplyAppEnforcedRestrictionsToAdHocRecipients.HasValue)
            {
                try
                {
                    Tenant.ApplyAppEnforcedRestrictionsToAdHocRecipients = ApplyAppEnforcedRestrictionsToAdHocRecipients.Value;
                    isDirty = true;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property ApplyAppEnforcedRestrictionsToAdHocRecipients is not supported by your version of the service");
                }
            }
            if (FilePickerExternalImageSearchEnabled.HasValue)
            {
                try
                {
                    Tenant.FilePickerExternalImageSearchEnabled = FilePickerExternalImageSearchEnabled.Value;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property FilePickerExternalImageSearchEnabled is not supported by your version of the service");
                }
                isDirty = true;
            }
            if (EmailAttestationRequired.HasValue)
            {
                try
                {
                    Tenant.EmailAttestationRequired = EmailAttestationRequired.Value;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property EmailAttestationRequired is not supported by your version of the service");
                }
                isDirty = true;
            }
            if (EmailAttestationReAuthDays.HasValue)
            {
                try
                {
                    Tenant.EmailAttestationReAuthDays = EmailAttestationReAuthDays.Value;
                }
                catch (PropertyOrFieldNotInitializedException)
                {
                    throw new InvalidOperationException("Setting the property EmailAttestationReAuthDays is not supported by your version of the service");
                }
                isDirty = true;
            }
            if(HideDefaultThemes.HasValue)
            {
                Tenant.HideDefaultThemes = HideDefaultThemes.Value;
                isDirty = true;
            }
            if (DisabledWebPartIds != null)
            {
                Tenant.DisabledWebPartIds = DisabledWebPartIds;
                isDirty = true;
            }
            if(EnableAIPIntegration.HasValue)
            {
                Tenant.EnableAIPIntegration = EnableAIPIntegration.Value;
                isDirty = true;
            }
            if (isDirty)
            {
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif