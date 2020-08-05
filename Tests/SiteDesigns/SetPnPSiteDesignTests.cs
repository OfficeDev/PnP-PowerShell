using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.SiteDesigns
{
    [TestClass]
    public class SetSiteDesignTests
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
        public void SetPnPSiteDesignTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The guid or an object representing the site design
				var identity = "";
				// From Cmdlet Help: The title of the site design
				var title = "";
				// From Cmdlet Help: An array of guids of site scripts
				var siteScriptIds = "";
				// From Cmdlet Help: The description of the site design
				var description = "";
				// From Cmdlet Help: Specifies if the site design is a default site design
				var isDefault = "";
				// From Cmdlet Help: Sets the text for the preview image
				var previewImageAltText = "";
				// From Cmdlet Help: Sets the url to the preview image
				var previewImageUrl = "";
				// From Cmdlet Help: Specifies the type of site to which this design applies
				var webTemplate = "";
				// From Cmdlet Help: Specifies the version of the design
				var version = "";

                var results = scope.ExecuteCommand("Set-PnPSiteDesign",
					new CommandParameter("Identity", identity),
					new CommandParameter("Title", title),
					new CommandParameter("SiteScriptIds", siteScriptIds),
					new CommandParameter("Description", description),
					new CommandParameter("IsDefault", isDefault),
					new CommandParameter("PreviewImageAltText", previewImageAltText),
					new CommandParameter("PreviewImageUrl", previewImageUrl),
					new CommandParameter("WebTemplate", webTemplate),
					new CommandParameter("Version", version));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            