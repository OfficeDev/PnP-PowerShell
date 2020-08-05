using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class SetProvisioningTemplateMetadataTests
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
        public void SetPnPProvisioningTemplateMetadataTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Path to the xml or pnp file containing the site template.
				var path = "";
				// From Cmdlet Help: It can be used to specify the DisplayName of the template file that will be updated.
				var templateDisplayName = "";
				// From Cmdlet Help: It can be used to specify the ImagePreviewUrl of the template file that will be updated.
				var templateImagePreviewUrl = "";
				// From Cmdlet Help: It can be used to specify custom Properties for the template file that will be updated.
				var templateProperties = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while extracting a template.
				var templateProviderExtensions = "";

                var results = scope.ExecuteCommand("Set-PnPProvisioningTemplateMetadata",
					new CommandParameter("Path", path),
					new CommandParameter("TemplateDisplayName", templateDisplayName),
					new CommandParameter("TemplateImagePreviewUrl", templateImagePreviewUrl),
					new CommandParameter("TemplateProperties", templateProperties),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            