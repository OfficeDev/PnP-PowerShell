using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Site
{
    [TestClass]
    public class AddRoleDefinitionTests
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
        public void AddPnPRoleDefinitionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Name of new permission level.
				var roleName = "";
				// From Cmdlet Help: An existing permission level or the name of an permission level to clone as base template.
				var clone = "";
				// From Cmdlet Help: Specifies permission flags(s) to enable. Please visit https://docs.microsoft.com/previous-versions/office/sharepoint-csom/ee536458(v%3Doffice.15) for the PermissionKind enum
				var include = "";
				// From Cmdlet Help: Specifies permission flags(s) to disable. Please visit https://docs.microsoft.com/previous-versions/office/sharepoint-csom/ee536458(v%3Doffice.15) for the PermissionKind enum
				var exclude = "";
				// From Cmdlet Help: Optional description for the new permission level.
				var description = "";

                var results = scope.ExecuteCommand("Add-PnPRoleDefinition",
					new CommandParameter("RoleName", roleName),
					new CommandParameter("Clone", clone),
					new CommandParameter("Include", include),
					new CommandParameter("Exclude", exclude),
					new CommandParameter("Description", description));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            