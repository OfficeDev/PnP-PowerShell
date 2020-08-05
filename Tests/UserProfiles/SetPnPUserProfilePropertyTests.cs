using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.UserProfiles
{
    [TestClass]
    public class SetUserProfilePropertyTests
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
        public void SetPnPUserProfilePropertyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com
				var account = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The property to set, for instance SPS-Skills or SPS-Location
				var propertyName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The value to set in the case of a single value property
				var value = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The values set in the case of a multi value property, e.g. "Value 1","Value 2"
				var values = "";

                var results = scope.ExecuteCommand("Set-PnPUserProfileProperty",
					new CommandParameter("Account", account),
					new CommandParameter("PropertyName", propertyName),
					new CommandParameter("Value", value),
					new CommandParameter("Values", values));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            