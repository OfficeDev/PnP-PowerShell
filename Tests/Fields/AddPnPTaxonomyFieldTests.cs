using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Fields
{
    [TestClass]
    public class AddTaxonomyFieldTests
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
        public void AddPnPTaxonomyFieldTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The list object or name where this field needs to be added
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The display name of the field
				var displayName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The internal name of the field
				var internalName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The path to the term that this needs to be bound
				var termSetPath = "";
				// From Cmdlet Help: The ID of the Taxonomy item
				var taxonomyItemId = "";
				// From Cmdlet Help: The path delimiter to be used, by default this is '|'
				var termPathDelimiter = "";
				// From Cmdlet Help: The group name to where this field belongs to
				var group = "";
				// From Cmdlet Help: The ID for the field, must be unique
				var id = "";
				// From Cmdlet Help: Switch Parameter if this field must be added to the default view
				var addToDefaultView = "";
				// From Cmdlet Help: Switch Parameter if this Taxonomy field can hold multiple values
				var multiValue = "";
				// From Cmdlet Help: Switch Parameter if the field is a required field
				var required = "";
				// From Cmdlet Help: Specifies the control settings while adding a field. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.addfieldoptions.aspx for details
				var fieldOptions = "";

                var results = scope.ExecuteCommand("Add-PnPTaxonomyField",
					new CommandParameter("List", list),
					new CommandParameter("DisplayName", displayName),
					new CommandParameter("InternalName", internalName),
					new CommandParameter("TermSetPath", termSetPath),
					new CommandParameter("TaxonomyItemId", taxonomyItemId),
					new CommandParameter("TermPathDelimiter", termPathDelimiter),
					new CommandParameter("Group", group),
					new CommandParameter("Id", id),
					new CommandParameter("AddToDefaultView", addToDefaultView),
					new CommandParameter("MultiValue", multiValue),
					new CommandParameter("Required", required),
					new CommandParameter("FieldOptions", fieldOptions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            