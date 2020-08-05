using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class ExportListToProvisioningTemplateTests
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
        public void ExportPnPListToProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specify the lists to extract, either providing their ID or their Title.
				var list = "";
				// From Cmdlet Help: Filename to write to, optionally including full path
				var outVar = "";
				// From Cmdlet Help: The schema of the output to use, defaults to the latest schema
				var schema = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";
				// From Cmdlet Help: Returns the template as an in-memory object, which is an instance of the ProvisioningTemplate type of the PnP Core Component. It cannot be used together with the -Out parameter.
				var outVarputInstance = "";

                var results = scope.ExecuteCommand("Export-PnPListToProvisioningTemplate",
					new CommandParameter("List", list),
					new CommandParameter("Out", outVar),
					new CommandParameter("Schema", schema),
					new CommandParameter("Force", force),
					new CommandParameter("OutputInstance", outVarputInstance));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            