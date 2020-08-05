using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class AddOffice365GroupToSiteTests
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
        public void AddPnPOffice365GroupToSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Url of the site to be connected to an Microsoft 365 Group
				var url = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the alias of the group. Cannot contain spaces.
				var alias = "";
				// From Cmdlet Help: The optional description of the group
				var description = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The display name of the group
				var displayName = "";
				// From Cmdlet Help: Specifies the classification of the group
				var classification = "";
				// From Cmdlet Help: Specifies if the group is public. Defaults to false.
				var isPublic = "";
				// From Cmdlet Help: Specifies if the current site home page is kept. Defaults to false.
				var keepOldHomePage = "";
				// From Cmdlet Help: If specified the site will be associated to the hubsite as identified by this id
				var hubSiteId = "";
				// From Cmdlet Help: The array UPN values of the group's owners.
				var owners = "";

                var results = scope.ExecuteCommand("Add-PnPOffice365GroupToSite",
					new CommandParameter("Url", url),
					new CommandParameter("Alias", alias),
					new CommandParameter("Description", description),
					new CommandParameter("DisplayName", displayName),
					new CommandParameter("Classification", classification),
					new CommandParameter("IsPublic", isPublic),
					new CommandParameter("KeepOldHomePage", keepOldHomePage),
					new CommandParameter("HubSiteId", hubSiteId),
					new CommandParameter("Owners", owners));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            