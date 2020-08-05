using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ContentTypes
{
    [TestClass]
    public class RemoveFieldFromContentTypeTests
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
        public void RemovePnPFieldFromContentTypeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The field to remove
				var field = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The content type where the field is to be removed from
				var contentType = "";
				// From Cmdlet Help: If specified, inherited content types will not be updated
				var doNotUpdateChildren = "";

                var results = scope.ExecuteCommand("Remove-PnPFieldFromContentType",
					new CommandParameter("Field", field),
					new CommandParameter("ContentType", contentType),
					new CommandParameter("DoNotUpdateChildren", doNotUpdateChildren));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            