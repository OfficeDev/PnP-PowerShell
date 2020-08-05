using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class SetFooterTests
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
        public void SetPnPFooterTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Indicates if the footer should be shown on the current web ($true) or if it should be hidden ($false)
				var enabled = "";
				// From Cmdlet Help: Defines how the footer should look like
				var layoutVar = "";
				// From Cmdlet Help: Defines the background emphasis of the content in the footer
				var backgroundTheme = "";
				// From Cmdlet Help: Defines the title displayed in the footer
				var title = "";
				// From Cmdlet Help: Defines the server relative URL to the logo to be displayed in the footer. Provide an empty string to remove the current logo.
				var logoUrl = "";

                var results = scope.ExecuteCommand("Set-PnPFooter",
					new CommandParameter("Enabled", enabled),
					new CommandParameter("Layout", layoutVar),
					new CommandParameter("BackgroundTheme", backgroundTheme),
					new CommandParameter("Title", title),
					new CommandParameter("LogoUrl", logoUrl));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            