using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class GetWebPartXmlTests
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
        public void GetPnPWebPartXmlTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Full server relative url of the web part page, e.g. /sites/mysite/sitepages/home.aspx
				var serverRelativePageUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Id or title of the web part. Use Get-PnPWebPart to retrieve all web part Ids
				var identity = "";

                var results = scope.ExecuteCommand("Get-PnPWebPartXml",
					new CommandParameter("ServerRelativePageUrl", serverRelativePageUrl),
					new CommandParameter("Identity", identity));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            