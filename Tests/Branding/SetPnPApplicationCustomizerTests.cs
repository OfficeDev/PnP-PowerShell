using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class SetApplicationCustomizerTests
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
        public void SetPnPApplicationCustomizerTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The id or name of the CustomAction representing the client side extension registration that needs to be updated or a CustomAction instance itself
				var identity = "";
				// From Cmdlet Help: The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be updated
				var clientSideComponentId = "";
				// From Cmdlet Help: Define if the CustomAction representing the client side extension registration is to be found at the web or site collection scope. Specify All to update the component on both web and site collection level.
				var scopeVar = "";
				// From Cmdlet Help: The title of the application customizer. Omit to not update this property.
				var title = "";
				// From Cmdlet Help: The description of the application customizer. Omit to not update this property.
				var description = "";
				// From Cmdlet Help: Sequence of this application customizer being injected. Use when you have a specific sequence with which to have multiple application customizers being added to the page. Omit to not update this property.
				var sequence = "";
				// From Cmdlet Help: The Client Side Component Properties of the application customizer to update. Specify values as a json string : "{Property1 : 'Value1', Property2: 'Value2'}". Omit to not update this property.
				var clientSideComponentProperties = "";

                var results = scope.ExecuteCommand("Set-PnPApplicationCustomizer",
					new CommandParameter("Identity", identity),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Sequence", sequence),
					new CommandParameter("ClientSideComponentProperties", clientSideComponentProperties));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            