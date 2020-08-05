using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.UserProfiles
{
    [TestClass]
    public class SetUserOneDriveQuotaTests
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
        public void SetPnPUserOneDriveQuotaTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com
				var account = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The quota to set on the OneDrive for Business site of the user, in bytes
				var quota = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The quota to set on the OneDrive for Business site of the user when to start showing warnings about the drive nearing being full, in bytes
				var quotaWarning = "";

                var results = scope.ExecuteCommand("Set-PnPUserOneDriveQuota",
					new CommandParameter("Account", account),
					new CommandParameter("Quota", quota),
					new CommandParameter("QuotaWarning", quotaWarning));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            