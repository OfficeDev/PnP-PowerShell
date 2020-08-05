using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class AddWebPartToWikiPageTests
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
        public void AddPnPWebPartToWikiPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Full server relative url of the web part page, e.g. /sites/demo/sitepages/home.aspx
				var serverRelativePageUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A string containing the XML for the web part.
				var xml = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A path to a web part file on a the file system.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Row number where the web part must be placed
				var row = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Column number where the web part must be placed
				var column = "";
				// From Cmdlet Help: Must there be a extra space between the web part
				var addSpace = "";

                var results = scope.ExecuteCommand("Add-PnPWebPartToWikiPage",
					new CommandParameter("ServerRelativePageUrl", serverRelativePageUrl),
					new CommandParameter("Xml", xml),
					new CommandParameter("Path", path),
					new CommandParameter("Row", row),
					new CommandParameter("Column", column),
					new CommandParameter("AddSpace", addSpace));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            