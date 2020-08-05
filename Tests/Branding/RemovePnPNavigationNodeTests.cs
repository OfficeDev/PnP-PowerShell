using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class RemoveNavigationNodeTests
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
        public void RemovePnPNavigationNodeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Id or node object to delete
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The location from where to remove the node. Either TopNavigationBar, QuickLaunch, SearchNav or Footer.
				var location = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The title of the node that needs to be removed
				var title = "";
				// From Cmdlet Help: The header where the node is located
				var header = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifying the All parameter will remove all the nodes from specified Location.
				var all = "";
				// From Cmdlet Help: Specifying the Force parameter will skip the confirmation question.
				var force = "";

                var results = scope.ExecuteCommand("Remove-PnPNavigationNode",
					new CommandParameter("Identity", identity),
					new CommandParameter("Location", location),
					new CommandParameter("Title", title),
					new CommandParameter("Header", header),
					new CommandParameter("All", all),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            