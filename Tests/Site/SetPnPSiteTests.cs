using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Site
{
    [TestClass]
    public class SetSiteTests
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
        public void SetPnPSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				var identity = "";
				// From Cmdlet Help: The classification to set
				var classification = "";
				// From Cmdlet Help: Disables Microsoft Flow for this site
				var disableFlows = "";
				// From Cmdlet Help: Sets the logo of the site if it concerns a modern team site. Provide a full path to a local image file on your disk which you want to use as the site logo. The logo will be uploaded automatically to SharePoint. If you want to set the logo for a classic site, use Set-PnPWeb -SiteLogoUrl.
				var logoFilePath = "";
				// From Cmdlet Help: Specifies what the sharing capabilities are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly
				var sharing = "";
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
				// From Cmdlet Help: Specifies if the site administrator can upgrade the site collection
				var allowSelfServiceUpgrade = "";
				// From Cmdlet Help: Specifies if a site allows custom script or not. See https://support.office.com/en-us/article/Turn-scripting-capabilities-on-or-off-1f2c515f-5d7e-448a-9fd7-835da935584f for more information.
				var noScriptSite = "";
				// From Cmdlet Help: Specifies owner(s) to add as site collection administrators. They will be added as additional site collection administrators. Existing administrators will stay. Can be both users and groups.
				var owners = "";
				// From Cmdlet Help: Specifies if comments on site pages are enabled or disabled
				var commentsOnSitePagesDisabled = "";
				// From Cmdlet Help: Specifies the default link permission for the site collection. None - Respect the organization default link permission. View - Sets the default link permission for the site to "view" permissions. Edit - Sets the default link permission for the site to "edit" permissions
				var defaultLinkPermission = "";
				// From Cmdlet Help: Specifies the default link type for the site collection. None - Respect the organization default sharing link type. AnonymousAccess - Sets the default sharing link for this site to an Anonymous Access or Anyone link. Internal - Sets the default sharing link for this site to the "organization" link or company shareable link. Direct - Sets the default sharing link for this site to the "Specific people" link
				var defaultSharingLinkType = "";
				var disableAppViews = "";
				var disableCompanyWideSharingLinks = "";
				// From Cmdlet Help: Specifies to prevent non-owners from inviting new users to the site
				var disableSharingForNonOwners = "";
				// From Cmdlet Help: Specifies the language of this site collection.
				var localeId = "";
				// From Cmdlet Help: Specifies the new URL for this site collection.
				var newUrl = "";
				// From Cmdlet Help: Specifies the Geo/Region restrictions of this site.
				var restrictedToGeo = "";
				// From Cmdlet Help: Disables or enables the Social Bar for Site Collection.
				var socialBarOnSitePagesDisabled = "";
				// From Cmdlet Help: Wait for the operation to complete
				var wait = "";

                var results = scope.ExecuteCommand("Set-PnPSite",
					new CommandParameter("Identity", identity),
					new CommandParameter("Classification", classification),
					new CommandParameter("DisableFlows", disableFlows),
					new CommandParameter("LogoFilePath", logoFilePath),
					new CommandParameter("Sharing", sharing),
					new CommandParameter("StorageMaximumLevel", storageMaximumLevel),
					new CommandParameter("StorageWarningLevel", storageWarningLevel),
					new CommandParameter("UserCodeMaximumLevel", userCodeMaximumLevel),
					new CommandParameter("UserCodeWarningLevel", userCodeWarningLevel),
					new CommandParameter("LockState", lockState),
					new CommandParameter("AllowSelfServiceUpgrade", allowSelfServiceUpgrade),
					new CommandParameter("NoScriptSite", noScriptSite),
					new CommandParameter("Owners", owners),
					new CommandParameter("CommentsOnSitePagesDisabled", commentsOnSitePagesDisabled),
					new CommandParameter("DefaultLinkPermission", defaultLinkPermission),
					new CommandParameter("DefaultSharingLinkType", defaultSharingLinkType),
					new CommandParameter("DisableAppViews", disableAppViews),
					new CommandParameter("DisableCompanyWideSharingLinks", disableCompanyWideSharingLinks),
					new CommandParameter("DisableSharingForNonOwners", disableSharingForNonOwners),
					new CommandParameter("LocaleId", localeId),
					new CommandParameter("NewUrl", newUrl),
					new CommandParameter("RestrictedToGeo", restrictedToGeo),
					new CommandParameter("SocialBarOnSitePagesDisabled", socialBarOnSitePagesDisabled),
					new CommandParameter("Wait", wait));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            