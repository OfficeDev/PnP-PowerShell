using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Collections;
using System.Linq;

namespace OfficeDevPnP.PowerShell.Tests
{
    [TestClass]
    public class ListsTests
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestList", false);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestList");
                if (list != null)
                {
                    list.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }

                list = ctx.Web.GetListByTitle("PnPTestList2");
                if (list != null)
                {
                    list.DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        [TestMethod]
        public void AddListItemTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var values = new Hashtable();
                values.Add("Title", "Test");

                var results = scope.ExecuteCommand("Add-SPOListItem",
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Values", values));

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListItem));
            }
        }

        [TestMethod]
        public void AddViewTest()
        {
            using (var scope = new PSTestScope(true))
            {

                var results = scope.ExecuteCommand("Add-SPOView",
                    new CommandParameter("List", "PnPTestList"),
                    new CommandParameter("Title", "TestView"),
                    new CommandParameter("Fields", new[] { "Title" }));

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.View));
            }
        }

        [TestMethod]
        public void GetListTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-SPOList");

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.List));
            }
        }


        [TestMethod]
        public void GetList_ByTitle_Test()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-SPOList",
                    new CommandParameter("Identity", "PnPTestList"));

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.List));
            }
        }

        [TestMethod]
        public void GetListItemTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestList");
                var item = list.AddItem(new ListItemCreationInformation());
                item["Title"] = "Test";
                item.Update();

                ctx.ExecuteQueryRetry();

            }
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-SPOListItem",
                    new CommandParameter("List", "PnPTestList"));

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListItem));
            }
        }

        [TestMethod]
        public void GetViewTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-SPOView",
                    new CommandParameter("List", "PnPTestList")
                    );

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.View));
            }
        }

        [TestMethod]
        public void NewListTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("New-SPOList",
                    new CommandParameter("Title", "PnPTestList2"),
                    new CommandParameter("Template", ListTemplateType.GenericList));
            }

            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestList2");
                Assert.IsNotNull(list);

                list.DeleteObject();
                ctx.ExecuteQueryRetry();
            }
        }

        [TestMethod]
        public void RemoveListTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestList2", false);

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Remove-SPOList",
                        new CommandParameter("Identity", "PnPTestList2"),
                        new CommandParameter("Force")
                        );
                }

                var list = ctx.Web.GetListByTitle("PnPTestList2");
                Assert.IsNull(list);
            }
        }

        [TestMethod]
        public void RemoveViewTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestList");

                list.CreateView("TestView", ViewType.None, new[] { "Title" }, 30, false);


                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Remove-SPOView",
                        new CommandParameter("List", "PnPTestList"),
                        new CommandParameter("Identity", "TestView"),
                        new CommandParameter("Force")
                        );
                }

                var view = list.GetViewByName("TestView");

                Assert.IsNull(view);
            }
        }

        [TestMethod]
        public void SetListTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestList3", false);

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Set-SPOList",
                        new CommandParameter("Identity", "PnPTestList3"),
                        new CommandParameter("Title", "NewPnPTestList3")
                        );
                }

                var list = ctx.Web.GetListByTitle("NewPnPTestList3");
                Assert.IsNotNull(list);

                list.DeleteObject();
                ctx.ExecuteQueryRetry();
            }
        }
    }
}
