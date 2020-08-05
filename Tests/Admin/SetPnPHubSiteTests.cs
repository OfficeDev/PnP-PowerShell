using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class SetHubSiteTests
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
        public void SetPnPHubSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Id or Url of a hub site to configure
				var identity = "";
				// From Cmdlet Help: The title to set on the hub which will be shown in the hub navigation bar
				var title = "";
				// From Cmdlet Help: Full url to the image to use for the hub site logo. Can either be a logo hosted on SharePoint or outside of SharePoint and must be an absolute URL to the image.
				var logoUrl = "";
				// From Cmdlet Help: The description of the hub site
				var description = "";
				// From Cmdlet Help: GUID of the SharePoint Site Design which should be applied when a site joins the hub site
				var siteDesignId = "";
				var hideNameInNavigation = "";
				var requiresJoinApproval = "";

                var results = scope.ExecuteCommand("Set-PnPHubSite",
					new CommandParameter("Identity", identity),
					new CommandParameter("Title", title),
					new CommandParameter("LogoUrl", logoUrl),
					new CommandParameter("Description", description),
					new CommandParameter("SiteDesignId", siteDesignId),
					new CommandParameter("HideNameInNavigation", hideNameInNavigation),
					new CommandParameter("RequiresJoinApproval", requiresJoinApproval));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            