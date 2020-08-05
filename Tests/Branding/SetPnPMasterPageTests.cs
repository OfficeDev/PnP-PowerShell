using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class SetMasterPageTests
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
        public void SetPnPMasterPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Specifies the Master page URL based on the server relative URL
				var masterPageServerRelativeUrl = "";
				// From Cmdlet Help: Specifies the custom Master page URL based on the server relative URL
				var customMasterPageServerRelativeUrl = "";
				// From Cmdlet Help: Specifies the Master page URL based on the site relative URL
				var masterPageSiteRelativeUrl = "";
				// From Cmdlet Help: Specifies the custom Master page URL based on the site relative URL
				var customMasterPageSiteRelativeUrl = "";

                var results = scope.ExecuteCommand("Set-PnPMasterPage",
					new CommandParameter("MasterPageServerRelativeUrl", masterPageServerRelativeUrl),
					new CommandParameter("CustomMasterPageServerRelativeUrl", customMasterPageServerRelativeUrl),
					new CommandParameter("MasterPageSiteRelativeUrl", masterPageSiteRelativeUrl),
					new CommandParameter("CustomMasterPageSiteRelativeUrl", customMasterPageSiteRelativeUrl));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            