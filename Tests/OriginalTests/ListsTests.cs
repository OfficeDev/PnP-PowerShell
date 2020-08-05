using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Collections;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Tests
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

                var results = scope.ExecuteCommand("Add-PnPListItem",
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

                var results = scope.ExecuteCommand("Add-PnPView",
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
                var results = scope.ExecuteCommand("Get-PnPList");

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.List));
            }
        }


        [TestMethod]
        public void GetList_ByTitle_Test()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPList",
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
                var results = scope.ExecuteCommand("Get-PnPListItem",
                    new CommandParameter("List", "PnPTestList"));

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListItem));
            }
        }

		[TestMethod]
		public void GetListItemScriptBlockTest()
		{
			int itemId;
			const string updatedItemTitle = "Test Updated";

			// Create item
			using (var ctx = TestCommon.CreateClientContext())
			{
				var list = ctx.Web.GetListByTitle("PnPTestList");
				var item = list.AddItem(new ListItemCreationInformation());
				item["Title"] = "Test";
				item.Update();
				ctx.ExecuteQueryRetry();

				// Execute Get-PnPListItem cmd-let
				using (var scope = new PSTestScope(true))
				{
					var results = scope.ExecuteCommand("Get-PnPListItem",
						new CommandParameter("List", "PnPTestList"),
						new CommandParameter("PageSize", 1),
						new CommandParameter("ScriptBlock", ScriptBlock.Create(
							"Param($items) $item = $items[0]; $item['Title'] = '" + updatedItemTitle + "'; $item.Update(); $item.Context.ExecuteQuery()")
						));
					itemId = (int)results[0].Properties["Id"].Value;
				}

				// Check that item's Title was updated
				var updatedItem = list.GetItemById(itemId);
				ctx.Load(updatedItem);
				ctx.ExecuteQueryRetry();
				Assert.IsTrue((string)updatedItem["Title"] == updatedItemTitle);
			}
		}

		[TestMethod]
		public void GetListItemByQueryScriptBlockTest()
		{
			int itemId;
			const string updatedItemTitle = "Test Updated";

			// Create item
			using (var ctx = TestCommon.CreateClientContext())
			{
				var list = ctx.Web.GetListByTitle("PnPTestList");
				var item = list.AddItem(new ListItemCreationInformation());
				item["Title"] = "Test";
				item.Update();
				ctx.ExecuteQueryRetry();

				// Execute Get-PnPListItem cmd-let
				using (var scope = new PSTestScope(true))
				{
					var results = scope.ExecuteCommand("Get-PnPListItem",
						new CommandParameter("List", "PnPTestList"),
						new CommandParameter("Query", "<View></View>"),
						new CommandParameter("PageSize", 1),
						new CommandParameter("ScriptBlock", ScriptBlock.Create(
							"Param($items) $item = $items[0]; $item['Title'] = '" + updatedItemTitle + "'; $item.Update(); $item.Context.ExecuteQuery()")
						));
					itemId = (int)results[0].Properties["Id"].Value;
				}

				// Check that item's Title was updated
				var updatedItem = list.GetItemById(itemId);
				ctx.Load(updatedItem);
				ctx.ExecuteQueryRetry();
				Assert.IsTrue((string)updatedItem["Title"] == updatedItemTitle);
			}
		}

		[TestMethod]
        public void GetViewTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPView",
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
                scope.ExecuteCommand("New-PnPList",
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
        public void NewListHiddenTest()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("New-PnPList",
                    new CommandParameter("Title", "PnPTestListHidden1"),
                    new CommandParameter("Template", ListTemplateType.GenericList),
                    new CommandParameter("Hidden"));
            }

            using (var ctx = TestCommon.CreateClientContext())
            {
                var list = ctx.Web.GetListByTitle("PnPTestListHidden1");
                Assert.IsTrue(list.Hidden);

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
                    scope.ExecuteCommand("Remove-PnPList",
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
                    scope.ExecuteCommand("Remove-PnPView",
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
                    scope.ExecuteCommand("Set-PnPList",
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

        [TestMethod]
        public void SetListHiddenTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                ctx.Web.CreateList(ListTemplateType.GenericList, "PnPTestListHidden2", false);

                using (var scope = new PSTestScope(true))
                {
                    scope.ExecuteCommand("Set-PnPList",
                        new CommandParameter("Identity", "PnPTestListHidden2"),
                        new CommandParameter("Hidden", true)
                        );
                }

                var list = ctx.Web.GetListByTitle("PnPTestListHidden2");
                Assert.IsTrue(list.Hidden);

                list.DeleteObject();
                ctx.ExecuteQueryRetry();
            }
        }
    }
}
