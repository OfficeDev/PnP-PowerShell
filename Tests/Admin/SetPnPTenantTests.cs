using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class SetTenantTests
    {
        #region Test Setup/CleanUp
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // This runs on class level once before all tests run
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [ClassCleanup]
        public static void Cleanup(TestContext testContext)
        {
            // This runs on class level once
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                // Example
                // scope.ExecuteCommand("cmdlet", new CommandParameter("param1", prop));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    // Do Test Setup - Note, this runs PER test
                }
                catch (Exception)
                {
                    // Describe Exception
                }
            }
        }
        #endregion

        #region Scaffolded Cmdlet Tests
        //TODO: This is a scaffold of the cmdlet - complete the unit test
        //[TestMethod]
        public void SetPnPTenantTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Permits the use of special characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.Note:
				// The only two characters that can be managed at this time are the # and % characters.The following are the valid values:NoPreference- Support for feature will be enabled by Microsoft on your Office 365 tenant.Allowed- Lets the # and % characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.Disallowed- Disallows the # and % characters in file and folder names in SharePoint Online and OneDrive for Business document libraries.
				var specialCharactersStateInFileFolderNames = "";
				// From Cmdlet Help: Specifies the lower bound on the compatibility level for new sites.
				var minCompatibilityLevel = "";
				// From Cmdlet Help: Specifies the upper bound on the compatibility level for new sites.
				var maxCompatibilityLevel = "";
				// From Cmdlet Help: Enables external services for a tenant.
				// External services are defined as services that are not in the Office 365 datacenters.The valid values are:
				// True (default) - External services are enabled for the tenant.
				// False - External services that are outside of the Office 365 datacenters cannot interact with SharePoint.
				var externalServicesEnabled = "";
				// From Cmdlet Help: Specifies the URL of the redirected site for those site collections which have the locked state "NoAccess"The valid values are:
				// ""(default) - Blank by default, this will also remove or clear any value that has been set.
				// Full URL - Example: https://contoso.sharepoint.com/Pages/Locked.aspx
				var noAccessRedirectUrl = "";
				// From Cmdlet Help: Determines what level of sharing is available for the site.The valid values are:
				// ExternalUserAndGuestSharing (default) - External user sharing (share by email) and guest link sharing are both enabled. Disabled - External user sharing (share by email) and guest link sharing are both disabled.
				// ExternalUserSharingOnly - External user sharing (share by email) is enabled, but guest link sharing is disabled.For more information about sharing, see Manage external sharing for your SharePoint online environment (http://office.microsoft.com/en-us/office365-sharepoint-online-enterprise-help/manage-external-sharing-for-your-sharepoint-online-environment-HA102849864.aspx).
				var sharingCapability = "";
				// From Cmdlet Help: Determines whether tenant users see the Start a Site menu option.The valid values are:
				// True (default) - Tenant users will see the Start a Site menu option.
				// False - Start a Site is hidden from the menu.
				var displayStartASiteOption = "";
				// From Cmdlet Help: Specifies URL of the form to load in the Start a Site dialog.The valid values are:
				// "" (default) - Blank by default, this will also remove or clear any value that has been set.
				// Full URL - Example: "https://contoso.sharepoint.com/path/to/form"
				var startASiteFormUrl = "";
				// From Cmdlet Help: Enables the administrator to hide the Everyone claim in the People Picker.
				// When users share an item with Everyone, it is accessible to all authenticated users in the tenant's Azure Active Directory, including any active external users who have previously accepted invitations.Note, that some SharePoint system resources such as templates and pages are required to be shared to Everyone and this type of sharing does not expose any user data or metadata.The valid values are:
				// True (default) - The Everyone claim group is displayed in People Picker.
				// False - The Everyone claim group is hidden from the People Picker.
				var showEveryoneClaim = "";
				// From Cmdlet Help: Enables the administrator to hide the All Users claim groups in People Picker.When users share an item with "All Users (x)", it is accessible to all organization members in the tenant's Azure Active Directory who have authenticated with via this method. When users share an item with "All Users (x)" it is accessible to all organization members in the tenant that used NTLM to authentication with SharePoint.Note, the All Users(authenticated) group is equivalent to the Everyone claim, and shows as Everyone.To change this, see - ShowEveryoneClaim.The valid values are:
				// True(default) - The All Users claim groups are displayed in People Picker.
				// False - The All Users claim groups are hidden in People Picker.
				var showAllUsersClaim = "";
				// From Cmdlet Help: Enables the administrator to hide the "Everyone except external users" claim in the People Picker.
				// When users share an item with "Everyone except external users", it is accessible to all organization members in the tenant's Azure Active Directory, but not to any users who have previously accepted invitations.The valid values are:
				// True(default) - The Everyone except external users is displayed in People Picker.
				// False - The Everyone except external users claim is not visible in People Picker.
				var showEveryoneExceptExternalUsersClaim = "";
				// From Cmdlet Help: Removes the search capability from People Picker. Note, recently resolved names will still appear in the list until browser cache is cleared or expired.SharePoint Administrators will still be able to use starts with or partial name matching when enabled.The valid values are:
				// False (default) - Starts with / partial name search functionality is available.
				// True - Disables starts with / partial name search functionality for all SharePoint users, except SharePoint Admins.
				var searchResolveExactEmailOrUPN = "";
				// From Cmdlet Help: When set to true this will disable the ability to use Modern Authentication that leverages ADAL across the tenant.The valid values are:
				// False (default) - Modern Authentication is enabled/allowed.
				// True - Modern Authentication via ADAL is disabled.
				var officeClientADALDisabled = "";
				// From Cmdlet Help: By default this value is set to $true.Setting this parameter prevents Office clients using non-modern authentication protocols from accessing SharePoint Online resources.A value of $true - Enables Office clients using non-modern authentication protocols(such as, Forms-Based Authentication (FBA) or Identity Client Runtime Library (IDCRL)) to access SharePoint resources.A value of $false - Prevents Office clients using non-modern authentication protocols from accessing SharePoint Online resources.Note:
				// This may also prevent third-party apps from accessing SharePoint Online resources.Also, this will also block apps using the SharePointOnlineCredentials class to access SharePoint Online resources.For additional information about SharePointOnlineCredentials, see SharePointOnlineCredentials class.
				var legacyAuthProtocolsEnabled = "";
				// From Cmdlet Help: Ensures that an external user can only accept an external sharing invitation with an account matching the invited email address.Administrators who desire increased control over external collaborators should consider enabling this feature.Note, this only applies to new external users accepting new sharing invitations. Also, the resource owner must share with an organizational or Microsoft account or the external user will be unable to access the resource.The valid values are:
				// False (default) - When a document is shared with an external user, bob@contoso.com, it can be accepted by any user with access to the invitation link in the original e-mail.
				// True - User must accept this invitation with bob@contoso.com.
				var requireAcceptingAccountMatchInvitedAccount = "";
				// From Cmdlet Help: Creates a Shared with Everyone folder in every user's new OneDrive for Business document library.The valid values are:
				// True (default) - The Shared with Everyone folder is created.
				// False - No folder is created when the site and OneDrive for Business document library is created.The default behavior of the Shared with Everyone folder changed in August 2015.
				// For additional information about the change, see Provision the Shared with Everyone folder in OneDrive for Business (https://support.office.com/en-us/article/Provision-the-Shared-with-Everyone-folder-in-OneDrive-for-Business-6bb02c91-fd0b-42ba-9457-3921cb6dc5b2?ui=en-US&rs=en-US&ad=US)
				var provisionSharedWithEveryoneFolder = "";
				// From Cmdlet Help: Specifies the home realm discovery value to be sent to Azure Active Directory (AAD) during the user sign-in process.When the organization uses a third-party identity provider, this prevents the user from seeing the Azure Active Directory Home Realm Discovery web page and ensures the user only sees their company's Identity Provider's portal.
				// This value can also be used with Azure Active Directory Premium to customize the Azure Active Directory login page.Acceleration will not occur on site collections that are shared externally.This value should be configured with the login domain that is used by your company (that is, example@contoso.com).If your company has multiple third-party identity providers, configuring the sign-in acceleration value will break sign-in for your organization.The valid values are:
				// "" (default) - Blank by default, this will also remove or clear any value that has been set.
				// Login Domain - For example: "contoso.com"
				var signInAccelerationDomain = "";
				// From Cmdlet Help: Accelerates guest-enabled site collections as well as member-only site collections when the SignInAccelerationDomain parameter is set.Note:
				// If enabled, your identity provider must be capable of authenticating guest users. If it is not, guest users will be unable to log in and access content that was shared with them.
				var enableGuestSignInAcceleration = "";
				// From Cmdlet Help: Lets SharePoint issue a special cookie that will allow this feature to work even when "Keep Me Signed In" is not selected."Open with Explorer" requires persisted cookies to operate correctly.
				// When the user does not select "Keep Me Signed in" at the time of sign -in, "Open with Explorer" will fail.This special cookie expires after 30 minutes and cannot be cleared by closing the browser or signing out of SharePoint Online.To clear this cookie, the user must log out of their Windows session.The valid values are:
				// False(default) - No special cookie is generated and the normal Office 365 sign -in length / timing applies.
				// True - Generates a special cookie that will allow "Open with Explorer" to function if the "Keep Me Signed In" box is not checked at sign -in.
				var usePersistentCookiesForExplorerView = "";
				// From Cmdlet Help: When the feature is enabled, all external sharing invitations that are sent will blind copy the e-mail messages listed in the BccExternalSharingInvitationsList.The valid values are:
				// False (default) - BCC for external sharing is disabled.
				// True - All external sharing invitations that are sent will blind copy the e-mail messages listed in the BccExternalSharingInvitationsList.
				var bccExternalSharingInvitations = "";
				// From Cmdlet Help: Specifies a list of e-mail addresses to be BCC'd when the BCC for External Sharing feature is enabled.
				// Multiple addresses can be specified by creating a comma separated list with no spaces.The valid values are:
				// "" (default) - Blank by default, this will also clear any value that has been set.
				// Single or Multiple e-mail addresses - joe@contoso.com or joe@contoso.com,bob@contoso.com
				var bccExternalSharingInvitationsList = "";
				var userVoiceForFeedbackEnabled = "";
				var publicVarCdnEnabled = "";
				var publicVarCdnAllowedFileTypes = "";
				// From Cmdlet Help: Specifies all anonymous links that have been created (or will be created) will expire after the set number of days .To remove the expiration requirement, set the value to zero (0).
				var requireAnonymousLinksExpireInDays = "";
				// From Cmdlet Help: Specifies a list of email domains that is allowed for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, "contoso.com fabrikam.com".For additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business
				var sharingAllowedDomainList = "";
				// From Cmdlet Help: Specifies a list of email domains that is blocked or prohibited for sharing with the external collaborators. Use space character as the delimiter for entering multiple values. For example, "contoso.com fabrikam.com".For additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business
				var sharingBlockedDomainList = "";
				// From Cmdlet Help: Specifies the external sharing mode for domains.The following values are: None AllowList BlockListFor additional information about how to restrict a domain sharing, see Restricted Domains Sharing in Office 365 SharePoint Online and OneDrive for Business.
				var sharingDomainRestrictionMode = "";
				// From Cmdlet Help: Sets a default OneDrive for Business storage quota for the tenant. It will be used for new OneDrive for Business sites created.A typical use will be to reduce the amount of storage associated with OneDrive for Business to a level below what the License entitles the users. For example, it could be used to set the quota to 10 gigabytes (GB) by default.If value is set to 0, the parameter will have no effect.If the value is set larger than the Maximum allowed OneDrive for Business quota, it will have no effect.
				var oneDriveStorageQuota = "";
				// From Cmdlet Help: Lets OneDrive for Business creation for administrator managed guest users. Administrator managed Guest users use credentials in the resource tenant to access the resources.The valid values are the following:$true-Administrator managed Guest users can be given OneDrives, provided needed licenses are assigned.$false- Administrator managed Guest users can't be given OneDrives as functionality is turned off.
				var oneDriveForGuestsEnabled = "";
				// From Cmdlet Help: Allows access from network locations that are defined by an administrator.The values are $true and $false. The default value is $false which means the setting is disabled.Before the IPAddressEnforcement parameter is set, make sure you add a valid IPv4 or IPv6 address to the IPAddressAllowList parameter.
				var iPAddressEnforcement = "";
				// From Cmdlet Help: Configures multiple IP addresses or IP address ranges (IPv4 or IPv6).Use commas to separate multiple IP addresses or IP address ranges. Verify there are no overlapping IP addresses and ensure IP ranges use Classless Inter-Domain Routing (CIDR) notation. For example, 172.16.0.0, 192.168.1.0/27.Note:
				// The IPAddressAllowList parameter only lets administrators set IP addresses or ranges that are recognized as trusted. To only grant access from these IP addresses or ranges, set the IPAddressEnforcement parameter to $true.
				var iPAddressAllowList = "";
				var iPAddressWACTokenLifetime = "";
				// From Cmdlet Help: Note:
				// When set to $true, users aren't able to share with security groups or SharePoint groups.
				var useFindPeopleInPeoplePicker = "";
				// From Cmdlet Help: Lets administrators choose what type of link appears is selected in the “Get a link” sharing dialog box in OneDrive for Business and SharePoint Online.For additional information about how to change the default link type, see Change the default link type when users get links for sharing.Note:
				// Setting this value to “none” will default “get a link” to the most permissive link available (that is, if anonymous links are enabled, the default link will be anonymous access; if they are disabled then the default link will be internal.The values are: None Direct Internal AnonymousAccess
				var defaultSharingLinkType = "";
				// From Cmdlet Help: Lets administrators set policy on re-sharing behavior in OneDrive for Business.Values:On- Users with edit permissions can re-share.Off- Only OneDrive for Business owner can share. The value of ODBAccessRequests defines whether a request to share gets sent to the owner.Unspecified- Let each OneDrive for Business owner enable or disable re-sharing behavior on their OneDrive.
				var oDBMembersCanShare = "";
				// From Cmdlet Help: Lets administrators set policy on access requests and requests to share in OneDrive for Business.Values:On- Users without permission to share can trigger sharing requests to the OneDrive for Business owner when they attempt to share. Also, users without permission to a file or folder can trigger access requests to the OneDrive for Business owner when they attempt to access an item they do not have permissions to.Off- Prevent access requests and requests to share on OneDrive for Business.Unspecified- Let each OneDrive for Business owner enable or disable access requests and requests to share on their OneDrive.
				var oDBAccessRequests = "";
				var preventExternalUsersFromResharing = "";
				var showPeoplePickerSuggestionsForGuestUsers = "";
				var fileAnonymousLinkType = "";
				var folderAnonymousLinkType = "";
				// From Cmdlet Help: When this parameter is set to $true and another user re-shares a document from a user’s OneDrive for Business, the OneDrive for Business owner is notified by e-mail.For additional information about how to configure notifications for external sharing, see Configure notifications for external sharing for OneDrive for Business.The values are $true and $false.
				var notifyOwnersWhenItemsReshared = "";
				// From Cmdlet Help: When this parameter is set to $true and when an external user accepts an invitation to a resource in a user’s OneDrive for Business, the OneDrive for Business owner is notified by e-mail.For additional information about how to configure notifications for external sharing, see Configure notifications for external sharing for OneDrive for Business.The values are $true and $false.
				var notifyOwnersWhenInvitationsAccepted = "";
				var notificationsInOneDriveForBusinessEnabled = "";
				var notificationsInSharePointEnabled = "";
				var ownerAnonymousNotification = "";
				var commentsOnSitePagesDisabled = "";
				var socialBarOnSitePagesDisabled = "";
				// From Cmdlet Help: Specifies the number of days after a user's Active Directory account is deleted that their OneDrive for Business content will be deleted.The value range is in days, between 30 and 3650. The default value is 30.
				var orphanedPersonalSitesRetentionPeriod = "";
				// From Cmdlet Help: Prevents the Download button from being displayed on the Virus Found warning page.Accepts a value of true (enabled) to hide the Download button or false (disabled) to display the Download button. By default this feature is set to false.
				var disallowInfectedFileDownload = "";
				var defaultLinkPermission = "";
				var conditionalAccessPolicy = "";
				var allowDownloadingNonWebViewableFiles = "";
				var allowEditing = "";
				var applyAppEnforcedRestrictionsToAdHocRecipients = "";
				var filePickerExternalImageSearchEnabled = "";
				var emailAttestationRequired = "";
				var emailAttestationReAuthDays = "";
				// From Cmdlet Help: Defines if the default themes are visible or hidden
				var hideDefaultThemes = "";
				// From Cmdlet Help: Guids of out of the box modern web part id's to hide
				var disabledWebPartIds = "";
				// From Cmdlet Help: Boolean indicating if Azure Information Protection (AIP) should be enabled on the tenant. For more information, see https://docs.microsoft.com/microsoft-365/compliance/sensitivity-labels-sharepoint-onedrive-files#use-powershell-to-enable-support-for-sensitivity-labels
				var enableAIPIntegration = "";

                var results = scope.ExecuteCommand("Set-PnPTenant",
					new CommandParameter("SpecialCharactersStateInFileFolderNames", specialCharactersStateInFileFolderNames),
					new CommandParameter("MinCompatibilityLevel", minCompatibilityLevel),
					new CommandParameter("MaxCompatibilityLevel", maxCompatibilityLevel),
					new CommandParameter("ExternalServicesEnabled", externalServicesEnabled),
					new CommandParameter("NoAccessRedirectUrl", noAccessRedirectUrl),
					new CommandParameter("SharingCapability", sharingCapability),
					new CommandParameter("DisplayStartASiteOption", displayStartASiteOption),
					new CommandParameter("StartASiteFormUrl", startASiteFormUrl),
					new CommandParameter("ShowEveryoneClaim", showEveryoneClaim),
					new CommandParameter("ShowAllUsersClaim", showAllUsersClaim),
					new CommandParameter("ShowEveryoneExceptExternalUsersClaim", showEveryoneExceptExternalUsersClaim),
					new CommandParameter("SearchResolveExactEmailOrUPN", searchResolveExactEmailOrUPN),
					new CommandParameter("OfficeClientADALDisabled", officeClientADALDisabled),
					new CommandParameter("LegacyAuthProtocolsEnabled", legacyAuthProtocolsEnabled),
					new CommandParameter("RequireAcceptingAccountMatchInvitedAccount", requireAcceptingAccountMatchInvitedAccount),
					new CommandParameter("ProvisionSharedWithEveryoneFolder", provisionSharedWithEveryoneFolder),
					new CommandParameter("SignInAccelerationDomain", signInAccelerationDomain),
					new CommandParameter("EnableGuestSignInAcceleration", enableGuestSignInAcceleration),
					new CommandParameter("UsePersistentCookiesForExplorerView", usePersistentCookiesForExplorerView),
					new CommandParameter("BccExternalSharingInvitations", bccExternalSharingInvitations),
					new CommandParameter("BccExternalSharingInvitationsList", bccExternalSharingInvitationsList),
					new CommandParameter("UserVoiceForFeedbackEnabled", userVoiceForFeedbackEnabled),
					new CommandParameter("PublicCdnEnabled", publicVarCdnEnabled),
					new CommandParameter("PublicCdnAllowedFileTypes", publicVarCdnAllowedFileTypes),
					new CommandParameter("RequireAnonymousLinksExpireInDays", requireAnonymousLinksExpireInDays),
					new CommandParameter("SharingAllowedDomainList", sharingAllowedDomainList),
					new CommandParameter("SharingBlockedDomainList", sharingBlockedDomainList),
					new CommandParameter("SharingDomainRestrictionMode", sharingDomainRestrictionMode),
					new CommandParameter("OneDriveStorageQuota", oneDriveStorageQuota),
					new CommandParameter("OneDriveForGuestsEnabled", oneDriveForGuestsEnabled),
					new CommandParameter("IPAddressEnforcement", iPAddressEnforcement),
					new CommandParameter("IPAddressAllowList", iPAddressAllowList),
					new CommandParameter("IPAddressWACTokenLifetime", iPAddressWACTokenLifetime),
					new CommandParameter("UseFindPeopleInPeoplePicker", useFindPeopleInPeoplePicker),
					new CommandParameter("DefaultSharingLinkType", defaultSharingLinkType),
					new CommandParameter("ODBMembersCanShare", oDBMembersCanShare),
					new CommandParameter("ODBAccessRequests", oDBAccessRequests),
					new CommandParameter("PreventExternalUsersFromResharing", preventExternalUsersFromResharing),
					new CommandParameter("ShowPeoplePickerSuggestionsForGuestUsers", showPeoplePickerSuggestionsForGuestUsers),
					new CommandParameter("FileAnonymousLinkType", fileAnonymousLinkType),
					new CommandParameter("FolderAnonymousLinkType", folderAnonymousLinkType),
					new CommandParameter("NotifyOwnersWhenItemsReshared", notifyOwnersWhenItemsReshared),
					new CommandParameter("NotifyOwnersWhenInvitationsAccepted", notifyOwnersWhenInvitationsAccepted),
					new CommandParameter("NotificationsInOneDriveForBusinessEnabled", notificationsInOneDriveForBusinessEnabled),
					new CommandParameter("NotificationsInSharePointEnabled", notificationsInSharePointEnabled),
					new CommandParameter("OwnerAnonymousNotification", ownerAnonymousNotification),
					new CommandParameter("CommentsOnSitePagesDisabled", commentsOnSitePagesDisabled),
					new CommandParameter("SocialBarOnSitePagesDisabled", socialBarOnSitePagesDisabled),
					new CommandParameter("OrphanedPersonalSitesRetentionPeriod", orphanedPersonalSitesRetentionPeriod),
					new CommandParameter("DisallowInfectedFileDownload", disallowInfectedFileDownload),
					new CommandParameter("DefaultLinkPermission", defaultLinkPermission),
					new CommandParameter("ConditionalAccessPolicy", conditionalAccessPolicy),
					new CommandParameter("AllowDownloadingNonWebViewableFiles", allowDownloadingNonWebViewableFiles),
					new CommandParameter("AllowEditing", allowEditing),
					new CommandParameter("ApplyAppEnforcedRestrictionsToAdHocRecipients", applyAppEnforcedRestrictionsToAdHocRecipients),
					new CommandParameter("FilePickerExternalImageSearchEnabled", filePickerExternalImageSearchEnabled),
					new CommandParameter("EmailAttestationRequired", emailAttestationRequired),
					new CommandParameter("EmailAttestationReAuthDays", emailAttestationReAuthDays),
					new CommandParameter("HideDefaultThemes", hideDefaultThemes),
					new CommandParameter("DisabledWebPartIds", disabledWebPartIds),
					new CommandParameter("EnableAIPIntegration", enableAIPIntegration));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            