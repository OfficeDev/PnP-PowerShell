using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Tenant
{
    [TestClass]
    public class NewTenantSequenceTeamNoGroupSubSiteTests
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
        public void NewPnPTenantSequenceTeamNoGroupSubSiteTest()
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
				var description = "";
				var templateIds = "";
				var quickLaunchDisabled = "";
				var useDifferentPermissionsFromParentSite = "";

                var results = scope.ExecuteCommand("New-PnPTenantSequenceTeamNoGroupSubSite",
					new CommandParameter("Url", url),
					new CommandParameter("Title", title),
					new CommandParameter("TimeZoneId", timeZoneId),
					new CommandParameter("Language", language),
					new CommandParameter("Description", description),
					new CommandParameter("TemplateIds", templateIds),
					new CommandParameter("QuickLaunchDisabled", quickLaunchDisabled),
					new CommandParameter("UseDifferentPermissionsFromParentSite", useDifferentPermissionsFromParentSite));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            