using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.DocumentSets
{
    [TestClass]
    public class RemoveContentTypeFromDocumentSetTests
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
        public void RemovePnPContentTypeFromDocumentSetTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The content type to remove. Either specify name, an id, or a content type object.
				var contentType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The document set to remove the content type from. Either specify a name, a document set template object, an id, or a content type object
				var documentSet = "";

                var results = scope.ExecuteCommand("Remove-PnPContentTypeFromDocumentSet",
					new CommandParameter("ContentType", contentType),
					new CommandParameter("DocumentSet", documentSet));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            