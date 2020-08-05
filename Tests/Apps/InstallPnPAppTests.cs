using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Apps
{
    [TestClass]
    public class InstallAppTests
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
        public void InstallPnPAppTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the Id or an actual app metadata instance
				var identity = "";
				// From Cmdlet Help: Defines which app catalog to use. Defaults to Tenant
				var scopeVar = "";
				// From Cmdlet Help: If specified the execution will pause until the app has been installed in the site.
				var wait = "";

                var results = scope.ExecuteCommand("Install-PnPApp",
					new CommandParameter("Identity", identity),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Wait", wait));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            