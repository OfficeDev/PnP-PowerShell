using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class GetListItemTests
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
        public void GetPnPListItemTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The list to query
				var list = "";
				// From Cmdlet Help: The ID of the item to retrieve
				var id = "";
				// From Cmdlet Help: The unique id (GUID) of the item to retrieve
				var uniqueId = "";
				// From Cmdlet Help: The CAML query to execute against the list
				var query = "";
				// From Cmdlet Help: The server relative URL of a list folder from which results will be returned.
				var folderServerRelativeUrl = "";
				// From Cmdlet Help: The fields to retrieve. If not specified all fields will be loaded in the returned list object.
				var fields = "";
				// From Cmdlet Help: The number of items to retrieve per page request.
				var pageSize = "";
				// From Cmdlet Help: The script block to run after every page request.
				var scriptBlock = "";

                var results = scope.ExecuteCommand("Get-PnPListItem",
					new CommandParameter("List", list),
					new CommandParameter("Id", id),
					new CommandParameter("UniqueId", uniqueId),
					new CommandParameter("Query", query),
					new CommandParameter("FolderServerRelativeUrl", folderServerRelativeUrl),
					new CommandParameter("Fields", fields),
					new CommandParameter("PageSize", pageSize),
					new CommandParameter("ScriptBlock", scriptBlock));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            