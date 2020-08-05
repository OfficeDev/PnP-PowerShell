using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class GetFileTests
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
        public void GetPnPFileTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The URL (server or site relative) to the file
				var url = "";
				// From Cmdlet Help: Local path where the file should be saved
				var path = "";
				// From Cmdlet Help: Name for the local file
				var filename = "";
				// This is a mandatory parameter
				var asFile = "";
				// From Cmdlet Help: Returns the file as a listitem showing all its properties
				var asListItem = "";
				// From Cmdlet Help: If provided in combination with -AsListItem, a System.ArgumentException will be thrown if the file specified in the -Url argument does not exist. Otherwise it will return nothing instead.
				var throwExceptionIfFileNotFound = "";
				// From Cmdlet Help: Retrieve the file contents as a string
				var asString = "";
				// From Cmdlet Help: Overwrites the file if it exists.
				var force = "";
				// From Cmdlet Help: Retrieve the file contents as a file object.
				var asFileObject = "";

                var results = scope.ExecuteCommand("Get-PnPFile",
					new CommandParameter("Url", url),
					new CommandParameter("Path", path),
					new CommandParameter("Filename", filename),
					new CommandParameter("AsFile", asFile),
					new CommandParameter("AsListItem", asListItem),
					new CommandParameter("ThrowExceptionIfFileNotFound", throwExceptionIfFileNotFound),
					new CommandParameter("AsString", asString),
					new CommandParameter("Force", force),
					new CommandParameter("AsFileObject", asFileObject));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            