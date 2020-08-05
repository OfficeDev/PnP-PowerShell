using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webhooks
{
    [TestClass]
    public class AddWebhookSubscriptionTests
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
        public void AddPnPWebhookSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The list object or name where the Webhook subscription will be added to
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The URL of the Webhook endpoint that will be notified of the change
				var notificationUrl = "";
				// From Cmdlet Help: The date at which the Webhook subscription will expire. (Default: 6 months from today)
				var expirationDate = "";
				// From Cmdlet Help: A client state information that will be passed through notifications
				var clientState = "";

                var results = scope.ExecuteCommand("Add-PnPWebhookSubscription",
					new CommandParameter("List", list),
					new CommandParameter("NotificationUrl", notificationUrl),
					new CommandParameter("ExpirationDate", expirationDate),
					new CommandParameter("ClientState", clientState));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            