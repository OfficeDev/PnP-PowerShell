#if !ONPREMISES
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class WebhookSubscriptionsTests
    {
        public const string PnPWebhookTestList = "PnPWebhookTestList";

        private List EnsureFreshTestList(ClientContext ctx)
        {
            if (ctx.Web.ListExists(PnPWebhookTestList))
            {
                List toDelete = ctx.Web.Lists.GetByTitle(PnPWebhookTestList);
                toDelete.DeleteObject();
                ctx.ExecuteQueryRetry();
            }

            // Create the test list
            List list =  ctx.Web.CreateList(ListTemplateType.GenericList, PnPWebhookTestList, false);
            list.EnsureProperty(l => l.Id);
            return list;
        }


        [TestMethod]
        public void AddListWebhookSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    // Create the test list
                    List testList = EnsureFreshTestList(ctx);
                    
                    // Test the Add-PnPWebhookSubscription cmdlet on the list
                    scope.ExecuteCommand("Add-PnPWebhookSubscription",
                    new CommandParameter("List", PnPWebhookTestList),
                    new CommandParameter("NotificationUrl", TestCommon.WebHookTestUrl));

                    IList<WebhookSubscription> webhookSubscriptions = testList.GetWebhookSubscriptions();
                    Assert.IsTrue(webhookSubscriptions.Count() == 1);

                    // Delete the test list
                    testList.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void RemoveListWebhookSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    // Create the test list
                    List testList = EnsureFreshTestList(ctx);

                    // Add a Webhook subscription
                    WebhookSubscription subscription = testList.AddWebhookSubscription(TestCommon.WebHookTestUrl);

                    // Test the Remove-PnPWebhookSubscription cmdlet on the list
                    scope.ExecuteCommand("Remove-PnPWebhookSubscription",
                    new CommandParameter("List", PnPWebhookTestList),
                    new CommandParameter("Identity", subscription.Id));

                    IList<WebhookSubscription> webhookSubscriptions = testList.GetWebhookSubscriptions();
                    Assert.IsTrue(webhookSubscriptions.Count() == 0);

                    // Delete the test list
                    testList.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }


        [TestMethod]
        public void GetListWebhookSubscriptionsTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    // Create the test list
                    List testList = EnsureFreshTestList(ctx);

                    // Add a Webhook subscription
                    WebhookSubscription subscription = testList.AddWebhookSubscription(TestCommon.WebHookTestUrl);

                    // Add a second Webhook subscription
                    WebhookSubscription subscription2 = testList.AddWebhookSubscription(TestCommon.WebHookTestUrl);


                    // Test the Get-PnPWebhookSubscriptions cmdlet on the list
                    var output = scope.ExecuteCommand("Get-PnPWebhookSubscriptions",
                    new CommandParameter("List", PnPWebhookTestList));

                    Assert.IsTrue(output.All(o => typeof(IList<WebhookSubscription>).IsAssignableFrom(o.BaseObject.GetType())));

                    // Delete the test list
                    testList.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void SetListWebhookSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    // Create the test list
                    List testList = EnsureFreshTestList(ctx);

                    // Add a Webhook subscription
                    WebhookSubscription subscription = testList.AddWebhookSubscription(TestCommon.WebHookTestUrl, DateTime.Today.AddDays(5));

                    // Change the expiration date
                    DateTime newExpirationDate = DateTime.Today.AddDays(20).ToUniversalTime();
                    subscription.ExpirationDateTime = newExpirationDate;
                    
                    // Test the Set-PnPWebhookSubscription cmdlet on the list
                    scope.ExecuteCommand("Set-PnPWebhookSubscription",
                    new CommandParameter("List", PnPWebhookTestList),
                    new CommandParameter("Subscription", subscription));

                    // Get the subscription from the test list
                    var subscriptions = testList.GetWebhookSubscriptions();

                    Assert.IsTrue(subscriptions.Count == 1 && subscriptions[0].ExpirationDateTime == newExpirationDate);

                    // Delete the test list
                    testList.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }
    }
}
#endif