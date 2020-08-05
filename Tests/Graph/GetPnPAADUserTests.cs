using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class GetAADUserTests
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
        public void GetPnPAADUserTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Returns the user with the provided user id
				var identity = "";
				// From Cmdlet Help: Includes a filter to the retrieval of the users. Use OData instructions to construct the filter, i.e. "startswith(DisplayName, 'John')".
				var filter = "";
				// From Cmdlet Help: Includes a custom sorting instruction to the retrieval of the users. Use OData syntax to construct the orderby, i.e. "DisplayName desc".
				var orderBy = "";
				// From Cmdlet Help: Allows providing an array with the property names of specific properties to return. If not provided, the default properties will be returned.
				var select = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Retrieves all users and provides a SkipToken delta token to allow to query for changes since this run when querying again by adding -DeltaToken to the command
				var delta = "";
				// From Cmdlet Help: The change token provided during the previous run with -Delta to query for the changes to user objects made in Azure Active Directory since that run
				var deltaToken = "";

                var results = scope.ExecuteCommand("Get-PnPAADUser",
					new CommandParameter("Identity", identity),
					new CommandParameter("Filter", filter),
					new CommandParameter("OrderBy", orderBy),
					new CommandParameter("Select", select),
					new CommandParameter("Delta", delta),
					new CommandParameter("DeltaToken", deltaToken));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            