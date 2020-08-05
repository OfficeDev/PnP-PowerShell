using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class GetNavigationNodeTests
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
        public void GetPnPNavigationNodeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The location of the nodes to retrieve. Either TopNavigationBar, QuickLaunch, SearchNav or Footer.
				var location = "";
				// From Cmdlet Help: The Id of the node to retrieve
				var id = "";
				// From Cmdlet Help: Show a tree view of all navigation nodes
				var tree = "";

                var results = scope.ExecuteCommand("Get-PnPNavigationNode",
					new CommandParameter("Location", location),
					new CommandParameter("Id", id),
					new CommandParameter("Tree", tree));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            