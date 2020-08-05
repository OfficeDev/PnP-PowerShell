using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class ExportTermGroupTests
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
        public void ExportPnPTermGroupToXmlTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The ID or name of the termgroup
				var identity = "";
				// From Cmdlet Help: File to export the data to.
				var outVar = "";
				// From Cmdlet Help: If specified, a full provisioning template structure will be returned
				var fullTemplate = "";
				// From Cmdlet Help: Defaults to Unicode
				var encoding = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";

                var results = scope.ExecuteCommand("Export-PnPTermGroupToXml",
					new CommandParameter("Identity", identity),
					new CommandParameter("Out", outVar),
					new CommandParameter("FullTemplate", fullTemplate),
					new CommandParameter("Encoding", encoding),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            