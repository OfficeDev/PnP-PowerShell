using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Linq;
using System.Management.Automation.Runspaces;
using OfficeDevPnP.Core.Framework.Provisioning.Model;

namespace SharePointPnP.PowerShell.Tests
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
                ctx.Load(listFields, fields => fields.Include(field => field.Title, field => field.InternalName));
                ctx.ExecuteQueryRetry();
                //Create 10 list items.
                for (var i = 0; i < 10; i++)
                {
                    var itemCreateInfo = new ListItemCreationInformation();
                    var listItem = list.AddItem(itemCreateInfo);
                    var titleField = listFields.FirstOrDefault(f => f.Title == "Title"); //resolve Field Internal Name by Title
                    listItem[titleField.InternalName] = "Item " + i.ToString();
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

        // note: Add-PnPDataRowsToProvisioningTemplate changes the template version to latest; check that this does not destroy the template (it did...)
        [TestMethod]
        public void GetDataRowsFromList_TwoTimes()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                    new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Query", "<View></View>")
                    );
                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].DataRows.Count, "Unexpected number of rows (first run)");

                results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                    new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Query", "<View></View>")
                    );
                Assert.IsTrue(results.Count > 0, "Did not get results the second time");
                template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].DataRows.Count, "Unexpected number of rows (second run)");
            }
        }

        [TestMethod]
        public void GetDataRowsFromListNoFields()
        {
            using(var scope = new PSTestScope(true))
            {
                //var template = scope.ExecuteCommand("Get-PnPProvisioningTemplate", new CommandParameter("OutputInstance", true));

                //Assert.IsTrue(template.Any());

                var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                    new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Query", "<View></View>")
                    );
                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].DataRows.Count);
               
                    
            }
        }

        [TestMethod]
        public void GetDataRowsFromListWithFields()
        {
            using (var scope = new PSTestScope(true))
            {
                //var template = scope.ExecuteCommand("Get-PnPProvisioningTemplate", new CommandParameter("OutputInstance", true));

                //Assert.IsTrue(template.Any());

                string[] fields = new string[] { "Title" };
                var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                    new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Query", "<View></View>"),
                    new CommandParameter("Fields", fields)
                    );
                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].DataRows.Count);


            }
        }



        [TestMethod]
        public void GetDataRowsWithSecurityFromList()
        {
            using (var scope = new PSTestScope(true))
            {
             
                string[] fields = new string[] { "Title" };
                var results = scope.ExecuteCommand("Add-PnPDataRowsToProvisioningTemplate",
                    new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Query", "<View></View>"),
                    new CommandParameter("Fields", fields),
                    new CommandParameter("IncludeSecurity", true)
                    );
                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].DataRows.Count);

                DataRow row = template.Lists[0].DataRows[0];
                Assert.IsTrue(row.Security.RoleAssignments.Count > 0);
                Assert.IsTrue(row.Security.ClearSubscopes == true);
                Assert.IsTrue(row.Security.CopyRoleAssignments == false);

            }
        }

        [TestMethod]
        public void GetFoldersFromList()
        {
            using(var scope =  new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                   new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                   new CommandParameter("List", "PnPTestList"),
                   new CommandParameter("Recursive", false)
                   );

                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].Folders.Count);

                Assert.AreEqual(0, template.Lists[0].Folders[0].Folders.Count);
               
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithRecursive()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                   new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                   new CommandParameter("List", "PnPTestList"),
                   new CommandParameter("Recursive", true)
                   );

                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].Folders.Count);
                OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");
                Assert.AreEqual(0, f.Security.RoleAssignments.Count);
                Assert.AreEqual(5, f.Folders.Count);
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithIncludeSecurity()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                   new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                   new CommandParameter("List", "PnPTestList"),
                   new CommandParameter("Recursive", false),
                   new CommandParameter("IncludeSecurity", true)
                   );

                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].Folders.Count);

                OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");
                Assert.IsTrue(f.Security.RoleAssignments.Count > 0);
                Assert.AreEqual(0, f.Folders.Count);
            }
        }

        [TestMethod]
        public void GetFoldersFromListWithRecursiveIncludeSecurity()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPListFoldersToProvisioningTemplate",
                   new CommandParameter("Path", @"Resources\PnPTestList.xml"),
                   new CommandParameter("List", "PnPTestList"),
                   new CommandParameter("Recursive", true),
                   new CommandParameter("IncludeSecurity", true)
                   );

                var template = results[0].BaseObject as ProvisioningTemplate;
                Assert.AreEqual(10, template.Lists[0].Folders.Count);

                OfficeDevPnP.Core.Framework.Provisioning.Model.Folder f = template.Lists[0].Folders.Find(fld => fld.Name == "TestFolder0");
               
                Assert.IsTrue(f.Security.RoleAssignments.Count > 0);
                Assert.AreEqual(5, f.Folders.Count);
            }
        }

    }
}
