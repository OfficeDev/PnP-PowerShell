using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class NewGraphSubscriptionTests
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
        public void NewPnPGraphSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The event(s) the subscription should trigger on
				var changeType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The URL that should be called when an event matching this subscription occurs
				var notificationUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The resource to monitor for changes. See https://docs.microsoft.com/graph/api/subscription-post-subscriptions#resources-examples for the list with supported options.
				var resource = "";
				// From Cmdlet Help: The datetime defining how long this subscription should stay alive before which it needs to get extended to stay alive. See https://docs.microsoft.com/graph/api/resources/subscription#maximum-length-of-subscription-per-resource-type for the supported maximum lifetime of the subscriber endpoints.
				var expirationDateTime = "";
				// From Cmdlet Help: Specifies the value of the clientState property sent by the service in each notification. The maximum length is 128 characters. The client can check that the notification came from the service by comparing the value of the clientState property sent with the subscription with the value of the clientState property received with each notification.
				var clientState = "";
				// From Cmdlet Help: Specifies the latest version of Transport Layer Security (TLS) that the notification endpoint, specified by NotificationUrl, supports. If not provided, TLS 1.2 will be assumed.
				var latestSupportedTlsVersion = "";

                var results = scope.ExecuteCommand("New-PnPGraphSubscription",
					new CommandParameter("ChangeType", changeType),
					new CommandParameter("NotificationUrl", notificationUrl),
					new CommandParameter("Resource", resource),
					new CommandParameter("ExpirationDateTime", expirationDateTime),
					new CommandParameter("ClientState", clientState),
					new CommandParameter("LatestSupportedTlsVersion", latestSupportedTlsVersion));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            