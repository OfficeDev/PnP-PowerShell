using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class NewGroupTests
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
        public void NewPnPGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Title of the group
				var title = "";
				// From Cmdlet Help: The description for the group
				var description = "";
				// From Cmdlet Help: The owner for the group, which can be a user or another group
				var owner = "";
				// From Cmdlet Help: A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group
				var allowRequestToJoinLeave = "";
				// From Cmdlet Help: A switch parameter that specifies whether users are automatically added or removed when they make a request
				var autoAcceptRequestToJoinLeave = "";
				// From Cmdlet Help: A switch parameter that specifies whether group members can modify membership in the group
				var allowMembersEditMembership = "";
				// From Cmdlet Help: A switch parameter that specifies whether only group members are allowed to view the list of members in the group
				var onlyAllowMembersViewMembership = "";
				// From Cmdlet Help: A switch parameter that disallows group members to view membership.
				var disallowMembersViewMembership = "";
				// From Cmdlet Help: The e-mail address to which membership requests are sent
				var requestToJoinEmail = "";
				var setAssociatedGroup = "";

                var results = scope.ExecuteCommand("New-PnPGroup",
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Owner", owner),
					new CommandParameter("AllowRequestToJoinLeave", allowRequestToJoinLeave),
					new CommandParameter("AutoAcceptRequestToJoinLeave", autoAcceptRequestToJoinLeave),
					new CommandParameter("AllowMembersEditMembership", allowMembersEditMembership),
					new CommandParameter("OnlyAllowMembersViewMembership", onlyAllowMembersViewMembership),
					new CommandParameter("DisallowMembersViewMembership", disallowMembersViewMembership),
					new CommandParameter("RequestToJoinEmail", requestToJoinEmail),
					new CommandParameter("SetAssociatedGroup", setAssociatedGroup));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            