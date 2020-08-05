using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class AddAlertTests
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
        public void AddPnPAlertTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID, Title or Url of the list.
				var list = "";
				// From Cmdlet Help: Alert title
				var title = "";
				// From Cmdlet Help: User to create the alert for (User ID, login name or actual User object). Skip this parameter to create an alert for the current user. Note: Only site owners can create alerts for other users.
				var user = "";
				// From Cmdlet Help: Alert delivery method
				var deliveryMethod = "";
				// From Cmdlet Help: Alert change type
				var changeType = "";
				// From Cmdlet Help: Alert frequency
				var frequency = "";
				// From Cmdlet Help: Alert filter
				var filter = "";
				// From Cmdlet Help: Alert time (if frequency is not immediate)
				var time = "";

                var results = scope.ExecuteCommand("Add-PnPAlert",
					new CommandParameter("List", list),
					new CommandParameter("Title", title),
					new CommandParameter("User", user),
					new CommandParameter("DeliveryMethod", deliveryMethod),
					new CommandParameter("ChangeType", changeType),
					new CommandParameter("Frequency", frequency),
					new CommandParameter("Filter", filter),
					new CommandParameter("Time", time));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            