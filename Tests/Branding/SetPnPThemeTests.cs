using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class SetThemeTests
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
        public void SetPnPThemeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Specifies the Color Palette Url based on the site or server relative url
				var colorPaletteUrl = "";
				// From Cmdlet Help: Specifies the Font Scheme Url based on the site or server relative url
				var fontSchemeUrl = "";
				// From Cmdlet Help: Specifies the Background Image Url based on the site or server relative url
				var backgroundImageUrl = "";
				// From Cmdlet Help: true if the generated theme files should be placed in the root web, false to store them in this web. Default is false
				var shareGenerated = "";
				// From Cmdlet Help: Resets subwebs to inherit the theme from the rootweb
				var resetSubwebsToInherit = "";
				// From Cmdlet Help: Updates only the rootweb, even if subwebs are set to inherit the theme.
				var updateRootWebOnly = "";

                var results = scope.ExecuteCommand("Set-PnPTheme",
					new CommandParameter("ColorPaletteUrl", colorPaletteUrl),
					new CommandParameter("FontSchemeUrl", fontSchemeUrl),
					new CommandParameter("BackgroundImageUrl", backgroundImageUrl),
					new CommandParameter("ShareGenerated", shareGenerated),
					new CommandParameter("ResetSubwebsToInherit", resetSubwebsToInherit),
					new CommandParameter("UpdateRootWebOnly", updateRootWebOnly));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            