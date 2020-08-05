using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class AddFileToProvisioningTemplateTests
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
        public void AddPnPFileToProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Filename of the .PNP Open XML site template to read from, optionally including full path.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The file to add to the in-memory template, optionally including full path.
				var source = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The file to add to the in-memory template, specifying its url in the current connected Web.
				var sourceUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The target Folder for the file to add to the in-memory template.
				var folder = "";
				// From Cmdlet Help: The target Container for the file to add to the in-memory template, optional argument.
				var container = "";
				// From Cmdlet Help: The level of the files to add. Defaults to Published
				var fileLevel = "";
				// From Cmdlet Help: Set to overwrite in site, Defaults to true
				var fileOverwrite = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while loading the template.
				var templateProviderExtensions = "";

                var results = scope.ExecuteCommand("Add-PnPFileToProvisioningTemplate",
					new CommandParameter("Path", path),
					new CommandParameter("Source", source),
					new CommandParameter("SourceUrl", sourceUrl),
					new CommandParameter("Folder", folder),
					new CommandParameter("Container", container),
					new CommandParameter("FileLevel", fileLevel),
					new CommandParameter("FileOverwrite", fileOverwrite),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            