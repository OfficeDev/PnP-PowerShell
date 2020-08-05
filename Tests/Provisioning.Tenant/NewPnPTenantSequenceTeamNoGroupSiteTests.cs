using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Tenant
{
    [TestClass]
    public class NewTenantSequenceTeamNoGroupSiteTests
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
        public void NewPnPTenantSequenceTeamNoGroupSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				var url = "";
				// This is a mandatory parameter
				var title = "";
				// This is a mandatory parameter
				var timeZoneId = "";
				var language = "";
				var owner = "";
				var description = "";
				var hubSite = "";
				var templateIds = "";

                var results = scope.ExecuteCommand("New-PnPTenantSequenceTeamNoGroupSite",
					new CommandParameter("Url", url),
					new CommandParameter("Title", title),
					new CommandParameter("TimeZoneId", timeZoneId),
					new CommandParameter("Language", language),
					new CommandParameter("Owner", owner),
					new CommandParameter("Description", description),
					new CommandParameter("HubSite", hubSite),
					new CommandParameter("TemplateIds", templateIds));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            