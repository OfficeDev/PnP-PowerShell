using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class SetTenantCdnPolicyTests
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
        public void SetPnPTenantCdnPolicyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The type of cdn to retrieve the policies from
				var cdnType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The type of the policy to set
				var policyType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The value of the policy to set
				var policyValue = "";

                var results = scope.ExecuteCommand("Set-PnPTenantCdnPolicy",
					new CommandParameter("CdnType", cdnType),
					new CommandParameter("PolicyType", policyType),
					new CommandParameter("PolicyValue", policyValue));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            