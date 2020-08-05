using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Linq;
using PnP.PowerShell.Commands.Principals;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class WebTests
	{
		[TestInitialize]
		public void Initialize()
		{
			using (var ctx = TestCommon.CreateClientContext())
			{
				ctx.Web.Webs.Add(new WebCreationInformation()
				{
					Title = "PnPTestWeb",
					Url = "PnPTestWeb",
					WebTemplate = "STS#0",
					UseSamePermissionsAsParentSite = false
				});
				ctx.ExecuteQueryRetry();
			}
		}

		[TestCleanup]
		public void Cleanup()
		{
			using (var ctx = TestCommon.CreateClientContext())
			{
				ctx.Web.DeleteWeb("PnPTestWeb");
				ctx.Web.DeleteWeb("PnPTestWeb2");
			}
		}

		[TestMethod]
        public void NewWebTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("New-PnPWeb",
                    new CommandParameter("Title", "PnPTestWeb2"),
                    new CommandParameter("Url", "PnPTestWeb2"),
					new CommandParameter("Template", "STS#1"));
				
				Assert.IsTrue(results.Any());
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.Web));
            }
		}

		[TestMethod]
		public void SetWebPermissionByWebUrlTest()
		{
			string webUrl = "PnPTestWeb";
			string group = "Excel Services Viewers";

			// Execute cmd-let
			using (var scope = new PSTestScope(true))
			{
				var results = scope.ExecuteCommand("Set-PnPWebPermission",
					new CommandParameter("Url", "PnPTestWeb"),
					new CommandParameter("Group", group),
					new CommandParameter("AddRole", "Contribute"));
			}

			// Check that permission is added
			using (var ctx = TestCommon.CreateClientContext())
			{
				var web = ctx.Web.GetWeb(webUrl);
				var roles = web.RoleAssignments;
				ctx.Load(roles, role => role.Include(r => r.Member));
				ctx.ExecuteQueryRetry();

				Assert.IsTrue(roles[0].Member.LoginName == group);
			}
		}

		[TestMethod]
		public void SetWebPermissionByWebIdTest()
		{
			string webUrl = "PnPTestWeb";
			string group = "Excel Services Viewers";

			using (var ctx = TestCommon.CreateClientContext())
			{
				// Get PnPTestWeb ID
				var web = ctx.Web.GetWeb(webUrl);
				web.EnsureProperty(w => w.Id);

				// Execute cmd-let
				using (var scope = new PSTestScope(true))
				{
					var results = scope.ExecuteCommand("Set-PnPWebPermission",
						new CommandParameter("Identity", web.Id),
						new CommandParameter("Group", group),
						new CommandParameter("AddRole", "Contribute"));
				}

				// Check that permission is added
				var roles = web.RoleAssignments;
				ctx.Load(roles, role => role.Include(r => r.Member));
				ctx.ExecuteQueryRetry();

				Assert.IsTrue(roles[0].Member.LoginName == group);
			}
		}

        [TestMethod]
        public void GetUser()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.EnsureProperties(w => w.CurrentUser.Id, w => w.CurrentUser.LoginName);
                var currentUser = ctx.Web.CurrentUser;

                // Execute cmd-let
                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPUser");
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.User));

                    results = scope.ExecuteCommand("Get-PnPUser",
                        new CommandParameter("Identity", currentUser.Id));
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    var user = results[0].BaseObject as User;
                    Assert.IsNotNull(user);
                    Assert.AreEqual(currentUser.Id, user.Id);
                    Assert.AreEqual(currentUser.LoginName, user.LoginName);

                    results = scope.ExecuteCommand("Get-PnPUser",
                        new CommandParameter("Identity", currentUser.LoginName));
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    user = results[0].BaseObject as User;
                    Assert.IsNotNull(user);
                    Assert.AreEqual(currentUser.Id, user.Id);
                    Assert.AreEqual(currentUser.LoginName, user.LoginName);

                    results = scope.ExecuteCommand("Get-PnPUser",
                        new CommandParameter("Identity", currentUser));
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    user = results[0].BaseObject as User;
                    Assert.IsNotNull(user);
                    Assert.AreEqual(currentUser.Id, user.Id);
                    Assert.AreEqual(currentUser.LoginName, user.LoginName);
                }
            }
        }
    }
}

