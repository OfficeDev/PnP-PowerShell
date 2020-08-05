using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Publishing
{
    [TestClass]
    public class AddHtmlPublishingPageLayoutTests
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
        public void AddPnPHtmlPublishingPageLayoutTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Path to the file which will be uploaded
				var sourceFilePath = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Title for the page layout
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Description for the page layout
				var description = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Associated content type ID
				var associatedContentTypeID = "";
				// From Cmdlet Help: Folder hierarchy where the HTML page layouts will be deployed
				var destinationFolderHierarchy = "";

                var results = scope.ExecuteCommand("Add-PnPHtmlPublishingPageLayout",
					new CommandParameter("SourceFilePath", sourceFilePath),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("AssociatedContentTypeID", associatedContentTypeID),
					new CommandParameter("DestinationFolderHierarchy", destinationFolderHierarchy));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            