using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using System.Linq;
using OfficeDevPnP.Core.Enums;

namespace SharePointPnP.PowerShell.Tests
{
    [TestClass]
    public class BrandingTests
    {
        // Planning to move to Core.Constants
        private readonly Guid PUBLISHING_FEATURE_WEB = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb");
        private readonly Guid PUBLISHING_FEATURE_SITE = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa");

        [TestMethod]
        public void AddCustomActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPCustomAction",
                    new CommandParameter("Name", "TestCustomAction"),
                    new CommandParameter("Title", "TestCustomAction"),
                    new CommandParameter("Description", "Test Custom Action Description"),
                    new CommandParameter("Group", "ActionsMenu"),
                    new CommandParameter("Location", "Microsoft.SharePoint.StandardMenu")
                    );


                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.LoadQuery(context.Web.UserCustomActions.Where(ca => ca.Name == "TestCustomAction"));
                    context.ExecuteQueryRetry();

                    Assert.IsTrue(actions.Any());

                    actions.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void AddJavascriptBlockTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPJavascriptBlock",
                    new CommandParameter("Name", "TestJavascriptBlock"),
                    new CommandParameter("Script", "<script type='text/javascript'>alert('1')</script>"));


                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptBlock");
                    Assert.IsTrue(actions.Any());

                    actions.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void AddJavascriptLinkTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPJavascriptLink",
                    new CommandParameter("Key", "TestJavascriptLink"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));


                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptLink");
                    Assert.IsTrue(actions.Any());

                    actions.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void AddNavigationNodeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPNavigationNode",
                    new CommandParameter("Location", NavigationType.QuickLaunch),
                    new CommandParameter("Title", "Test Navigation Item"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));


                using (var context = TestCommon.CreateClientContext())
                {
                    var nodes = context.LoadQuery(context.Web.Navigation.QuickLaunch.Where(n => n.Title == "Test Navigation Item"));
                    context.ExecuteQueryRetry();

                    Assert.IsTrue(nodes.Any());

                    nodes.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void GetCustomActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Add-PnPJavascriptLink",
                    new CommandParameter("Key", "TestJavascriptLink"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));

                var results = scope.ExecuteCommand("Get-SPOCustomAction");

                Assert.IsTrue(results.Any());

                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptLink");

                    actions.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void GetJavaScriptLinkTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Add-PnPJavascriptLink",
                    new CommandParameter("Key", "TestJavascriptLink"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));

                var results = scope.ExecuteCommand("Get-PnPJavaScriptLink");

                Assert.IsTrue(results.Any());

                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptLink");

                    actions.FirstOrDefault().DeleteObject();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void GetProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPProvisioningTemplate");

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results.FirstOrDefault().BaseObject.GetType() == typeof(string));
            }
        }

        [TestMethod]
        public void ApplyProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Apply-PnPProvisioningTemplate",
                    new CommandParameter("Path", "..\\..\\Resources\\template.xml")
                    );


                using (var ctx = TestCommon.CreateClientContext())
                {
                    var succeeded = false;
                    try
                    {
                        var field = ctx.Web.Fields.GetByInternalNameOrTitle("PnPPowerShellTemplateTest");
                        ctx.ExecuteQueryRetry();
                        succeeded = true;
                        field.DeleteObject();
                        ctx.ExecuteQueryRetry();
                    }
                    catch { }

                    Assert.IsTrue(succeeded);
                }
            }
        }


        [TestMethod]
        public void RemoveCustomActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Add-PnPCustomAction",
                    new CommandParameter("Name", "TestCustomAction"),
                    new CommandParameter("Title", "TestCustomAction"),
                    new CommandParameter("Description", "Test Custom Action Description"),
                    new CommandParameter("Group", "ActionsMenu"),
                    new CommandParameter("Location", "Microsoft.SharePoint.StandardMenu")
                    );


                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.LoadQuery(context.Web.UserCustomActions.Where(ca => ca.Name == "TestCustomAction"));
                    context.ExecuteQueryRetry();

                    Assert.IsTrue(actions.Any());

                    var id = actions.FirstOrDefault().Id;

                    scope.ExecuteCommand("Remove-PnPCustomAction",
                        new CommandParameter("Identity", id),
                        new CommandParameter("Force", true));

                    actions = context.LoadQuery(context.Web.UserCustomActions.Where(ca => ca.Name == "TestCustomAction"));
                    context.ExecuteQueryRetry();

                    Assert.IsFalse(actions.Any());
                }
            }
        }

        [TestMethod]
        public void RemoveJavascriptLinkTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPJavascriptLink",
                    new CommandParameter("Key", "TestJavascriptLink"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));


                using (var context = TestCommon.CreateClientContext())
                {
                    var actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptLink");
                    Assert.IsTrue(actions.Any());

                    var name = actions.FirstOrDefault().Name;

                    scope.ExecuteCommand("Remove-PnPJavaScriptLink",
                        new CommandParameter("Name", name),
                        new CommandParameter("Force", true));

                    actions = context.Web.GetCustomActions().Where(c => c.Location == "ScriptLink" && c.Name == "TestJavascriptLink");
                    Assert.IsFalse(actions.Any());

                }
            }
        }

        [TestMethod]
        public void RemoveNavigationNodeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPNavigationNode",
                    new CommandParameter("Location", NavigationType.QuickLaunch),
                    new CommandParameter("Title", "Test Navigation Item"),
                    new CommandParameter("Url", "https://testserver.com/testtojavascriptlink.js"));


                using (var context = TestCommon.CreateClientContext())
                {
                    var nodes = context.LoadQuery(context.Web.Navigation.QuickLaunch.Where(n => n.Title == "Test Navigation Item"));
                    context.ExecuteQueryRetry();

                    Assert.IsTrue(nodes.Any());

                    scope.ExecuteCommand("Remove-PnPNavigationNode",
                        new CommandParameter("Location", NavigationType.QuickLaunch),
                        new CommandParameter("Title", "Test Navigation Item"),
                        new CommandParameter("Force", true)
                     );

                    nodes = context.LoadQuery(context.Web.Navigation.QuickLaunch.Where(n => n.Title == "Test Navigation Item"));
                    context.ExecuteQueryRetry();

                    Assert.IsFalse(nodes.Any());
                }
            }
        }

        [TestMethod]
        public void SetHomePageTest()
        {
            using (var context = TestCommon.CreateClientContext())
            {

                context.Load(context.Web, w => w.RootFolder.WelcomePage);
                context.ExecuteQueryRetry();
                var existingHomePageUrl = context.Web.RootFolder.WelcomePage;

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Set-PnPHomePage",
                        new CommandParameter("RootFolderRelativeUrl", "sitepages/demo.aspx"));

                    context.Load(context.Web, w => w.RootFolder.WelcomePage);
                    context.ExecuteQuery();
                    var homePageUrl = context.Web.RootFolder.WelcomePage;
                    Assert.IsTrue(homePageUrl == "sitepages/demo.aspx");


                    context.Web.RootFolder.WelcomePage = existingHomePageUrl;
                    context.Web.RootFolder.Update();
                    context.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void GetHomePageTest()
        {
            using (var context = TestCommon.CreateClientContext())
            {

                context.Load(context.Web, w => w.RootFolder.WelcomePage);
                context.ExecuteQueryRetry();
                var existingHomePageUrl = context.Web.RootFolder.WelcomePage;

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Get-PnPHomePage");

                    Assert.IsInstanceOfType(results.FirstOrDefault().BaseObject, typeof(string));
                    Assert.IsTrue((results.FirstOrDefault().BaseObject as string).ToLowerInvariant() == existingHomePageUrl.ToLowerInvariant());
                }
            }
        }

        [TestMethod]
        public void SetAvailablePageLayoutsTest()
        {
            using (var context = TestCommon.CreateClientContext())
            {
                // Arrange
                var newPageLayouts = new string[3];
                newPageLayouts[0] = "articleleft.aspx";
                newPageLayouts[1] = "articleright.aspx";
                newPageLayouts[2] = "projectpage.aspx";

                using (var scope = new PSTestScope(true))
                {
                    // Act
                    var results = scope.ExecuteCommand("Set-PnPAvailablePageLayouts",
                        new CommandParameter("PageLayouts", newPageLayouts));

                    var pageLayouts = context.Web.GetPropertyBagValueString(
                        "__PageLayouts", string.Empty);

                    // Assert
                    Assert.IsTrue(!string.IsNullOrWhiteSpace(pageLayouts));

                    foreach (var item in newPageLayouts)
                    {
                        Assert.IsTrue(pageLayouts.ToLowerInvariant().Contains(item));
                    }
                }
            }
        }

        [TestMethod]
        public void SetAllowAllPageLayoutsTest()
        {
            using (var context = TestCommon.CreateClientContext())
            {
                // Arrange

                using (var scope = new PSTestScope(true))
                {
                    // Act
                    var results = scope.ExecuteCommand("Set-PnPAvailablePageLayouts",
                        new CommandParameter("AllowAllPageLayouts"));

                    var pageLayouts = context.Web.GetPropertyBagValueString(
                        "__PageLayouts", string.Empty);

                    // Assert
                    Assert.IsTrue(string.IsNullOrWhiteSpace(pageLayouts));
                }
            }
        }

        [TestMethod]
        public void SetMasterPageTest()
        {
            using (var context = TestCommon.CreateClientContext())
            {

                context.Load(context.Web, w => w.MasterUrl, w => w.CustomMasterUrl);
                context.ExecuteQueryRetry();
                var existingMasterUrl = context.Web.MasterUrl;
                var existingCustomMasterUrl = context.Web.CustomMasterUrl;

                using (var scope = new PSTestScope(true))
                {
                    var results = scope.ExecuteCommand("Set-PnPMasterPage",
                        new CommandParameter("MasterPageServerRelativeUrl", "/sites/tests/_catalogs/default.master"),
                        new CommandParameter("CustomMasterPageServerRelativeUrl", "/sites/tests/_catalogs/custom.master"));

                    context.Load(context.Web, w => w.MasterUrl, w => w.CustomMasterUrl);
                    context.ExecuteQuery();
                    Assert.IsTrue(context.Web.MasterUrl == "/sites/tests/_catalogs/default.master");
                    Assert.IsTrue(context.Web.CustomMasterUrl == "/sites/tests/_catalogs/custom.master");

                    context.Web.MasterUrl = existingMasterUrl;
                    context.Web.CustomMasterUrl = existingCustomMasterUrl;
                    context.Web.Update();
                    context.ExecuteQueryRetry();
                }
            }
        }

        /// <summary>
        /// Sets projectpage.aspx as the default layout and checks if it is set correctly
        /// </summary>
        /// <remarks>
        /// Activates the publishing feature if not activated, then deactivates it
        /// </remarks>
        [TestMethod]
        public void SetDefaultPageLayoutTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                // Arrange
                var pageLayoutFileName = "projectpage.aspx";
                var isSiteFeatureActive = ctx.Site.IsFeatureActive(PUBLISHING_FEATURE_SITE);
                var isWebFeatureActive = ctx.Web.IsFeatureActive(PUBLISHING_FEATURE_WEB);

                if (!isSiteFeatureActive)
                {
                    ctx.Site.ActivateFeature(PUBLISHING_FEATURE_SITE);
                }

                if (!isWebFeatureActive)
                {
                    ctx.Web.ActivateFeature(PUBLISHING_FEATURE_WEB);
                }

                using (var scope = new PSTestScope(true))
                {
                    // Act
                    var results = scope.ExecuteCommand(
                        "Set-PnPDefaultPageLayout",
                        new CommandParameter("Title", pageLayoutFileName));

                    var defaultPageLayout = ctx.Web.GetPropertyBagValueString(
                        "__DefaultPageLayout",
                        string.Empty);

                    // Assert
                    Assert.AreNotEqual(defaultPageLayout, "__inherit", true); //confirm it is not set to inherit
                    Assert.IsTrue(defaultPageLayout.ToLowerInvariant().Contains(pageLayoutFileName));
                }

                // Cleanup
                if (!isWebFeatureActive)
                {
                    ctx.Web.DeactivateFeature(PUBLISHING_FEATURE_WEB);
                }

                if (!isSiteFeatureActive)
                {
                    ctx.Site.DeactivateFeature(PUBLISHING_FEATURE_SITE);
                }
            }
        }

        /// <summary>
        /// Sets projectpage.aspx as the default page layout, then resets the site to inherit,\
        /// then checks if it is set correctly
        /// </summary>
        /// <remarks>
        /// Activates the publishing feature if not activated, then deactivates it
        /// </remarks>
        [TestMethod]
        public void SetPageLayoutToInheritTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                // Arrange
                var pageLayoutFileName = "projectpage.aspx";
                var isSiteFeatureActive = ctx.Site.IsFeatureActive(PUBLISHING_FEATURE_SITE);
                var isWebFeatureActive = ctx.Web.IsFeatureActive(PUBLISHING_FEATURE_WEB);

                if (!isSiteFeatureActive)
                {
                    ctx.Site.ActivateFeature(PUBLISHING_FEATURE_SITE);
                }

                if (!isWebFeatureActive)
                {
                    ctx.Web.ActivateFeature(PUBLISHING_FEATURE_WEB);
                }

                using (var scope = new PSTestScope(true))
                {
                    // Act
                    scope.ExecuteCommand(
                        "Set-PnPDefaultPageLayout",
                        new CommandParameter("Title", pageLayoutFileName));

                    scope.ExecuteCommand(
                        "Set-PnPDefaultPageLayout",
                        new CommandParameter("InheritFromParentSite"));

                    var defaultPageLayout = ctx.Web.GetPropertyBagValueString(
                        "__DefaultPageLayout",
                        string.Empty);

                    // Assert
                    Assert.AreEqual(defaultPageLayout, "__inherit", true); //confirm it is set to inherit
                    Assert.IsFalse(defaultPageLayout.ToLowerInvariant().Contains(pageLayoutFileName));
                }

                // Cleanup
                if (!isWebFeatureActive)
                {
                    ctx.Web.DeactivateFeature(PUBLISHING_FEATURE_WEB);
                }

                if (!isSiteFeatureActive)
                {
                    ctx.Site.DeactivateFeature(PUBLISHING_FEATURE_SITE);
                }
            }
        }


        [TestMethod]
        public void SetMinimalDownloadStrategyTest()
        {
            bool isActive = false;
            using (var context = TestCommon.CreateClientContext())
            {
                isActive = context.Web.IsFeatureActive(OfficeDevPnP.Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);

                using (var scope = new PSTestScope(true))
                {
                    if (isActive)
                    {
                        // Deactivate
                        scope.ExecuteCommand("Set-PnPMinimalDownloadStrategy",
                            new CommandParameter("Off"),
                            new CommandParameter("Force"));

                    }
                    else
                    {
                        scope.ExecuteCommand("Set-PnPMinimalDownloadStrategy",
                            new CommandParameter("On"));
                    }
                }
            }
            using (var context = TestCommon.CreateClientContext())
            {
                var featureActive = context.Web.IsFeatureActive(OfficeDevPnP.Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                if (isActive)
                {
                    Assert.IsFalse(featureActive);
                    context.Web.ActivateFeature(OfficeDevPnP.Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }
                else
                {
                    Assert.IsTrue(featureActive);
                    context.Web.DeactivateFeature(OfficeDevPnP.Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID);
                }
            }
        }
    }
}

