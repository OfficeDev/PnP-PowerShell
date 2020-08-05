using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class RemoveOrgAssetsLibraryTests
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
        public void RemovePnPOrgAssetsLibraryTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The server relative url of the document library flagged as organizational asset which you want to remove, i.e. "sites/branding/logos"
				var libraryUrl = "";
				// From Cmdlet Help: Boolean indicating if the document library that will no longer be flagged as an organizational asset also needs to be removed as an Office 365 CDN source
				var shouldRemoveFromCdn = "";
				// From Cmdlet Help: Indicates what type of Office 365 CDN source the document library that will no longer be flagged as an organizational asset was of
				var cdnType = "";

                var results = scope.ExecuteCommand("Remove-PnPOrgAssetsLibrary",
					new CommandParameter("LibraryUrl", libraryUrl),
					new CommandParameter("ShouldRemoveFromCdn", shouldRemoveFromCdn),
					new CommandParameter("CdnType", cdnType));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            