using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Linq;
using System.Management.Automation.Runspaces;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System.Collections.Generic;

namespace PnP.PowerShell.Tests
{
    /// <summary>
    /// Class used to test pulling datarows from a list into a template file
    /// </summary>
    [TestClass]
    public class ListDataRowProvisioningTemplate
    {
        [TestInitialize]
        public void Initialize()
        {
            //make sure the list doesn't exist.
            Cleanup();
            using (var ctx = TestCommon.CreateClientContext())
            {
                try
                {
                    ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestList", false);
                }
                catch(Exception)
                {

                }
                var list = ctx.Web.Lists.GetByTitle("PnPTestList");
                var listFields = list.Fields;
                // add field for multichoice export test
                listFields.AddFieldAsXml($@"<Field DisplayName=""MultiChoice"" FillInChoice=""FALSE"" Format=""Dropdown"" Name=""MultiChoice"" Title=""MultiChoice"" Type=""MultiChoice"" ID=""{(Guid.NewGuid().ToString("B"))}"" StaticName=""MultiChoice"" ColName=""ntext2"" RowOrdinal=""0""> <CHOICES> <CHOICE>1</CHOICE> <CHOICE>2</CHOICE> <CHOICE>c#;3</CHOICE> <CHOICE>c#4</CHOICE> <CHOICE>c;5</CHOICE> <CHOICE>c,6</CHOICE> <CHOICE>c.7</CHOICE> <CHOICE>c-8</CHOICE> <CHOICE>cö9</CHOICE> <CHOICE>a</CHOICE> <CHOICE>b</CHOICE> <CHOICE>c</CHOICE> </CHOICES> </Field>", true, AddFieldOptions.DefaultValue);
                ctx.Load(listFields, fields => fields.Include(field => field.Title, field => field.InternalName));
                ctx.ExecuteQueryRetry();
                //Create 10 list items.
                for (var i = 0; i < 10; i++)
                {
                    var itemCreateInfo = new ListItemCreationInformation();
                    var listItem = list.AddItem(itemCreateInfo);
                    var titleField = listFields.FirstOrDefault(f => f.Title == "Title"); //resolve Field Internal Name by Title
                    listItem[titleField.InternalName] = "Item " + i.ToString();
                    listItem["MultiChoice"] = new List<string>() { "a", "b", "c" };
                    listItem.Update();
                    if (i % 2 == 0)
                    {
                        listItem.BreakRoleInheritance(true, false);
                    }
                    ctx.ExecuteQueryRetry();
                }

                for(var i = 0; i < 10; i++)
                {
                    var testFolder = list.RootFolder.CreateFolder("TestFolder"+i.ToString());
                    ctx.ExecuteQueryRetry();
                    if(i % 2 == 0)
                    {
                        var listItem = testFolder.ListItemAllFields;
                        ctx.Load(listItem);
                        ctx.ExecuteQueryRetry();

                        for(var j = 0; j < 5; j++)
                        {
                            var subFolder = testFolder.CreateFolder("subFolder" + j.ToString());
                            ctx.ExecuteQueryRetry();
                        }

                        listItem.BreakRoleInheritance(true, true);
                        ctx.ExecuteQueryRetry();
                    }
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                try
                {
                    var list = ctx.Web.GetListByTitle("PnPTestList");
                    if (list != null)
                    {
                        list.DeleteObject();
                        ctx.ExecuteQueryRetry();
                    }
                }
                catch(Exception)
                {

                }
            }
        }

        [TestMethod]
        public void GetDataRowsFromListNoFields()
        {
            using(var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>")
                        );
                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].DataRows.Count);
                } finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetDataRowsFromListWithFields()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try { 
                    string[] fields = new string[] { "Title" };
                    var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>"),
                        new CommandParameter("Fields", fields)
                        );
                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].DataRows.Count);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetDataRowsFromListWithMultiChoiceField()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    string[] fields = new string[] { "MultiChoice" };
                    var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>"),
                        new CommandParameter("Fields", fields)
                        );
                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual("a;#b;#c", template.Lists[0].DataRows[0].Values["MultiChoice"]);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetDataRowsWithSecurityFromList()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    string[] fields = new string[] { "Title" };
                    var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>"),
                        new CommandParameter("Fields", fields),
                        new CommandParameter("IncludeSecurity", true)
                        );
                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].DataRows.Count);

                    DataRow row = template.Lists[0].DataRows[0];
                    Assert.IsTrue(row.Security.RoleAssignments.Count > 0);
                    Assert.IsTrue(row.Security.ClearSubscopes == true);
                    Assert.IsTrue(row.Security.CopyRoleAssignments == false);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetFoldersFromList()
        {
            using(var scope =  new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                       new CommandParameter("Path", filePath),
                       new CommandParameter("List", "PnPTestList"),
                       new CommandParameter("Recursive", false)
                       );

                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].Folders.Count);

                    Assert.AreEqual(0, template.Lists[0].Folders[0].Folders.Count);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithRecursive()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                       new CommandParameter("Path", filePath),
                       new CommandParameter("List", "PnPTestList"),
                       new CommandParameter("Recursive", true)
                       );

                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].Folders.Count);
                    OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");
                    Assert.AreEqual(0, f.Security.RoleAssignments.Count);
                    Assert.AreEqual(5, f.Folders.Count);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithIncludeSecurity()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                       new CommandParameter("Path", filePath),
                       new CommandParameter("List", "PnPTestList"),
                       new CommandParameter("Recursive", false),
                       new CommandParameter("IncludeSecurity", true)
                       );

                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].Folders.Count);

                    OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");
                    Assert.IsTrue(f.Security.RoleAssignments.Count > 0);
                    Assert.AreEqual(0, f.Folders.Count);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithRecursiveIncludeSecurity()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                       new CommandParameter("Path", filePath),
                       new CommandParameter("List", "PnPTestList"),
                       new CommandParameter("Recursive", true),
                       new CommandParameter("IncludeSecurity", true)
                       );

                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].Folders.Count);

                    OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");

                    Assert.IsTrue(f.Security.RoleAssignments.Count > 0);
                    Assert.AreEqual(5, f.Folders.Count);
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        // note: Add-PnPDataRowsToProvisioningTemplate changes the template version to the latest; check that this does not destroy the template (it did once...)
        [TestMethod]
        public void GetDataRowsFromList_TwoTimes()
        {
            using (var scope = new PSTestScope(true))
            {
                var filePath = CreateUniqueCopyOfTemplateFile(@"Resources\PnPTestList.xml");
                try
                {
                    var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>")
                        );

                    var template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(10, template.Lists[0].DataRows.Count, "Unexpected number of rows (first run)");

                    results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                        new CommandParameter("Path", filePath),
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Query", "<View></View>")
                        );
                    template = GetTemplateFromXmlFile(filePath);
                    Assert.AreEqual(20, template.Lists[0].DataRows.Count, "Unexpected number of rows (second run)");
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        private static ProvisioningTemplate GetTemplateFromXmlFile(string filePath)
        {
            var path = System.IO.Path.GetDirectoryName(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);
            XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(path, "");
            return provider.GetTemplate(fileName);
        }

        private static string CreateUniqueCopyOfTemplateFile(string filePath)
        {
            var fullSourceFilePath = System.IO.Path.GetFullPath(filePath);
            var newTemplateFilePath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(fullSourceFilePath),
                $"template-{Guid.NewGuid().ToString()}.xml"
            );
            System.IO.File.Copy(filePath, newTemplateFilePath);
            return newTemplateFilePath;
        }
    }
}
