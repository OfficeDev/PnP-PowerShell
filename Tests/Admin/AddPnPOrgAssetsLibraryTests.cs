using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class AddOrgAssetsLibraryTests
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
        public void AddPnPOrgAssetsLibraryTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The full url of the document library to be marked as one of organization's assets sources
				var libraryUrl = "";
				// From Cmdlet Help: The full url to an image that should be used as a thumbnail for showing this source. The image must reside in the same site as the document library you specify.
				var thumbnailUrl = "";
				// From Cmdlet Help: Indicates what type of Office 365 CDN source the document library will be added to
				var cdnType = "";

                var results = scope.ExecuteCommand("Add-PnPOrgAssetsLibrary",
					new CommandParameter("LibraryUrl", libraryUrl),
					new CommandParameter("ThumbnailUrl", thumbnailUrl),
					new CommandParameter("CdnType", cdnType));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            