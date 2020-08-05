using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class AddWebPartToWebPartPageTests
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
        public void AddPnPWebPartToWebPartPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Server Relative Url of the page to add the web part to.
				var serverRelativePageUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A string containing the XML for the web part.
				var xml = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A path to a web part file on a the file system.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Zone Id where the web part must be placed
				var zoneId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Zone Index where the web part must be placed
				var zoneIndex = "";

                var results = scope.ExecuteCommand("Add-PnPWebPartToWebPartPage",
					new CommandParameter("ServerRelativePageUrl", serverRelativePageUrl),
					new CommandParameter("Xml", xml),
					new CommandParameter("Path", path),
					new CommandParameter("ZoneId", zoneId),
					new CommandParameter("ZoneIndex", zoneIndex));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            