using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.SiteDesigns
{
    [TestClass]
    public class SetSiteScriptTests
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
        public void SetPnPSiteScriptTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The guid or an object representing the site script
				var identity = "";
				// From Cmdlet Help: The title of the site script
				var title = "";
				// From Cmdlet Help: The description of the site script
				var description = "";
				// From Cmdlet Help: A JSON string containing the site script
				var content = "";
				// From Cmdlet Help: Specifies the version of the site script
				var version = "";

                var results = scope.ExecuteCommand("Set-PnPSiteScript",
					new CommandParameter("Identity", identity),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Content", content),
					new CommandParameter("Version", version));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            