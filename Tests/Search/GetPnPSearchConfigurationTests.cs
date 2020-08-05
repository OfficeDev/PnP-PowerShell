using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Search
{
    [TestClass]
    public class GetSearchConfigurationTests
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
        public void GetPnPSearchConfigurationTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Scope to use. Either Web, Site, or Subscription. Defaults to Web
				var scopeVar = "";
				// From Cmdlet Help: Local path where the search configuration will be saved
				var path = "";
				// From Cmdlet Help: Output format for of the configuration. Defaults to complete XML
				var outVarputFormat = "";

                var results = scope.ExecuteCommand("Get-PnPSearchConfiguration",
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Path", path),
					new CommandParameter("OutputFormat", outVarputFormat));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            