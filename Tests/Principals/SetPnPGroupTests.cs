using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class SetGroupTests
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
        public void SetPnPGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: A group object, an ID or a name of a group
				var identity = "";
				// From Cmdlet Help: One of the associated group types (Visitors, Members, Owners
				var setAssociatedGroup = "";
				// From Cmdlet Help: Name of the permission set to add to this SharePoint group
				var addRole = "";
				// From Cmdlet Help: Name of the permission set to remove from this SharePoint group
				var removeRole = "";
				// From Cmdlet Help: The title for the group
				var title = "";
				// From Cmdlet Help: The owner for the group, which can be a user or another group
				var owner = "";
				// From Cmdlet Help: The description for the group
				var description = "";
				// From Cmdlet Help: A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group
				var allowRequestToJoinLeave = "";
				// From Cmdlet Help: A switch parameter that specifies whether users are automatically added or removed when they make a request
				var autoAcceptRequestToJoinLeave = "";
				// From Cmdlet Help: A switch parameter that specifies whether group members can modify membership in the group
				var allowMembersEditMembership = "";
				// From Cmdlet Help: A switch parameter that specifies whether only group members are allowed to view the list of members in the group
				var onlyAllowMembersViewMembership = "";
				// From Cmdlet Help: The e-mail address to which membership requests are sent
				var requestToJoinEmail = "";

                var results = scope.ExecuteCommand("Set-PnPGroup",
					new CommandParameter("Identity", identity),
					new CommandParameter("SetAssociatedGroup", setAssociatedGroup),
					new CommandParameter("AddRole", addRole),
					new CommandParameter("RemoveRole", removeRole),
					new CommandParameter("Title", title),
					new CommandParameter("Owner", owner),
					new CommandParameter("Description", description),
					new CommandParameter("AllowRequestToJoinLeave", allowRequestToJoinLeave),
					new CommandParameter("AutoAcceptRequestToJoinLeave", autoAcceptRequestToJoinLeave),
					new CommandParameter("AllowMembersEditMembership", allowMembersEditMembership),
					new CommandParameter("OnlyAllowMembersViewMembership", onlyAllowMembersViewMembership),
					new CommandParameter("RequestToJoinEmail", requestToJoinEmail));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            