using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class GetUserTests
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
        public void GetPnPUserTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: User ID or login name
				var identity = "";
				// From Cmdlet Help: If provided, only users that currently have any kinds of access rights assigned to the current site collection will be returned. Otherwise all users, even those who previously had rights assigned, but not anymore at the moment, will be returned as the information is pulled from the User Information List. Only works if you don't provide an -Identity.
				var withRightsAssigned = "";

                var results = scope.ExecuteCommand("Get-PnPUser",
					new CommandParameter("Identity", identity),
					new CommandParameter("WithRightsAssigned", withRightsAssigned));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            