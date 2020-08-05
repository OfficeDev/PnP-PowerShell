using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class GetWebPartPropertyTests
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
        public void GetPnPWebPartPropertyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Full server relative URL of the web part page, e.g. /sites/mysite/sitepages/home.aspx
				var serverRelativePageUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The id of the web part
				var identity = "";
				// From Cmdlet Help: Name of a single property to be returned
				var key = "";

                var results = scope.ExecuteCommand("Get-PnPWebPartProperty",
					new CommandParameter("ServerRelativePageUrl", serverRelativePageUrl),
					new CommandParameter("Identity", identity),
					new CommandParameter("Key", key));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            