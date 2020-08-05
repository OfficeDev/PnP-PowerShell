using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class RemoveSiteTests
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
        public void RemovePnPTenantSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the full URL of the site collection that needs to be deleted
				var url = "";
				// From Cmdlet Help: Do not add to the tenant scoped recycle bin when selected.
				var skipRecycleBin = "";
				// From Cmdlet Help: OBSOLETE: If true, will wait for the site to be deleted before processing continues
				var wait = "";
				// From Cmdlet Help: If specified, will search for the site in the Recycle Bin and remove it from there.
				var fromRecycleBin = "";
				// From Cmdlet Help: Do not ask for confirmation.
				var force = "";

                var results = scope.ExecuteCommand("Remove-PnPTenantSite",
					new CommandParameter("Url", url),
					new CommandParameter("SkipRecycleBin", skipRecycleBin),
					new CommandParameter("Wait", wait),
					new CommandParameter("FromRecycleBin", fromRecycleBin),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            