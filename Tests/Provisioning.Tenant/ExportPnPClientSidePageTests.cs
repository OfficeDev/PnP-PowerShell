using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Tenant
{
    [TestClass]
    public class ExportClientSidePageTests
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
        public void ExportPnPClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page
				var identity = "";
				// From Cmdlet Help: If specified referenced files will be exported to the current folder.
				var persistBrandingFiles = "";
				// From Cmdlet Help: If specified the template will be saved to the file specified with this parameter.
				var outVar = "";
				// From Cmdlet Help: Specify to override the question to overwrite a file if it already exists.
				var force = "";
				// From Cmdlet Help: Specify a JSON configuration file to configure the extraction progress.
				var configuration = "";

                var results = scope.ExecuteCommand("Export-PnPClientSidePage",
					new CommandParameter("Identity", identity),
					new CommandParameter("PersistBrandingFiles", persistBrandingFiles),
					new CommandParameter("Out", outVar),
					new CommandParameter("Force", force),
					new CommandParameter("Configuration", configuration));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            