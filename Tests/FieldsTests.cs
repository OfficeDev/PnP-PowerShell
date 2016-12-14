using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using System.Linq;
using OfficeDevPnP.Core.Entities;

namespace SharePointPnP.PowerShell.Tests
{
    [TestClass]
    public class FieldsTests
    {
        [TestMethod]
        public void AddFieldTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Add-PnPField",
                        new CommandParameter("DisplayName", "PSCmdletTestField"),
                        new CommandParameter("InternalName", "PSCmdletTestField"),
                        new CommandParameter("Type", FieldType.Text),
                        new CommandParameter("Group", "Test Group"));
                }

                var succeeded = false;
                try
                {
                    var field = ctx.Web.Fields.GetByInternalNameOrTitle("PSCmdletTestField");
                    ctx.ExecuteQueryRetry();
                    succeeded = true;
                    field.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
                catch { }

                Assert.IsTrue(succeeded);
            }
        }


        [TestMethod]
        public void AddFieldFromXmlTest()
        {
            var xml = @"<Field Type=""Text"" Name=""PSCmdletTest"" DisplayName=""PSCmdletTest"" ID=""{27d81055-f208-41c9-a976-61c5473eed4a}"" Group=""Test"" Required=""FALSE"" StaticName=""PSCmdletTest"" />";

            using (var ctx = TestCommon.CreateClientContext())
            {
                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Add-PnPFieldFromXml",
                        new CommandParameter("FieldXml", xml));
                }

                var succeeded = false;
                try
                {
                    var field = ctx.Web.Fields.GetByInternalNameOrTitle("PSCmdletTest");
                    ctx.ExecuteQueryRetry();
                    succeeded = true;
                    field.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
                catch { }

                Assert.IsTrue(succeeded);
            }
        }

        [TestMethod]
        public void AddTaxonomyField()
        {

            using (var ctx = TestCommon.CreateClientContext())
            {
                // Get the first group
                var taxSession = ctx.Site.GetTaxonomySession();
                var termStore = taxSession.GetDefaultSiteCollectionTermStore();
                ctx.Load(termStore, ts => ts.Groups);
                ctx.ExecuteQueryRetry();

                var termGroup = termStore.Groups[0];
                ctx.Load(termGroup, tg => tg.TermSets);
                ctx.ExecuteQueryRetry();

                var termSet = termGroup.TermSets[0];
                ctx.Load(termSet, ts => ts.Id);
                ctx.ExecuteQueryRetry();
                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Add-PnPTaxonomyField",
                        new CommandParameter("DisplayName", "PSCmdletTestField"),
                        new CommandParameter("InternalName", "PSCmdletTestField"),
                        new CommandParameter("TaxonomyItemId", termSet.Id),
                        new CommandParameter("Group", "Test Group"));
                }

                var succeeded = false;
                try
                {
                    var field = ctx.Web.Fields.GetByInternalNameOrTitle("PSCmdletTestField");
                    ctx.ExecuteQueryRetry();
                    succeeded = true;
                    field.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
                catch { }

                Assert.IsTrue(succeeded);
            }
        }

        [TestMethod]
        public void GetFieldTest()
        {

            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPField");

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.Field));
            }
        }

        [TestMethod]
        public void GetField_ByTitle_Test()
        {

            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPField",
                    new CommandParameter("Identity", "Title"));

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.Field));
            }
        }

        [TestMethod]
        public void RemoveFieldTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                FieldCreationInformation fieldCI = new FieldCreationInformation(FieldType.Text)
                {
                    DisplayName = "PnPTestTextField",
                    Group = "PnP",
                    InternalName = "PnPTestTextField",
                    Required = false
                };

                ctx.Web.CreateField(fieldCI);

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Remove-PnPField",
                        new CommandParameter("Identity", "PnPTestTextField"),
                        new CommandParameter("Force"));

                }

                var succeeded = false;
                try
                {
                    var field = ctx.Web.Fields.GetByInternalNameOrTitle("PnPTestTextField");
                    ctx.ExecuteQueryRetry();
                }
                catch {
                    succeeded = true;
                }

                Assert.IsTrue(succeeded);

            }
        }
    }
}
