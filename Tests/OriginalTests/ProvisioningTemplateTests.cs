using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Xml;

namespace PnP.PowerShell.Tests
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
        public void ValidateNumberOfContentTypes()
        {
            using (var scope = new PSTestScope(true))
            {
                // Arrange
                var results = scope.ExecuteCommand("Get-PnPProvisioningTemplate",
                    new CommandParameter("ContentTypeGroups", ContentTypeGroupName));
                var doc = new XmlDocument();
                doc.LoadXml(results[0].BaseObject.ToString());

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("pnp", "http://schemas.dev.office.com/PnP/2016/05/ProvisioningSchema");

                var xpath = "/pnp:Provisioning/pnp:Templates[1]/pnp:ProvisioningTemplate[1]/pnp:ContentTypes/pnp:ContentType";
                var attributeName = "Name";
                var root = doc.DocumentElement;

                // Act
                var elements = root.SelectNodes(xpath, nsmgr);
                var name1 = elements[0].Attributes[attributeName].Value;
                var name2 = elements[1].Attributes[attributeName].Value;

                // Assert
                Assert.AreEqual(2, elements.Count);
                Assert.AreEqual(ContentTypeName1, name1);
                Assert.AreEqual(ContentTypeName2, name2);
            }
        }
    }
}