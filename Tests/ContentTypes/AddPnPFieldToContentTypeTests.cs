using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ContentTypes
{
    [TestClass]
    public class AddFieldToContentTypeTests
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
        public void AddPnPFieldToContentTypeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the field that needs to be added to the content type
				var field = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies which content type a field needs to be added to
				var contentType = "";
				// From Cmdlet Help: Specifies whether the field is required or not
				var required = "";
				// From Cmdlet Help: Specifies whether the field should be hidden or not
				var hidden = "";

                var results = scope.ExecuteCommand("Add-PnPFieldToContentType",
					new CommandParameter("Field", field),
					new CommandParameter("ContentType", contentType),
					new CommandParameter("Required", required),
					new CommandParameter("Hidden", hidden));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            