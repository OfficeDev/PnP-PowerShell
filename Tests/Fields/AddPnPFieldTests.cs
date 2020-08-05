using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Fields
{
    [TestClass]
    public class AddFieldTests
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
        public void AddPnPFieldTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The name of the list, its ID or an actual list object where this field needs to be added
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the field, its ID or an actual field object that needs to be added
				var field = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The display name of the field
				var displayName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The internal name of the field
				var internalName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The type of the field like Choice, Note, MultiChoice. For a complete list of field types visit https://docs.microsoft.com/dotnet/api/microsoft.sharepoint.client.fieldtype
				var type = "";
				// From Cmdlet Help: The ID of the field, must be unique
				var id = "";
				// From Cmdlet Help: Switch Parameter if this field must be added to the default view
				var addToDefaultView = "";
				// From Cmdlet Help: Switch Parameter if the field is a required field
				var required = "";
				// From Cmdlet Help: The group name to where this field belongs to
				var group = "";
				// From Cmdlet Help: The Client Side Component Id to set to the field
				var clientSideComponentId = "";
				// From Cmdlet Help: The Client Side Component Properties to set to the field
				var clientSideComponentProperties = "";

                var results = scope.ExecuteCommand("Add-PnPField",
					new CommandParameter("List", list),
					new CommandParameter("Field", field),
					new CommandParameter("DisplayName", displayName),
					new CommandParameter("InternalName", internalName),
					new CommandParameter("Type", type),
					new CommandParameter("Id", id),
					new CommandParameter("AddToDefaultView", addToDefaultView),
					new CommandParameter("Required", required),
					new CommandParameter("Group", group),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("ClientSideComponentProperties", clientSideComponentProperties));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            