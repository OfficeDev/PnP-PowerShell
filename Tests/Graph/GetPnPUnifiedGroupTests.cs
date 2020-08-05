using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class GetUnifiedGroupTests
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
        public void GetPnPUnifiedGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The Identity of the Microsoft 365 Group
				var identity = "";
				// From Cmdlet Help: Exclude fetching the site URL for Microsoft 365 Groups. This speeds up large listings.
				var excludeSiteUrl = "";
				// From Cmdlet Help: Include Classification value of Microsoft 365 Groups
				var includeClassification = "";
				// From Cmdlet Help: Include a flag for every Microsoft 365 Group if it has a Microsoft Team provisioned for it. This will slow down the retrieval of Microsoft 365 Groups so only use it if you need it.
				var includeHasTeam = "";

                var results = scope.ExecuteCommand("Get-PnPUnifiedGroup",
					new CommandParameter("Identity", identity),
					new CommandParameter("ExcludeSiteUrl", excludeSiteUrl),
					new CommandParameter("IncludeClassification", includeClassification),
					new CommandParameter("IncludeHasTeam", includeHasTeam));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            