using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class GetCustomActionTests
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
        public void GetPnPCustomActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Identity of the CustomAction to return. Omit to return all CustomActions.
				var identity = "";
				// From Cmdlet Help: Scope of the CustomAction, either Web, Site or All to return both
				var scopeVar = "";
				// From Cmdlet Help: Switch parameter if an exception should be thrown if the requested CustomAction does not exist (true) or if omitted, nothing will be returned in case the CustomAction does not exist
				var throwExceptionIfCustomActionNotFound = "";

                var results = scope.ExecuteCommand("Get-PnPCustomAction",
					new CommandParameter("Identity", identity),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("ThrowExceptionIfCustomActionNotFound", throwExceptionIfCustomActionNotFound));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            