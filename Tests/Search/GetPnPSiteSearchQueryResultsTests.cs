using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Search
{
    [TestClass]
    public class GetSiteSearchQueryResultsTests
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
        public void GetPnPSiteSearchQueryResultsTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Search query in Keyword Query Language (KQL) to execute to refine the returned sites. If omitted, all indexed sites will be returned.
				var query = "";
				// From Cmdlet Help: Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.
				var startRow = "";
				// From Cmdlet Help: Maximum amount of search results to return. Default and max is 500 search results.
				var maxResults = "";
				// From Cmdlet Help: Automatically page results until the end to get more than 500 sites. Use with caution!
				var all = "";

                var results = scope.ExecuteCommand("Get-PnPSiteSearchQueryResults",
					new CommandParameter("Query", query),
					new CommandParameter("StartRow", startRow),
					new CommandParameter("MaxResults", maxResults),
					new CommandParameter("All", all));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            