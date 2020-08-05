using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class GetAlertTests
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
        public void GetPnPAlertTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The ID, Title or Url of the list.
				var list = "";
				// From Cmdlet Help: User to retrieve the alerts for (User ID, login name or actual User object). Skip this parameter to retrieve the alerts for the current user. Note: Only site owners can retrieve alerts for other users.
				var user = "";
				// From Cmdlet Help: Retrieve alerts with this title. Title comparison is case sensitive.
				var title = "";

                var results = scope.ExecuteCommand("Get-PnPAlert",
					new CommandParameter("List", list),
					new CommandParameter("User", user),
					new CommandParameter("Title", title));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            