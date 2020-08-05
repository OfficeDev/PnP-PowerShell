using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class CopyFileTests
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
        public void CopyPnPFileTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Server relative Url specifying the file or folder to copy.
				var serverRelativeUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Site relative Url specifying the file or folder to copy.
				var sourceUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Server relative Url where to copy the file or folder to.
				var targetUrl = "";
				// From Cmdlet Help: If provided, if a file already exists at the TargetUrl, it will be overwritten. If omitted, the copy operation will be canceled if the file already exists at the TargetUrl location.
				var overwriteIfAlreadyExists = "";
				// From Cmdlet Help: If provided, no confirmation will be requested and the action will be performed
				var force = "";
				// From Cmdlet Help: If the source is a folder, the source folder name will not be created, only the contents within it.
				var skipSourceFolderName = "";

                var results = scope.ExecuteCommand("Copy-PnPFile",
					new CommandParameter("ServerRelativeUrl", serverRelativeUrl),
					new CommandParameter("SourceUrl", sourceUrl),
					new CommandParameter("TargetUrl", targetUrl),
					new CommandParameter("OverwriteIfAlreadyExists", overwriteIfAlreadyExists),
					new CommandParameter("Force", force),
					new CommandParameter("SkipSourceFolderName", skipSourceFolderName));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            