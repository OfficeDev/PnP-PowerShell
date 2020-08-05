using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Fields
{
    [TestClass]
    public class SetFieldTests
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
        public void SetPnPFieldTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The list object, name or id where to update the field. If omitted the field will be updated on the web.
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The field object, internal field name (case sensitive) or field id to update
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Hashtable of properties to update on the field. Use the syntax @{property1="value";property2="value"}.
				var values = "";
				// From Cmdlet Help: If provided, the field will be updated on existing lists that use it as well. If not provided or set to $false, existing lists using the field will remain unchanged but new lists will get the updated field.
				var updateExistingLists = "";

                var results = scope.ExecuteCommand("Set-PnPField",
					new CommandParameter("List", list),
					new CommandParameter("Identity", identity),
					new CommandParameter("Values", values),
					new CommandParameter("UpdateExistingLists", updateExistingLists));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            