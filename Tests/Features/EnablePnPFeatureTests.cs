using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Features
{
    [TestClass]
    public class EnableFeatureTests
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
        public void EnablePnPFeatureTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The id of the feature to enable.
				var identity = "";
				// From Cmdlet Help: Specifies whether to overwrite an existing feature with the same feature identifier. This parameter is ignored if there are no errors.
				var force = "";
				// From Cmdlet Help: Specify the scope of the feature to activate, either Web or Site. Defaults to Web.
				var scopeVar = "";
				// From Cmdlet Help: Specify this parameter if the feature you're trying to activate is part of a sandboxed solution.
				var sandboxed = "";

                var results = scope.ExecuteCommand("Enable-PnPFeature",
					new CommandParameter("Identity", identity),
					new CommandParameter("Force", force),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("Sandboxed", sandboxed));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            