using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class AddNavigationNodeTests
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
        public void AddPnPNavigationNodeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The location where to add the navigation node to. Either TopNavigationBar, QuickLaunch, SearchNav or Footer.
				var location = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The title of the node to add
				var title = "";
				// From Cmdlet Help: The url to navigate to when clicking the new menu item. This can either be absolute or relative to the Web. Fragments are not supported.
				var url = "";
				// From Cmdlet Help: The key of the parent. Leave empty to add to the top level
				var parent = "";
				// From Cmdlet Help: Optional value of a header entry to add the menu item to
				var header = "";
				// From Cmdlet Help: Add the new menu item to beginning of the collection
				var first = "";
				// From Cmdlet Help: Indicates the destination URL is outside of the site collection
				var external = "";

                var results = scope.ExecuteCommand("Add-PnPNavigationNode",
					new CommandParameter("Location", location),
					new CommandParameter("Title", title),
					new CommandParameter("Url", url),
					new CommandParameter("Parent", parent),
					new CommandParameter("Header", header),
					new CommandParameter("First", first),
					new CommandParameter("External", external));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            