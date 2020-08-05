using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Apps
{
    [TestClass]
    public class RegisterAppCatalogSiteTests
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
        public void RegisterPnPAppCatalogSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The full url of the app catalog site to be created, e.g. https://yourtenant.sharepoint.com/sites/appcatalog
				var url = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The login account of the user designated to be the admin for the site, e.g. user@domain.com
				var owner = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Use Get-PnPTimeZoneId to retrieve possible timezone values
				var timeZoneId = "";
				// From Cmdlet Help: If specified, and an app catalog is already present, a new app catalog site will be created. If the same URL is used the existing/current app catalog site will be deleted first.
				var force = "";

                var results = scope.ExecuteCommand("Register-PnPAppCatalogSite",
					new CommandParameter("Url", url),
					new CommandParameter("Owner", owner),
					new CommandParameter("TimeZoneId", timeZoneId),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            