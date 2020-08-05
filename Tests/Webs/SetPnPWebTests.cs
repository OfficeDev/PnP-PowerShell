using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webs
{
    [TestClass]
    public class SetWebTests
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
        public void SetPnPWebTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Sets the logo of the web to the current url. If you want to set the logo to a modern team site, use Set-PnPSite -LogoFilePath.
				var siteLogoUrl = "";
				// From Cmdlet Help: Sets the AlternateCssUrl of the web. Only works for classic pages.
				var alternateCssUrl = "";
				// From Cmdlet Help: Sets the title of the web
				var title = "";
				// From Cmdlet Help: Sets the description of the web
				var description = "";
				// From Cmdlet Help: Sets the MasterUrl of the web. Only works for classic pages.
				var masterUrl = "";
				// From Cmdlet Help: Sets the CustomMasterUrl of the web. Only works for classic pages.
				var customMasterUrl = "";
				// From Cmdlet Help: Defines if the quick launch menu on the left side of modern Team Sites should be shown ($true) or hidden ($false)
				var quickLaunchEnabled = "";
				// From Cmdlet Help: Indicates if members of this site can share the site and individual sites with others ($true) or only owners can do this ($false)
				var membersCanShare = "";
				// From Cmdlet Help: Indicates if this site should not be returned in search results ($true) or if it should be ($false)
				var noCrawl = "";
				var headerLayoutVar = "";
				var headerEmphasis = "";
				// From Cmdlet Help: Defines if the navigation menu on a modern site should be enabled for modern audience targeting ($true) or not ($false)
				var navAudienceTargetingEnabled = "";
				// From Cmdlet Help: Defines if the navigation menu should be shown as the mega menu ($true) or the smaller sized menu ($false)
				var megaMenuEnabled = "";
				// From Cmdlet Help: Defines if Power Automate should be available on lists and document libraries ($false) or if the option should be hidden ($true)
				var disablePowerAutomate = "";
				// From Cmdlet Help: Defines if comments on modern site pages should be enabled by default ($false) or they should be hidden ($true)
				var commentsOnSitePagesDisabled = "";

                var results = scope.ExecuteCommand("Set-PnPWeb",
					new CommandParameter("SiteLogoUrl", siteLogoUrl),
					new CommandParameter("AlternateCssUrl", alternateCssUrl),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("MasterUrl", masterUrl),
					new CommandParameter("CustomMasterUrl", customMasterUrl),
					new CommandParameter("QuickLaunchEnabled", quickLaunchEnabled),
					new CommandParameter("MembersCanShare", membersCanShare),
					new CommandParameter("NoCrawl", noCrawl),
					new CommandParameter("HeaderLayout", headerLayoutVar),
					new CommandParameter("HeaderEmphasis", headerEmphasis),
					new CommandParameter("NavAudienceTargetingEnabled", navAudienceTargetingEnabled),
					new CommandParameter("MegaMenuEnabled", megaMenuEnabled),
					new CommandParameter("DisablePowerAutomate", disablePowerAutomate),
					new CommandParameter("CommentsOnSitePagesDisabled", commentsOnSitePagesDisabled));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            