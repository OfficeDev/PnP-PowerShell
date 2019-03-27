using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Linq;
using SharePointPnP.PowerShell.Commands.Principals;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Tests
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

#if !ONPREMISES
        [TestMethod]
        public void AddAlert_WithDefaultProperties_Test()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.EnsureProperties(w => w.CurrentUser.Id, w => w.CurrentUser.LoginName);
                var currentUser = ctx.Web.CurrentUser;
                var randomizer = new Random();
                var randomAlertTitle = randomizer.Next(int.MaxValue).ToString();
                var list = ctx.Web.DefaultDocumentLibrary();
                list.EnsureProperties(l => l.Id);
                var listId = list.Id;

                // Execute cmd-let
                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Add-PnPAlert",
                        new CommandParameter("List", listId),
                        new CommandParameter("Title", randomAlertTitle));
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.AlertCreationInformation));
                    var alertInfo = results[0].BaseObject as AlertCreationInformation;
                    Assert.AreEqual(randomAlertTitle, alertInfo.Title);
                }

                // get actual alert and check properties
                ctx.Web.EnsureProperties(w => w.CurrentUser.Alerts);
                var newAlerts = currentUser.Alerts.Where(a => a.Title == randomAlertTitle);
                Assert.AreEqual(1, newAlerts.Count(), "Unexpected number of created alerts");
                var newAlert = newAlerts.First();
                newAlert.EnsureProperty(a => a.Properties);
                newAlert.User.EnsureProperties(u => u.LoginName);
                newAlert.List.EnsureProperties(l => l.Id);
                ctx.ExecuteQueryRetry();

                try
                {
                    Assert.AreEqual(AlertFrequency.Immediate, newAlert.AlertFrequency);
                    Assert.AreEqual(AlertDeliveryChannel.Email, newAlert.DeliveryChannels);
                    Assert.AreEqual(AlertEventType.All, newAlert.EventType);
                    Assert.AreEqual(AlertStatus.On, newAlert.Status);
                    Assert.AreEqual(AlertType.List, newAlert.AlertType);
                    Assert.AreEqual(currentUser.LoginName, newAlert.User.LoginName);
                    Assert.AreEqual(listId, newAlert.List.Id);
                    Assert.AreEqual(1, newAlert.Properties.Count, "Unexpected number of properties");
                    Assert.IsTrue(newAlert.Properties.ContainsKey("filterindex"));
                    Assert.AreEqual("0", newAlert.Properties["filterindex"]);
                }
                finally
                {
                    // delete alert
                    currentUser.Alerts.DeleteAlert(newAlert.ID);
                    currentUser.Update();
                    ctx.ExecuteQueryRetry();
                }
            }
        }
#endif

#if !ONPREMISES
        [TestMethod]
        public void AddAlert_WithNonDefaultProperties_Test()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.EnsureProperties(w => w.CurrentUser.Id, w => w.CurrentUser.LoginName);
                var currentUser = ctx.Web.CurrentUser;
                // generate random alert title
                var randomizer = new Random();
                var alertTitle = randomizer.Next(int.MaxValue).ToString();
                var list = ctx.Web.DefaultDocumentLibrary();
                list.EnsureProperties(l => l.Id);
                var listId = list.Id;
                var currentTime = DateTime.Now;
                // note: SharePoint rounds the milliseconds away, so use a time without milliseconds for testing
                var alertTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Minute) ;

                // Execute cmd-let
                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Add-PnPAlert",
                        new CommandParameter("List", "Shared Documents"),
                        new CommandParameter("Title", alertTitle),
                        new CommandParameter("DeliveryMethod", AlertDeliveryChannel.Email), // cannot use SmS without having Frequency set to "Immediate"
                        new CommandParameter("ChangeType", AlertEventType.DeleteObject),
                        new CommandParameter("Frequency", AlertFrequency.Weekly),
                        new CommandParameter("Time", alertTime),
                        new CommandParameter("Filter", AlertFilter.SomeoneElseChangesAnItem)
                        );
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                }

                // get actual alert and check properties
                ctx.Web.EnsureProperties(w => w.CurrentUser.Alerts);
                var newAlerts = currentUser.Alerts.Where(a => a.Title == alertTitle);
                Assert.AreEqual(1, newAlerts.Count(), "Unexpected number of created alerts");
                var newAlert = newAlerts.First();
                newAlert.EnsureProperty(a => a.AlertTime);
                newAlert.EnsureProperty(a => a.Properties);
                newAlert.User.EnsureProperties(u => u.LoginName);
                newAlert.List.EnsureProperties(l => l.Id);
                ctx.ExecuteQueryRetry();

                try
                {
                    Assert.AreEqual(AlertFrequency.Weekly, newAlert.AlertFrequency);
                    Assert.AreEqual(AlertDeliveryChannel.Email, newAlert.DeliveryChannels);
                    Assert.AreEqual(AlertEventType.DeleteObject, newAlert.EventType);
                    Assert.AreEqual(AlertStatus.On, newAlert.Status);
                    Assert.AreEqual(AlertType.List, newAlert.AlertType);
                    Assert.AreEqual(currentUser.LoginName, newAlert.User.LoginName);
                    Assert.AreEqual(listId, newAlert.List.Id);
                    
                    Assert.AreEqual(alertTime, newAlert.AlertTime);
                    Assert.AreEqual(1, newAlert.Properties.Count, "Unexpected number of properties");
                    Assert.IsTrue(newAlert.Properties.ContainsKey("filterindex"));
                    Assert.AreEqual("1", newAlert.Properties["filterindex"]);
                }
                finally
                {
                    // delete alert
                    currentUser.Alerts.DeleteAlert(newAlert.ID);
                    currentUser.Update();
                    ctx.ExecuteQueryRetry();
                }
            }
        }
#endif
    }
}
