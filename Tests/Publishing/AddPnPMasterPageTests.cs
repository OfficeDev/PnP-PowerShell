using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Publishing
{
    [TestClass]
    public class AddMasterPageTests
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
        public void AddPnPMasterPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Path to the file which will be uploaded
				var sourceFilePath = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Title for the Masterpage
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Description for the Masterpage
				var description = "";
				// From Cmdlet Help: Folder hierarchy where the MasterPage will be deployed
				var destinationFolderHierarchy = "";
				// From Cmdlet Help: UIVersion of the Masterpage. Default = 15
				var uIVersion = "";
				// From Cmdlet Help: Default CSS file for the MasterPage, this Url is SiteRelative
				var defaultCssFile = "";

                var results = scope.ExecuteCommand("Add-PnPMasterPage",
					new CommandParameter("SourceFilePath", sourceFilePath),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("DestinationFolderHierarchy", destinationFolderHierarchy),
					new CommandParameter("UIVersion", uIVersion),
					new CommandParameter("DefaultCssFile", defaultCssFile));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            