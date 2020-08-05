using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class SetTaxonomyFieldValueTests
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
        public void SetPnPTaxonomyFieldValueTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The list item to set the field value to
				var listItem = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The internal name of the field
				var internalFieldName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Id of the Term
				var termId = "";
				// From Cmdlet Help: The Label value of the term
				var label = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A path in the form of GROUPLABEL|TERMSETLABEL|TERMLABEL
				var termPath = "";
				// From Cmdlet Help: Allows you to specify terms with key value pairs that can be referred to in the template by means of the {id:label} token. See examples on how to use this parameter.
				var terms = "";

                var results = scope.ExecuteCommand("Set-PnPTaxonomyFieldValue",
					new CommandParameter("ListItem", listItem),
					new CommandParameter("InternalFieldName", internalFieldName),
					new CommandParameter("TermId", termId),
					new CommandParameter("Label", label),
					new CommandParameter("TermPath", termPath),
					new CommandParameter("Terms", terms));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            