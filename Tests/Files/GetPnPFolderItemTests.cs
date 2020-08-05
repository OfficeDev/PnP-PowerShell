using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class GetFolderItemTests
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
        public void GetPnPFolderItemTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The site relative URL of the folder to retrieve
				var folderSiteRelativeUrl = "";
				// From Cmdlet Help: A folder instance to the folder to retrieve
				var identity = "";
				// From Cmdlet Help: The type of contents to retrieve, either File, Folder or All (default)
				var itemType = "";
				// From Cmdlet Help: Optional name of the item to retrieve
				var itemName = "";
				// From Cmdlet Help: A switch parameter to include contents of all subfolders in the specified folder
				var recursive = "";

                var results = scope.ExecuteCommand("Get-PnPFolderItem",
					new CommandParameter("FolderSiteRelativeUrl", folderSiteRelativeUrl),
					new CommandParameter("Identity", identity),
					new CommandParameter("ItemType", itemType),
					new CommandParameter("ItemName", itemName),
					new CommandParameter("Recursive", recursive));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            