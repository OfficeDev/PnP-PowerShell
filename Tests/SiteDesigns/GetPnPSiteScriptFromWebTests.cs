using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.SiteDesigns
{
    [TestClass]
    public class GetSiteScriptFromWebTests
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
        public void GetPnPSiteScriptFromWebTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the URL of the site to generate a Site Script from
				var url = "";
				// From Cmdlet Help: Allows specifying one or more site relative URLs of lists that should be included into the Site Script, i.e. "Shared Documents","List\MyList"
				var lists = "";
				// From Cmdlet Help: If specified will include all supported components into the Site Script
				var includeAll = "";
				// From Cmdlet Help: If specified will include the branding of the site into the Site Script
				var includeBranding = "";
				// From Cmdlet Help: If specified will include navigation links into the Site Script
				var includeLinksToExportedItems = "";
				// From Cmdlet Help: If specified will include the regional settings into the Site Script
				var includeRegionalSettings = "";
				// From Cmdlet Help: If specified will include the external sharing configuration into the Site Script
				var includeSiteExternalSharingCapability = "";
				// From Cmdlet Help: If specified will include the branding of the site into the Site Script
				var includeTheme = "";

                var results = scope.ExecuteCommand("Get-PnPSiteScriptFromWeb",
					new CommandParameter("Url", url),
					new CommandParameter("Lists", lists),
					new CommandParameter("IncludeAll", includeAll),
					new CommandParameter("IncludeBranding", includeBranding),
					new CommandParameter("IncludeLinksToExportedItems", includeLinksToExportedItems),
					new CommandParameter("IncludeRegionalSettings", includeRegionalSettings),
					new CommandParameter("IncludeSiteExternalSharingCapability", includeSiteExternalSharingCapability),
					new CommandParameter("IncludeTheme", includeTheme));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            