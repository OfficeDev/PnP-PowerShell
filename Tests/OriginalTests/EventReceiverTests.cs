using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using System.Linq;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class EventReceiverTests
    {

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var receiver = ctx.Web.GetEventReceiverByName("TestEventReceiver");
                if (receiver != null)
                {
                    receiver.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void AddEventReceiverTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Add-PnPEventReceiver",
                    new CommandParameter("Name", "TestEventReceiver"),
                    new CommandParameter("Url", "https://testserver.com/testeventreceiver.svc"),
                    new CommandParameter("EventReceiverType", EventReceiverType.ListAdded),
                    new CommandParameter("Synchronization", EventReceiverSynchronization.Asynchronous));


                using (var ctx = TestCommon.CreateClientContext())
                {
                    var receiver = ctx.Web.GetEventReceiverByName("TestEventReceiver");

                    Assert.IsNotNull(receiver);
                    Assert.IsTrue(receiver.ReceiverUrl == "https://testserver.com/testeventreceiver.svc");
                    Assert.IsTrue(receiver.EventType == EventReceiverType.ListAdded);
                    Assert.IsTrue(receiver.Synchronization == EventReceiverSynchronization.Asynchronous);

                    receiver.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }

            }
        }

        [TestMethod]
        public void GetEventReceiverTest()
        {
            using (var scope = new PSTestScope(true))
            {
                EventReceiverDefinition receiver = null;
                using (var ctx = TestCommon.CreateClientContext())
                {
                    receiver = ctx.Web.AddRemoteEventReceiver("TestEventReceiver", "https://testserver.com/testeventreceiver.svc", EventReceiverType.ListAdded, EventReceiverSynchronization.Asynchronous, true);
                }

                var results = scope.ExecuteCommand("Get-PnPEventReceiver");

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(EventReceiverDefinition));


                results = scope.ExecuteCommand("Get-PnPEventReceiver",
                    new CommandParameter("Identity", receiver.ReceiverId));

                Assert.IsTrue(results.Any());

                using (var ctx = TestCommon.CreateClientContext())
                {
                    receiver = ctx.Web.GetEventReceiverByName("TestEventReceiver");

                    receiver.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }

            }
        }

        [TestMethod]
        public void RemoveEventReceiverTest()
        {
            using (var scope = new PSTestScope(true))
            {
                EventReceiverDefinition receiver = null;
                using (var ctx = TestCommon.CreateClientContext())
                {
                    receiver = ctx.Web.AddRemoteEventReceiver("TestEventReceiver", "https://testserver.com/testeventreceiver.svc", EventReceiverType.ListAdded, EventReceiverSynchronization.Asynchronous, true);
                }

                var results = scope.ExecuteCommand("Remove-PnPEventReceiver",
                    new CommandParameter("Identity", receiver.ReceiverId),
                    new CommandParameter("Force"));


                using (var ctx = TestCommon.CreateClientContext())
                {
                    receiver = ctx.Web.GetEventReceiverByName("TestEventReceiver");

                    Assert.IsNull(receiver);
                }

            }
        }
    }
}
