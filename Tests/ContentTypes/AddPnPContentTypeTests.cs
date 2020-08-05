using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ContentTypes
{
    [TestClass]
    public class AddContentTypeTests
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
        public void AddPnPContentTypeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specify the name of the new content type
				var name = "";
				// From Cmdlet Help: If specified, in the format of 0x0100233af432334r434343f32f3, will create a content type with the specific ID
				var contentTypeId = "";
				// From Cmdlet Help: Specifies the description of the new content type
				var description = "";
				// From Cmdlet Help: Specifies the group of the new content type
				var group = "";
				// From Cmdlet Help: Specifies the parent of the new content type
				var parentContentType = "";

                var results = scope.ExecuteCommand("Add-PnPContentType",
					new CommandParameter("Name", name),
					new CommandParameter("ContentTypeId", contentTypeId),
					new CommandParameter("Description", description),
					new CommandParameter("Group", group),
					new CommandParameter("ParentContentType", parentContentType));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            