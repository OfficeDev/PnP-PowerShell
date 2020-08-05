using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class AddTenantThemeTests
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
        public void AddPnPTenantThemeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: If a theme is already present, specifying this will overwrite the existing theme
				var overwrite = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the theme to add or update
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The palette to add. See examples for more information.
				var palette = "";
				// This is a mandatory parameter
				// From Cmdlet Help: If the theme is inverted or not
				var isInverted = "";

                var results = scope.ExecuteCommand("Add-PnPTenantTheme",
					new CommandParameter("Overwrite", overwrite),
					new CommandParameter("Identity", identity),
					new CommandParameter("Palette", palette),
					new CommandParameter("IsInverted", isInverted));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            