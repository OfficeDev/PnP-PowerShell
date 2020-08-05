using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Fields
{
    [TestClass]
    public class SetViewTests
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
        public void SetPnPViewTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The Id, Title or Url of the list
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Id, Title or instance of the view
				var identity = "";
				// From Cmdlet Help: Hashtable of properties to update on the view. Use the syntax @{property1="value";property2="value"}.
				var values = "";
				// From Cmdlet Help: An array of fields to use in the view. Notice that specifying this value will remove the existing fields
				var fields = "";
				// From Cmdlet Help: A valid XML fragment containing one or more Aggregations
				var aggregations = "";

                var results = scope.ExecuteCommand("Set-PnPView",
					new CommandParameter("List", list),
					new CommandParameter("Identity", identity),
					new CommandParameter("Values", values),
					new CommandParameter("Fields", fields),
					new CommandParameter("Aggregations", aggregations));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            