using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.UserProfiles
{
    [TestClass]
    public class NewUPABulkImportJobTests
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
        public void NewPnPUPABulkImportJobTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Site or server relative URL of the folder to where you want to store the import job file.
				var folder = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The local file path.
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specify user profile property mapping between the import file and UPA property names.
				var userProfilePropertyMapping = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the identifying property in your file.
				var idProperty = "";
				// From Cmdlet Help: The type of profile identifier (Email/CloudId/PrincipalName). Defaults to Email.
				var idType = "";

                var results = scope.ExecuteCommand("New-PnPUPABulkImportJob",
					new CommandParameter("Folder", folder),
					new CommandParameter("Path", path),
					new CommandParameter("UserProfilePropertyMapping", userProfilePropertyMapping),
					new CommandParameter("IdProperty", idProperty),
					new CommandParameter("IdType", idType));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            