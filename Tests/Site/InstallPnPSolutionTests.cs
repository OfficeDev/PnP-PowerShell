using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Site
{
    [TestClass]
    public class InstallSolutionTests
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
        public void InstallPnPSolutionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: ID of the solution, from the solution manifest
				var packageId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Path to the sandbox solution package (.WSP) file
				var sourceFilePath = "";
				// From Cmdlet Help: Optional major version of the solution, defaults to 1
				var majorVersion = "";
				// From Cmdlet Help: Optional minor version of the solution, defaults to 0
				var minorVersion = "";

                var results = scope.ExecuteCommand("Install-PnPSolution",
					new CommandParameter("PackageId", packageId),
					new CommandParameter("SourceFilePath", sourceFilePath),
					new CommandParameter("MajorVersion", majorVersion),
					new CommandParameter("MinorVersion", minorVersion));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            