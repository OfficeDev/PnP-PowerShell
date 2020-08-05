using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Search
{
    [TestClass]
    public class SetSearchSettingsTests
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
        public void SetPnPSearchSettingsTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Set the scope of which the suite bar search box shows. Possible values: Inherit, AllPages, ModernOnly, Hidden
				var searchBoxInNavBar = "";
				// From Cmdlet Help: Set the URL where the search box should redirect to.
				var searchPageUrl = "";
				// From Cmdlet Help: Set the search scope of the suite bar search box. Possible values: DefaultScope, Tenant, Hub, Site
				var searchScope = "";
				// From Cmdlet Help: Scope to apply the setting to. Possible values: Web (default), Site\r\n\r\nFor a root site, the scope does not matter.
				var scopeVar = "";
				// From Cmdlet Help: Do not ask for confirmation.
				var force = "";

                var results = scope.ExecuteCommand("Set-PnPSearchSettings",
					new CommandParameter("SearchBoxInNavBar", searchBoxInNavBar),
					new CommandParameter("SearchPageUrl", searchPageUrl),
					new CommandParameter("SearchScope", searchScope),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            