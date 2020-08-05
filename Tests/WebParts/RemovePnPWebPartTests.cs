using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class RemoveWebPartTests
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
        public void RemovePnPWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Guid of the web part
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the web part
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Server relative url of the web part page, e.g. /sites/demo/sitepages/home.aspx
				var serverRelativePageUrl = "";

                var results = scope.ExecuteCommand("Remove-PnPWebPart",
					new CommandParameter("Identity", identity),
					new CommandParameter("Title", title),
					new CommandParameter("ServerRelativePageUrl", serverRelativePageUrl));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            