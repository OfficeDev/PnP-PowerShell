using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class AddUserToGroupTests
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
        public void AddPnPUserToGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The login name of the user
				var loginName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The group id, group name or group object to add the user to.
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The email address of the user
				var emailAddress = "";
				var sendEmail = "";
				var emailBody = "";

                var results = scope.ExecuteCommand("Add-PnPUserToGroup",
					new CommandParameter("LoginName", loginName),
					new CommandParameter("Identity", identity),
					new CommandParameter("EmailAddress", emailAddress),
					new CommandParameter("SendEmail", sendEmail),
					new CommandParameter("EmailBody", emailBody));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            