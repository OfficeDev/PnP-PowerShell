using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Linq;
using Core = OfficeDevPnP.Core;

namespace SharePointPnP.PowerShell.Tests
{
    [TestClass]
    public class FeatureTests
    {
        [TestMethod]
        public void DisableFeatureTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var isActive = ctx.Web.IsFeatureActive(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);

                if (!isActive)
                {
                    ctx.Web.ActivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Disable-PnPFeature",
                        new CommandParameter("Identity", Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID));
                }

                Assert.IsFalse(ctx.Web.IsFeatureActive(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID));

                if (isActive)
                {
                    ctx.Web.ActivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }
            }
        }

        [TestMethod]
        public void EnableFeatureTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var isActive = ctx.Web.IsFeatureActive(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);

                if (isActive)
                {
                    ctx.Web.DeactivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Enable-PnPFeature",
                        new CommandParameter("Identity", Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID));
                }

                Assert.IsTrue(ctx.Web.IsFeatureActive(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID));

                if (!isActive)
                {
                    ctx.Web.DeactivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }
            }
        }

        [TestMethod]
        public void GetFeatureTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var isActive = ctx.Web.IsFeatureActive(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);

                if (!isActive)
                {
                    ctx.Web.ActivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPFeature",
                        new CommandParameter("Identity", Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID));
                    Assert.IsTrue(results.Any());

                }

                if (!isActive)
                {
                    ctx.Web.DeactivateFeature(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }
            }
        }

    }
}
