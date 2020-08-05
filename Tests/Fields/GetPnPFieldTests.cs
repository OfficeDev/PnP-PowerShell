using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Fields
{
    [TestClass]
    public class GetFieldTests
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
        public void GetPnPFieldTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The list object or name where to get the field from
				var list = "";
				// From Cmdlet Help: The field object or name to get
				var identity = "";
				// From Cmdlet Help: Filter to the specified group
				var group = "";
				// From Cmdlet Help: Search site hierarchy for fields
				var inSiteHierarchy = "";

                var results = scope.ExecuteCommand("Get-PnPField",
					new CommandParameter("List", list),
					new CommandParameter("Identity", identity),
					new CommandParameter("Group", group),
					new CommandParameter("InSiteHierarchy", inSiteHierarchy));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            