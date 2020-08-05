using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class AddApplicationCustomizerTests
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
        public void AddPnPApplicationCustomizerTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The title of the application customizer
				var title = "";
				// From Cmdlet Help: The description of the application customizer
				var description = "";
				// From Cmdlet Help: Sequence of this application customizer being injected. Use when you have a specific sequence with which to have multiple application customizers being added to the page.
				var sequence = "";
				// From Cmdlet Help: The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.
				var scopeVar = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest
				var clientSideComponentId = "";
				// From Cmdlet Help: The Client Side Component Properties of the application customizer. Specify values as a json string : "{Property1 : 'Value1', Property2: 'Value2'}"
				var clientSideComponentProperties = "";
				// From Cmdlet Help: The Client Side Host Properties of the application customizer. Specify values as a json string : "{'preAllocatedApplicationCustomizerTopHeight': '50', 'preAllocatedApplicationCustomizerBottomHeight': '50'}"
				var clientSideHostProperties = "";

                var results = scope.ExecuteCommand("Add-PnPApplicationCustomizer",
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Sequence", sequence),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("ClientSideComponentProperties", clientSideComponentProperties),
					new CommandParameter("ClientSideHostProperties", clientSideHostProperties));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            