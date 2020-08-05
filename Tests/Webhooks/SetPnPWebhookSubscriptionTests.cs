using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webhooks
{
    [TestClass]
    public class SetWebhookSubscriptionTests
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
        public void SetPnPWebhookSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The identity of the Webhook subscription to update
				var subscription = "";
				// From Cmdlet Help: The list object or name from which the Webhook subscription will be modified
				var list = "";
				// From Cmdlet Help: The URL of the Webhook endpoint that will be notified of the change
				var notificationUrl = "";
				// From Cmdlet Help: The date at which the Webhook subscription will expire. (Default: 6 months from today)
				var expirationDate = "";

                var results = scope.ExecuteCommand("Set-PnPWebhookSubscription",
					new CommandParameter("Subscription", subscription),
					new CommandParameter("List", list),
					new CommandParameter("NotificationUrl", notificationUrl),
					new CommandParameter("ExpirationDate", expirationDate));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            