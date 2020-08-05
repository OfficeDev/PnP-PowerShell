using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class GetApplicationCustomizerTests
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
        public void GetPnPApplicationCustomizerTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Identity of the SharePoint Framework client side extension application customizer to return. Omit to return all SharePoint Frameworkclient side extension application customizer.
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be removed
				var clientSideComponentId = "";
				// From Cmdlet Help: Scope of the SharePoint Framework client side extension application customizer, either Web, Site or All to return both (all is the default)
				var scopeVar = "";
				// From Cmdlet Help: Switch parameter if an exception should be thrown if the requested SharePoint Frameworkclient side extension application customizer does not exist (true) or if omitted, nothing will be returned in case the SharePoint Framework client side extension application customizer does not exist
				var throwExceptionIfCustomActionNotFound = "";

                var results = scope.ExecuteCommand("Get-PnPApplicationCustomizer",
					new CommandParameter("Identity", identity),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("ThrowExceptionIfCustomActionNotFound", throwExceptionIfCustomActionNotFound));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            