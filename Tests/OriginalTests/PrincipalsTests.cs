using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Linq;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class PrincipalsTests
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestList", false);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestList");
                if (list != null)
                {
                    list.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }

                list = ctx.Web.GetListByTitle("PnPTestList2");
                if (list != null)
                {
                    list.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

#if !SP2013 && !SP2016
        [TestMethod]
        public void AddAlert_WithDefaultProperties_Test()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.EnsureProperties(w => w.CurrentUser.Id, w => w.CurrentUser.LoginName);
                var currentUser = ctx.Web.CurrentUser;
                var randomizer = new Random();
                var randomAlertTitle = randomizer.Next(int.MaxValue).ToString();
                var list = ctx.Web.GetListByTitle("PnPTestList");
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
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(AlertCreationInformation));
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

#if !SP2013 && !SP2016
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
                var list = ctx.Web.GetListByTitle("PnPTestList");
                list.EnsureProperties(l => l.Id);
                var listId = list.Id;
                var currentTime = DateTime.Now;
                // note: SharePoint rounds the milliseconds away, so use a time without milliseconds for testing
                var alertTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Minute);

                // Execute cmd-let
                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Add-PnPAlert",
                        new CommandParameter("List", "PnPTestList"),
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

#if !SP2013 && !SP2016
        [TestMethod]
        public void AddGetRemoveAlertTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.EnsureProperties(w => w.CurrentUser.Id, w => w.CurrentUser.LoginName);
                var currentUser = ctx.Web.CurrentUser;
                var randomizer = new Random();
                var randomAlertTitle = randomizer.Next(int.MaxValue).ToString();
                var list = ctx.Web.GetListByTitle("PnPTestList");
                list.EnsureProperties(l => l.Id);
                var listId = list.Id;
                Guid alertId;

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Add-PnPAlert",
                        new CommandParameter("List", listId),
                        new CommandParameter("Title", randomAlertTitle));
                    Assert.IsNotNull(results);
                    Assert.IsTrue(results.Count > 0);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(AlertCreationInformation));
                }

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPAlert",
                        new CommandParameter("List", listId),
                        new CommandParameter("Title", randomAlertTitle));
                    Assert.IsNotNull(results);
                    Assert.AreEqual(1, results.Count);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Alert));
                    var alert = results[0].BaseObject as Alert;
                    Assert.AreEqual(randomAlertTitle, alert.Title);
                    alertId = alert.ID;
                }

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPAlert",
                        new CommandParameter("Title", randomAlertTitle));
                    Assert.IsNotNull(results);
                    Assert.AreEqual(1, results.Count);
                }

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPAlert",
                        new CommandParameter("List", listId));
                    Assert.IsNotNull(results);
                    Assert.AreEqual(1, results.Count);
                }

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Remove-PnPAlert",
                        new CommandParameter("Identity", alertId),
                        new CommandParameter("Force"));
                }

                // check that the alert has been deleted
                ctx.Web.EnsureProperties(w => w.CurrentUser.Alerts);
                var newAlerts = currentUser.Alerts.Where(a => a.Title == randomAlertTitle);
                Assert.AreEqual(0, newAlerts.Count(), "Alert is still present");
            }
        }
#endif

    }
}
