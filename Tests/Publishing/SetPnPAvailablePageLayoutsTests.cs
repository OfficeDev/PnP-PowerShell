using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Publishing
{
    [TestClass]
    public class SetAvailablePageLayoutsTests
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
        public void SetPnPAvailablePageLayoutsTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: An array of page layout files to set as available page layouts for the site.
				var pageLayoutVars = "";
				// This is a mandatory parameter
				// From Cmdlet Help: An array of page layout files to set as available page layouts for the site.
				var allowAllPageLayoutVars = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Set the available page layouts to inherit from the parent site.
				var inheritPageLayoutVars = "";

                var results = scope.ExecuteCommand("Set-PnPAvailablePageLayouts",
					new CommandParameter("PageLayouts", pageLayoutVars),
					new CommandParameter("AllowAllPageLayouts", allowAllPageLayoutVars),
					new CommandParameter("InheritPageLayouts", inheritPageLayoutVars));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            