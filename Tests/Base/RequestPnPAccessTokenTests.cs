using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Base
{
    [TestClass]
    public class RequestAccessTokenTests
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
        public void RequestPnPAccessTokenTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The Azure Application Client Id to use to retrieve the token. Defaults to the PnP Office 365 Management Shell
				var clientId = "";
				// From Cmdlet Help: The scopes to retrieve the token for. Defaults to AllSites.FullControl
				var resource = "";
				// From Cmdlet Help: The scopes to retrieve the token for. Defaults to AllSites.FullControl
				var scopeVars = "";
				// From Cmdlet Help: Returns the token in a decoded / human readible manner
				var decoded = "";
				// From Cmdlet Help: Set this token as the current token to use when performing Azure AD based authentication requests with PnP PowerShell
				var setAsCurrent = "";
				// From Cmdlet Help: Optional credentials to use when retrieving the access token. If not present you need to connect first with Connect-PnPOnline.
				var credentials = "";
				// From Cmdlet Help: Optional tenant URL to use when retrieving the access token. The Url should be in the shape of https://yourtenant.sharepoint.com. See examples for more info.
				var tenantUrl = "";

                var results = scope.ExecuteCommand("Request-PnPAccessToken",
					new CommandParameter("ClientId", clientId),
					new CommandParameter("Resource", resource),
					new CommandParameter("Scopes", scopeVars),
					new CommandParameter("Decoded", decoded),
					new CommandParameter("SetAsCurrent", setAsCurrent),
					new CommandParameter("Credentials", credentials),
					new CommandParameter("TenantUrl", tenantUrl));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            