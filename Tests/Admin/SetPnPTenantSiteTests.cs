using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class SetTenantSiteTests
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
        public void SetPnPTenantSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the URL of the site
				var url = "";
				// From Cmdlet Help: Specifies the title of the site
				var title = "";
				// From Cmdlet Help: Specifies the language of this site collection. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: (Get-PnPWeb -Includes RegionalSettings.InstalledLanguages).RegionalSettings.InstalledLanguages.
				var localeId = "";
				// From Cmdlet Help: Specifies if the site administrator can upgrade the site collection
				var allowSelfServiceUpgrade = "";
				// From Cmdlet Help: Specifies owner(s) to add as site collection administrators. They will be added as additional site collection administrators. Existing administrators will stay. Can be both users and groups.
				var owners = "";
				// From Cmdlet Help: Determines whether the Add And Customize Pages right is denied on the site collection. For more information about permission levels, see User permissions and permission levels in SharePoint.
				var denyAddAndCustomizePages = "";
				// From Cmdlet Help: Specifies what the sharing capabilities are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly
				var sharingCapability = "";
				// From Cmdlet Help: Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.
				var storageMaximumLevel = "";
				// From Cmdlet Help: Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter
				var storageWarningLevel = "";
				// From Cmdlet Help: Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.
				var userCodeMaximumLevel = "";
				// From Cmdlet Help: Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter
				var userCodeWarningLevel = "";
				// From Cmdlet Help: Sets the lockstate of a site
				var lockState = "";
				// From Cmdlet Help: Specifies the default link permission for the site collection. None - Respect the organization default link permission. View - Sets the default link permission for the site to "view" permissions. Edit - Sets the default link permission for the site to "edit" permissions
				var defaultLinkPermission = "";
				// From Cmdlet Help: Specifies the default link type for the site collection. None - Respect the organization default sharing link type. AnonymousAccess - Sets the default sharing link for this site to an Anonymous Access or Anyone link. Internal - Sets the default sharing link for this site to the "organization" link or company shareable link. Direct - Sets the default sharing link for this site to the "Specific people" link
				var defaultSharingLinkType = "";
				// From Cmdlet Help: Specifies a list of email domains that is allowed for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, "contoso.com fabrikam.com".
				var sharingAllowedDomainList = "";
				// From Cmdlet Help: Specifies a list of email domains that is blocked for sharing with the external collaborators. Use the space character as the delimiter for entering multiple values. For example, "contoso.com fabrikam.com".
				var sharingBlockedDomainList = "";
				// From Cmdlet Help: Specifies if non web viewable files can be downloaded.
				var blockDownloadOfNonViewableFiles = "";
				// From Cmdlet Help: Specifies the external sharing mode for domains.
				var sharingDomainRestrictionMode = "";
				// From Cmdlet Help: Specifies if comments on site pages are enabled
				var commentsOnSitePagesDisabled = "";
				// From Cmdlet Help: -
				var disableAppViews = "";
				// From Cmdlet Help: -
				var disableCompanyWideSharingLinks = "";
				// From Cmdlet Help: -
				var disableFlows = "";
				// From Cmdlet Help: Wait for the operation to complete
				var wait = "";

                var results = scope.ExecuteCommand("Set-PnPTenantSite",
					new CommandParameter("Url", url),
					new CommandParameter("Title", title),
					new CommandParameter("LocaleId", localeId),
					new CommandParameter("AllowSelfServiceUpgrade", allowSelfServiceUpgrade),
					new CommandParameter("Owners", owners),
					new CommandParameter("DenyAddAndCustomizePages", denyAddAndCustomizePages),
					new CommandParameter("SharingCapability", sharingCapability),
					new CommandParameter("StorageMaximumLevel", storageMaximumLevel),
					new CommandParameter("StorageWarningLevel", storageWarningLevel),
					new CommandParameter("UserCodeMaximumLevel", userCodeMaximumLevel),
					new CommandParameter("UserCodeWarningLevel", userCodeWarningLevel),
					new CommandParameter("LockState", lockState),
					new CommandParameter("DefaultLinkPermission", defaultLinkPermission),
					new CommandParameter("DefaultSharingLinkType", defaultSharingLinkType),
					new CommandParameter("SharingAllowedDomainList", sharingAllowedDomainList),
					new CommandParameter("SharingBlockedDomainList", sharingBlockedDomainList),
					new CommandParameter("BlockDownloadOfNonViewableFiles", blockDownloadOfNonViewableFiles),
					new CommandParameter("SharingDomainRestrictionMode", sharingDomainRestrictionMode),
					new CommandParameter("CommentsOnSitePagesDisabled", commentsOnSitePagesDisabled),
					new CommandParameter("DisableAppViews", disableAppViews),
					new CommandParameter("DisableCompanyWideSharingLinks", disableCompanyWideSharingLinks),
					new CommandParameter("DisableFlows", disableFlows),
					new CommandParameter("Wait", wait));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            