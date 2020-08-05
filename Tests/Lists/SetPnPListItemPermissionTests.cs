using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class SetListItemPermissionTests
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
        public void SetPnPListItemPermissionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID, Title or Url of the list.
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The ID of the listitem, or actual ListItem object
				var identity = "";
				// This is a mandatory parameter
				var group = "";
				// This is a mandatory parameter
				var user = "";
				// From Cmdlet Help: The role that must be assigned to the group or user
				var addRole = "";
				// From Cmdlet Help: The role that must be removed from the group or user
				var removeRole = "";
				// From Cmdlet Help: Clear all existing permissions
				var clearExisting = "";
				// From Cmdlet Help: Inherit permissions from the list, removing unique permissions
				var inheritPermissions = "";
				// From Cmdlet Help: Update the item permissions without creating a new version or triggering MS Flow.
				var systemUpdate = "";

                var results = scope.ExecuteCommand("Set-PnPListItemPermission",
					new CommandParameter("List", list),
					new CommandParameter("Identity", identity),
					new CommandParameter("Group", group),
					new CommandParameter("User", user),
					new CommandParameter("AddRole", addRole),
					new CommandParameter("RemoveRole", removeRole),
					new CommandParameter("ClearExisting", clearExisting),
					new CommandParameter("InheritPermissions", inheritPermissions),
					new CommandParameter("SystemUpdate", systemUpdate));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            