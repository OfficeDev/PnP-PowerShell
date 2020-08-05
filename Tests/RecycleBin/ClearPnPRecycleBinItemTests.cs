using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.RecycleBin
{
    [TestClass]
    public class ClearRecycleBinItemTests
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
        public void ClearPnPRecycleBinItemTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Id of the recycle bin item or the recycle bin item itself to permanently delete
				var identity = "";
				// From Cmdlet Help: Clears all items
				var all = "";
				// From Cmdlet Help: If provided, only all the items in the second stage recycle bin will be cleared
				var secondStageOnly = "";
				// From Cmdlet Help: If provided, no confirmation will be asked to permanently delete the recycle bin item
				var force = "";

                var results = scope.ExecuteCommand("Clear-PnPRecycleBinItem",
					new CommandParameter("Identity", identity),
					new CommandParameter("All", all),
					new CommandParameter("SecondStageOnly", secondStageOnly),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            