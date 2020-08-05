using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Tenant
{
    [TestClass]
    public class SaveTenantTemplateTests
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
        public void SavePnPTenantTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Allows you to provide an in-memory instance of a Tenant Template or a filename of a template file in XML format. When using this parameter, the -Out parameter refers to the path for saving the template and storing any supporting file for the template.
				var template = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Filename to write to, optionally including full path.
				var outVar = "";
				// From Cmdlet Help: The optional schema to use when creating the PnP file. Always defaults to the latest schema.
				var schema = "";
				// From Cmdlet Help: Specifying the Force parameter will skip the confirmation question.
				var force = "";

                var results = scope.ExecuteCommand("Save-PnPTenantTemplate",
					new CommandParameter("Template", template),
					new CommandParameter("Out", outVar),
					new CommandParameter("Schema", schema),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            