using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.DocumentSets
{
    [TestClass]
    public class SetFieldInDocumentSetTests
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
        public void SetPnPDocumentSetFieldTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The document set in which to set the field. Either specify a name, a document set template object, an id, or a content type object
				var documentSet = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The field to set. The field needs to be available in one of the available content types. Either specify a name, an id or a field object
				var field = "";
				// From Cmdlet Help: Set the field as a Shared Field
				var setSharedField = "";
				// From Cmdlet Help: Set the field as a Welcome Page field
				var setWelcomePageField = "";
				// From Cmdlet Help: Removes the field as a Shared Field
				var removeSharedField = "";
				// From Cmdlet Help: Removes the field as a Welcome Page Field
				var removeWelcomePageField = "";

                var results = scope.ExecuteCommand("Set-PnPDocumentSetField",
					new CommandParameter("DocumentSet", documentSet),
					new CommandParameter("Field", field),
					new CommandParameter("SetSharedField", setSharedField),
					new CommandParameter("SetWelcomePageField", setWelcomePageField),
					new CommandParameter("RemoveSharedField", removeSharedField),
					new CommandParameter("RemoveWelcomePageField", removeWelcomePageField));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            