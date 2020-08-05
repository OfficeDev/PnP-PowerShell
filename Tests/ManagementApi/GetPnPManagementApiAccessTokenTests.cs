using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ManagementApi
{
    [TestClass]
    public class GetManagementApiAccessTokenTests
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
        public void GetPnPManagementApiAccessTokenTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Tenant ID to connect to the Office 365 Management API
				var tenantId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The App\Client ID of the app which gives you access to the Office 365 Management API
				var clientId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client Secret of the app which gives you access to the Office 365 Management API
				var clientSecret = "";

                var results = scope.ExecuteCommand("Get-PnPManagementApiAccessToken",
					new CommandParameter("TenantId", tenantId),
					new CommandParameter("ClientId", clientId),
					new CommandParameter("ClientSecret", clientSecret));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            