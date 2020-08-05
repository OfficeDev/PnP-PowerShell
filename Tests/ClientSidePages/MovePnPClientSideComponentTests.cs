using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class MoveClientSideWebPartTests
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
        public void MovePnPClientSideComponentTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page
				var page = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The instance id of the control. Use Get-PnPClientSideControl retrieve the instance ids.
				var instanceId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The section to move the web part to
				var section = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The column to move the web part to
				var column = "";
				// From Cmdlet Help: Change to order of the web part in the column
				var position = "";

                var results = scope.ExecuteCommand("Move-PnPClientSideComponent",
					new CommandParameter("Page", page),
					new CommandParameter("InstanceId", instanceId),
					new CommandParameter("Section", section),
					new CommandParameter("Column", column),
					new CommandParameter("Position", position));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            