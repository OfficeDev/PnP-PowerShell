using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Publishing
{
    [TestClass]
    public class AddPublishingPageTests
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
        public void AddPnPPublishingPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page to be added as an aspx file
				var pageName = "";
				// From Cmdlet Help: The site relative folder path of the page to be added
				var folderPath = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page layout you want to use. Specify without the .aspx extension. So 'ArticleLeft' or 'BlankWebPartPage'
				var pageTemplateName = "";
				// From Cmdlet Help: The title of the page
				var title = "";
				// From Cmdlet Help: Publishes the page. Also Approves it if moderation is enabled on the Pages library.
				var publish = "";

                var results = scope.ExecuteCommand("Add-PnPPublishingPage",
					new CommandParameter("PageName", pageName),
					new CommandParameter("FolderPath", folderPath),
					new CommandParameter("PageTemplateName", pageTemplateName),
					new CommandParameter("Title", title),
					new CommandParameter("Publish", publish));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            