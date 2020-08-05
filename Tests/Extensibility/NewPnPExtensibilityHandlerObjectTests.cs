using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Extensibility
{
    [TestClass]
    public class NewExtensibilityHandlerObjectTests
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
        public void NewPnPExtensibilityHandlerObjectTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The full assembly name of the handler
				var assembly = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The type of the handler
				var type = "";
				// From Cmdlet Help: Any configuration data you want to send to the handler
				var configuration = "";
				// From Cmdlet Help: If set, the handler will be disabled
				var disabled = "";

                var results = scope.ExecuteCommand("New-PnPExtensibilityHandlerObject",
					new CommandParameter("Assembly", assembly),
					new CommandParameter("Type", type),
					new CommandParameter("Configuration", configuration),
					new CommandParameter("Disabled", disabled));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            