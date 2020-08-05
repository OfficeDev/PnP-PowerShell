using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Apps
{
    [TestClass]
    public class AddAppTests
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
        public void AddPnPAppTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the Id or an actual app metadata instance
				var path = "";
				// From Cmdlet Help: Defines which app catalog to use. Defaults to Tenant
				var scopeVar = "";
				// This is a mandatory parameter
				// From Cmdlet Help: This will deploy/trust an app into the app catalog
				var publish = "";
				var skipFeatureDeployment = "";
				// From Cmdlet Help: Overwrites the existing app package if it already exists
				var overwrite = "";
				// From Cmdlet Help: Specifies the timeout in seconds. Defaults to 200.
				var timeoutVar = "";

                var results = scope.ExecuteCommand("Add-PnPApp",
					new CommandParameter("Path", path),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Publish", publish),
					new CommandParameter("SkipFeatureDeployment", skipFeatureDeployment),
					new CommandParameter("Overwrite", overwrite),
					new CommandParameter("Timeout", timeoutVar));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            