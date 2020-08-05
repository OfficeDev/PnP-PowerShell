using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webs
{
    [TestClass]
    public class GetWebTemplatesTests
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
        public void GetPnPWebTemplatesTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The language ID. For instance: 1033 for English. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: Get-PnPAvailableLanguage.
				var lcid = "";
				// From Cmdlet Help: The compatibily level of SharePoint where 14 is SharePoint 2010, 15 is SharePoint 2013 and 16 is SharePoint 2016 and later including SharePoint Online
				var compatibilityLevel = "";

                var results = scope.ExecuteCommand("Get-PnPWebTemplates",
					new CommandParameter("Lcid", lcid),
					new CommandParameter("CompatibilityLevel", compatibilityLevel));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            