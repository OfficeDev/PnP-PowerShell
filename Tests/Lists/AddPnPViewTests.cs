using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class AddViewTests
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
        public void AddPnPViewTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID or Url of the list.
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The title of the view.
				var title = "";
				// From Cmdlet Help: A valid CAML Query.
				var query = "";
				// This is a mandatory parameter
				// From Cmdlet Help: A list of fields to add.
				var fields = "";
				// From Cmdlet Help: The type of view to add.
				var viewType = "";
				// From Cmdlet Help: The row limit for the view. Defaults to 30.
				var rowLimit = "";
				// From Cmdlet Help: If specified, a personal view will be created.
				var personal = "";
				// From Cmdlet Help: If specified, the view will be set as the default view for the list.
				var setAsDefault = "";
				// From Cmdlet Help: If specified, the view will have paging.
				var paged = "";
				// From Cmdlet Help: A valid XML fragment containing one or more Aggregations
				var aggregations = "";

                var results = scope.ExecuteCommand("Add-PnPView",
					new CommandParameter("List", list),
					new CommandParameter("Title", title),
					new CommandParameter("Query", query),
					new CommandParameter("Fields", fields),
					new CommandParameter("ViewType", viewType),
					new CommandParameter("RowLimit", rowLimit),
					new CommandParameter("Personal", personal),
					new CommandParameter("SetAsDefault", setAsDefault),
					new CommandParameter("Paged", paged),
					new CommandParameter("Aggregations", aggregations));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            