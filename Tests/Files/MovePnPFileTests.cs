using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class MoveFileTests
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
        public void MovePnPFileTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Server relative Url specifying the file to move. Must include the file name.
				var serverRelativeUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Site relative Url specifying the file to move. Must include the file name.
				var siteRelativeUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Server relative Url where to move the file to. Must include the file name.
				var targetUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Server relative url of a document library where to move the file to. Must not include the file name.
				var targetServerRelativeLibrary = "";
				// From Cmdlet Help: If provided, if a file already exists at the TargetUrl, it will be overwritten. If omitted, the move operation will be canceled if the file already exists at the TargetUrl location.
				var overwriteIfAlreadyExists = "";
				// From Cmdlet Help: If provided and the target document library specified using TargetServerRelativeLibrary has different fields than the document library where the document is being moved from, the move will succeed. If not provided, it will fail to protect against data loss of metadata stored in fields that cannot be moved along.
				var allowSchemaMismatch = "";
				// From Cmdlet Help: If provided and the target document library specified using TargetServerRelativeLibrary is configured to keep less historical versions of documents than the document library where the document is being moved from, the move will succeed. If not provided, it will fail to protect against data loss of historical versions that cannot be moved along.
				var allowSmallerVersionLimitOnDestination = "";
				// From Cmdlet Help: If provided, only the latest version of the document will be moved and its history will be discared. If not provided, all historical versions will be moved along.
				var ignoreVersionHistory = "";
				// From Cmdlet Help: If provided, no confirmation will be requested and the action will be performed
				var force = "";

                var results = scope.ExecuteCommand("Move-PnPFile",
					new CommandParameter("ServerRelativeUrl", serverRelativeUrl),
					new CommandParameter("SiteRelativeUrl", siteRelativeUrl),
					new CommandParameter("TargetUrl", targetUrl),
					new CommandParameter("TargetServerRelativeLibrary", targetServerRelativeLibrary),
					new CommandParameter("OverwriteIfAlreadyExists", overwriteIfAlreadyExists),
					new CommandParameter("AllowSchemaMismatch", allowSchemaMismatch),
					new CommandParameter("AllowSmallerVersionLimitOnDestination", allowSmallerVersionLimitOnDestination),
					new CommandParameter("IgnoreVersionHistory", ignoreVersionHistory),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            