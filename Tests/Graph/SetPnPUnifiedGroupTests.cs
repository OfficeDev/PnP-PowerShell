using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class SetUnifiedGroupTests
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
        public void SetPnPUnifiedGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Identity of the Microsoft 365 Group
				var identity = "";
				// From Cmdlet Help: The DisplayName of the group to set
				var displayName = "";
				// From Cmdlet Help: The Description of the group to set
				var description = "";
				// From Cmdlet Help: The array UPN values of owners to set to the group. Note: Will replace owners.
				var owners = "";
				// From Cmdlet Help: The array UPN values of members to set to the group. Note: Will replace members.
				var members = "";
				// From Cmdlet Help: Makes the group private when selected
				var isPrivate = "";
				// From Cmdlet Help: The path to the logo file of to set
				var groupLogoPath = "";
				// From Cmdlet Help: Creates a Microsoft Teams team associated with created group
				var createTeam = "";

                var results = scope.ExecuteCommand("Set-PnPUnifiedGroup",
					new CommandParameter("Identity", identity),
					new CommandParameter("DisplayName", displayName),
					new CommandParameter("Description", description),
					new CommandParameter("Owners", owners),
					new CommandParameter("Members", members),
					new CommandParameter("IsPrivate", isPrivate),
					new CommandParameter("GroupLogoPath", groupLogoPath),
					new CommandParameter("CreateTeam", createTeam));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            