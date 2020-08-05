using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class NewListTests
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
        public void NewPnPListTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Title of the list
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The type of list to create.
				var template = "";
				// From Cmdlet Help: If set, will override the url of the list.
				var url = "";
				// From Cmdlet Help: Switch parameter if list should be hidden from the SharePoint UI
				var hidden = "";
				// From Cmdlet Help: Switch parameter if versioning should be enabled
				var enableVersioning = "";
				// From Cmdlet Help: Obsolete
				var quickLaunchOptions = "";
				// From Cmdlet Help: Switch parameter if content types should be enabled on this list
				var enableContentTypes = "";
				// From Cmdlet Help: Switch parameter if this list should be visible on the QuickLaunch
				var onQuickLaunch = "";

                var results = scope.ExecuteCommand("New-PnPList",
					new CommandParameter("Title", title),
					new CommandParameter("Template", template),
					new CommandParameter("Url", url),
					new CommandParameter("Hidden", hidden),
					new CommandParameter("EnableVersioning", enableVersioning),
					new CommandParameter("QuickLaunchOptions", quickLaunchOptions),
					new CommandParameter("EnableContentTypes", enableContentTypes),
					new CommandParameter("OnQuickLaunch", onQuickLaunch));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            