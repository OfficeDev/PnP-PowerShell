using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Principals
{
    [TestClass]
    public class GetGroupTests
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
        public void GetPnPGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Get a specific group by name
				var identity = "";
				// From Cmdlet Help: Retrieve the associated member group
				var associatedMemberGroup = "";
				// From Cmdlet Help: Retrieve the associated visitor group
				var associatedVisitorGroup = "";
				// From Cmdlet Help: Retrieve the associated owner group
				var associatedOwnerGroup = "";

                var results = scope.ExecuteCommand("Get-PnPGroup",
					new CommandParameter("Identity", identity),
					new CommandParameter("AssociatedMemberGroup", associatedMemberGroup),
					new CommandParameter("AssociatedVisitorGroup", associatedVisitorGroup),
					new CommandParameter("AssociatedOwnerGroup", associatedOwnerGroup));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            