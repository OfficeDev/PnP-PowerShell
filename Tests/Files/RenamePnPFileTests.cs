using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class RenameFileTests
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
        public void RenamePnPFileTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Server relative Url specifying the file to rename. Must include the file name.
				var serverRelativeUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Site relative Url specifying the file to rename. Must include the file name.
				var siteRelativeUrl = "";
				// This is a mandatory parameter
				// From Cmdlet Help: File name to rename the file to. Should only be the file name and not include the path to its location. Use Move-PnPFile to move the file to another location.
				var targetFileName = "";
				// From Cmdlet Help: If provided, if a file already exist with the provided TargetFileName, it will be overwritten. If omitted, the rename operation will be canceled if a file already exists with the TargetFileName file name.
				var overwriteIfAlreadyExists = "";
				// From Cmdlet Help: If provided, no confirmation will be requested and the action will be performed
				var force = "";

                var results = scope.ExecuteCommand("Rename-PnPFile",
					new CommandParameter("ServerRelativeUrl", serverRelativeUrl),
					new CommandParameter("SiteRelativeUrl", siteRelativeUrl),
					new CommandParameter("TargetFileName", targetFileName),
					new CommandParameter("OverwriteIfAlreadyExists", overwriteIfAlreadyExists),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            