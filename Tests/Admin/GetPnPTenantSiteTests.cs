using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class GetTenantSiteTests
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
        public void GetPnPTenantSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The URL of the site
				var url = "";
				// From Cmdlet Help: By default, all sites will be returned. Specify a template value alike "STS#0" here to filter on the template
				var template = "";
				// From Cmdlet Help: By default, not all returned attributes are populated. This switch populates all attributes. It can take several seconds to run. Without this, some attributes will show default values that may not be correct.
				var detailed = "";
				// From Cmdlet Help: By default, the OneDrives are not returned. This switch includes all OneDrives.
				var includeOneDriveSites = "";
				// From Cmdlet Help: When the switch IncludeOneDriveSites is used, this switch ignores the question shown that the command can take a long time to execute
				var force = "";
				// From Cmdlet Help: Limit results to a specific web template name
				var webTemplate = "";
				// From Cmdlet Help: Specifies the script block of the server-side filter to apply. See https://technet.microsoft.com/en-us/library/fp161380.aspx
				var filter = "";

                var results = scope.ExecuteCommand("Get-PnPTenantSite",
					new CommandParameter("Url", url),
					new CommandParameter("Template", template),
					new CommandParameter("Detailed", detailed),
					new CommandParameter("IncludeOneDriveSites", includeOneDriveSites),
					new CommandParameter("Force", force),
					new CommandParameter("WebTemplate", webTemplate),
					new CommandParameter("Filter", filter));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            