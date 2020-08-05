using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class RemoveFileFromProvisioningTemplateTests
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
        public void RemovePnPFileFromProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Filename to read the template from, optionally including full path.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The relative File Path of the file to remove from the in-memory template
				var filePath = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while saving the template.
				var templateProviderExtensions = "";

                var results = scope.ExecuteCommand("Remove-PnPFileFromProvisioningTemplate",
					new CommandParameter("Path", path),
					new CommandParameter("FilePath", filePath),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            