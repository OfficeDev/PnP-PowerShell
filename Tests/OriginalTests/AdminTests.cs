using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class GetTenantSiteTests
    {
        [TestMethod]
        public void GetTenantSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPTenantSite");

                Assert.IsTrue(results.Count > 0);
            }
        }

        [TestMethod]
        public void GetTimeZoneIdTest1()
        {
            using (var scope = new PSTestScope(false))
            {
                var results = scope.ExecuteCommand("Get-PnPTimeZoneId");
                Assert.IsTrue(results.Count > 0);
            }
        }

        [TestMethod]
        public void GetTimeZoneIdTest2()
        {
            using (var scope = new PSTestScope(false))
            {
                var results = scope.ExecuteCommand("Get-PnPTimeZoneId", new CommandParameter("Match", "Stockholm"));

                Assert.IsTrue(results.Count == 1);
            }
        }

        [TestMethod]
        public void GetWebTemplatesTest()
        {
            using (var scope = new PSTestScope(true))
            {                
                var results = scope.ExecuteCommand("Get-PnPWebTemplates", new CommandParameter("Lcid", "1033"), new CommandParameter("CompatibilityLevel", "15"));

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType().Equals(typeof(Microsoft.Online.SharePoint.TenantAdministration.SPOTenantWebTemplate)));
            }
        }
    }
}
