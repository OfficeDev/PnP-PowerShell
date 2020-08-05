using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class GrantHubSiteRightsTests
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
        public void GrantPnPHubSiteRightsTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Hub Site to set the permissions on to associate another site with this Hub Site
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: One or more usernames that will be given or revoked the permission to associate a site with this Hub Site. It does not replace permissions given out before but adds to the already existing permissions.
				var principals = "";
				// From Cmdlet Help: Provide Join to give permissions to associate a site with this Hub Site or use None to revoke the permissions for the user(s) specified with the Principals argument
				var rights = "";

                var results = scope.ExecuteCommand("Grant-PnPHubSiteRights",
					new CommandParameter("Identity", identity),
					new CommandParameter("Principals", principals),
					new CommandParameter("Rights", rights));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            