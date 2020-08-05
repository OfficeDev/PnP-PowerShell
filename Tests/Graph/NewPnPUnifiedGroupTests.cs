using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class NewPnPUnifiedGroupTests
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
        public void NewPnPUnifiedGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Display Name of the Microsoft 365 Group
				var displayName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Description of the Microsoft 365 Group
				var description = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Mail Nickname of the Microsoft 365 Group. Cannot contain spaces.
				var mailNickname = "";
				// From Cmdlet Help: The array UPN values of the group's owners
				var owners = "";
				// From Cmdlet Help: The array UPN values of the group's members
				var members = "";
				// From Cmdlet Help: Makes the group private when selected
				var isPrivate = "";
				// From Cmdlet Help: The path to the logo file of to set
				var groupLogoPath = "";
				// From Cmdlet Help: Creates a Microsoft Teams team associated with created group
				var createTeam = "";
				// From Cmdlet Help: Specifying the Force parameter will skip the confirmation question.
				var force = "";

                var results = scope.ExecuteCommand("New-PnPUnifiedGroup",
					new CommandParameter("DisplayName", displayName),
					new CommandParameter("Description", description),
					new CommandParameter("MailNickname", mailNickname),
					new CommandParameter("Owners", owners),
					new CommandParameter("Members", members),
					new CommandParameter("IsPrivate", isPrivate),
					new CommandParameter("GroupLogoPath", groupLogoPath),
					new CommandParameter("CreateTeam", createTeam),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            