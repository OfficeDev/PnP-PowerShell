using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class UpdateSiteClassificationTests
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
        public void UpdatePnPSiteClassificationTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: A settings object retrieved by Get-PnPSiteClassification
				var settings = "";
				// From Cmdlet Help: A list of classifications, separated by commas. E.g. "HBI","LBI","Top Secret"
				var classifications = "";
				// From Cmdlet Help: The default classification to be used. The value needs to be present in the list of possible classifications
				var defaultClassification = "";
				// From Cmdlet Help: The UsageGuidelinesUrl. Set to "" to clear.
				var usageGuidelinesUrl = "";

                var results = scope.ExecuteCommand("Update-PnPSiteClassification",
					new CommandParameter("Settings", settings),
					new CommandParameter("Classifications", classifications),
					new CommandParameter("DefaultClassification", defaultClassification),
					new CommandParameter("UsageGuidelinesUrl", usageGuidelinesUrl));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            