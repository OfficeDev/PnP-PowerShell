using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class AddListFoldersToProvisioningTemplateTests
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
        public void AddPnPListFoldersToProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Filename of the .PNP Open XML site template to read from, optionally including full path.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The list to query
				var list = "";
				// From Cmdlet Help: A switch parameter to include all folders in the list, or just top level folders.
				var recursive = "";
				// From Cmdlet Help: A switch to include ObjectSecurity information.
				var includeSecurity = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while loading the template.
				var templateProviderExtensions = "";

                var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
					new CommandParameter("Path", path),
					new CommandParameter("List", list),
					new CommandParameter("Recursive", recursive),
					new CommandParameter("IncludeSecurity", includeSecurity),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            