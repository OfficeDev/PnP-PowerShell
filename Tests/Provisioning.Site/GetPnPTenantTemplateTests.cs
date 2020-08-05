using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class GetTenantTemplateTests
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
        public void GetPnPTenantTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				var siteUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Filename to write to, optionally including full path
				var outVar = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Returns the template as an in-memory object, which is an instance of the ProvisioningHierarchy type of the PnP Core Component. It cannot be used together with the -Out parameter.
				var asInstance = "";
				// From Cmdlet Help: Specify a JSON configuration file to configure the extraction progress.
				var configuration = "";

                var results = scope.ExecuteCommand("Get-PnPTenantTemplate",
					new CommandParameter("SiteUrl", siteUrl),
					new CommandParameter("Out", outVar),
					new CommandParameter("Force", force),
					new CommandParameter("AsInstance", asInstance),
					new CommandParameter("Configuration", configuration));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            