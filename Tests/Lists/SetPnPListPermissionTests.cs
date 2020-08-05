using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class SetListPermissionTests
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
        public void SetPnPListPermissionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID or Title of the list.
				var identity = "";
				// This is a mandatory parameter
				var group = "";
				// This is a mandatory parameter
				var user = "";
				// From Cmdlet Help: The role that must be assigned to the group or user
				var addRole = "";
				// From Cmdlet Help: The role that must be removed from the group or user
				var removeRole = "";

                var results = scope.ExecuteCommand("Set-PnPListPermission",
					new CommandParameter("Identity", identity),
					new CommandParameter("Group", group),
					new CommandParameter("User", user),
					new CommandParameter("AddRole", addRole),
					new CommandParameter("RemoveRole", removeRole));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            