using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class RemoveApplicationCustomizerTests
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
        public void RemovePnPApplicationCustomizerTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The id or name of the CustomAction representing the client side extension registration that needs to be removed or a CustomAction instance itself
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be removed
				var clientSideComponentId = "";
				// From Cmdlet Help: Define if the CustomAction representing the client side extension registration is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection (default).
				var scopeVar = "";
				// From Cmdlet Help: Use the -Force flag to bypass the confirmation question
				var force = "";

                var results = scope.ExecuteCommand("Remove-PnPApplicationCustomizer",
					new CommandParameter("Identity", identity),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            