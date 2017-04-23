using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using OfficeDevPnP.Core.Framework.Provisioning.Model;

namespace SharePointPnP.PowerShell.Tests
{
    [TestClass]
    public class ProvisioningTemplateTests
    {
        private const string ContentTypeGroupName = "Provisioning Template Tests Group";
        private const string ContentTypeName1 = "ProvisioningTemplateTests1";
        private const string ContentTypeName2 = "ProvisioningTemplateTests2";

        [TestInitialize]
        public void Initialize()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var cts = new List<string>() { ContentTypeName1, ContentTypeName2 };
                cts.ForEach(ctName =>
                {
                    if (ctx.Web.ContentTypeExistsByName(ctName))
                    {
                        var ct = ctx.Web.GetContentTypeByName(ctName);
                        ct.DeleteObject();
                        ctx.ExecuteQueryRetry();
                    }
                    ctx.Web.CreateContentType(ctName, null, ContentTypeGroupName);
                });
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var cts = new List<string>() { ContentTypeName1, ContentTypeName2 };
                cts.ForEach(ctName =>
                {
                    if (ctx.Web.ContentTypeExistsByName(ctName))
                    {
                        var ct = ctx.Web.GetContentTypeByName(ctName);
                        ct.DeleteObject();
                    }
                });
                ctx.ExecuteQueryRetry();
            }
        }

        [TestMethod]
        public void ValidatNumberOfContentTypes()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPProvisioningTemplate",
                    new CommandParameter("ContentTypeGroups", ContentTypeGroupName));

                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.IsNotNull(template);
            }
        }
    }
}