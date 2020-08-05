using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Collections;
using System.Linq;
using PnP.PowerShell.Tests;
using System.Collections.Generic;
using PnP.PowerShell.Commands.InvokeAction;
using System.Threading;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class InvokeWebActionTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Cleanup();

            using (var ctx = TestCommon.CreateClientContext())
            {
                AddList(ctx.Web, "PnPTestList1", 2, "Test1");
                AddList(ctx.Web, "PnPTestList2", 3, "Test2");
                AddList(ctx.Web, "PnPTestList3", 0, "Test3");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                IEnumerable<string> listNames = new[]
                {
                    "PnPTestList1",
                    "PnPTestList2",
                    "PnPTestList3"
                };

                foreach (var listName in listNames)
                {
                    var list = ctx.Web.GetListByTitle(listName);
                    if (list != null)
                    {
                        list.DeleteObject();
                        ctx.ExecuteQueryRetry();
                    }
                }
            }
        }

        private void AddList(Web web, string listName, int listItemCount, string listItemPrefix)
        {
            List list = web.CreateList(ListTemplateType.GenericList, listName, false);

            for (int i = 1; i <= listItemCount; i++)
            {
                AddListItem(list, listItemPrefix + "-" + i);
            }
        }

        public void SetupSubWebs(Web web, bool addLists)
        {
            DeleteSubWebs(web);
            web.Context.Load(web, item => item.WebTemplate, item => item.Language);
            web.Context.ExecuteQueryRetry();

            //Level 1
            //  1-1
            //      1-1-1
            //      1-1-2
            //  1-2
            //      1-2-1
            //          1-2-1-1
            //  1-3

            //Web is Level 1
            Web web1 = web;

            //Level 2
            Web Web1_1 = web1.CreateWeb("SubWeb-1-1", "SubWeb-1-1", "PnPTest", web.WebTemplate, (int)web.Language);

            //Level 3
            Web Web1_1_1 = Web1_1.CreateWeb("SubWeb-1-1-1", "SubWeb-1-1-1", "PnPTest", web.WebTemplate, (int)web.Language);
            Web Web1_1_2 = Web1_1.CreateWeb("SubWeb-1-1-2", "SubWeb-1-1-2", "PnPTest", web.WebTemplate, (int)web.Language);

            //Level2
            Web Web1_2 = web1.CreateWeb("SubWeb-1-2", "SubWeb-1-2", "PnPTest", web.WebTemplate, (int)web.Language);

            //Level 3
            Web Web1_2_1 = Web1_2.CreateWeb("SubWeb-1-2-1", "SubWeb-1-2-1", "PnPTest", web.WebTemplate, (int)web.Language);

            //Level 4
            Web Web1_2_1_1 = Web1_2_1.CreateWeb("SubWeb-1-1", "SubWeb-1-2-1-1", "PnPTest", web.WebTemplate, (int)web.Language);

            //Level2
            Web Web1_3 = web1.CreateWeb("SubWeb-1-3", "SubWeb-1-3", "PnPTest", web.WebTemplate, (int)web.Language);

            if(addLists)
            {
                AddList(Web1_1, "PnPTestList1-1", 3, "Test2-1");
                AddList(Web1_1_1, "PnPTestList1-1-1", 3, "Test2-1-1");
                AddList(Web1_1_2, "PnPTestList1-1-2", 3, "Test2-1-2");
                AddList(Web1_2, "PnPTestList1-2", 3, "Test2-2");
                AddList(Web1_2_1, "PnPTestList1-2-1", 3, "Test2-2-1");
                AddList(Web1_2_1_1, "PnPTestList1-2-1-1", 3, "Test2-2-1-1");
                AddList(Web1_3, "PnPTestList1-3", 3, "Test2-3");
            }
        }

        private void DeleteSubWebs(Web web)
        {
            web.Context.Load(web, item => item.Webs);
            web.Context.ExecuteQueryRetry();

            for (int i = web.Webs.Count - 1 ; i >= 0; i--)
            {
                Web subWeb = web.Webs[i];
                DeleteSubWebs(subWeb);
            }

            if (web.IsSubSite())
            {
                web.DeleteObject();
                web.Context.ExecuteQueryRetry();
            }
        }

        private void AddListItem(List list, string title)
        {
            var item = list.AddItem(new ListItemCreationInformation());
            item["Title"] = title;
            item.Update();

            list.Context.ExecuteQueryRetry();
        }

        [TestMethod]
        public void InvokeWebActionWebAction()
        {
            using (var scope = new PSTestScope(true))
            {
                Guid id = Guid.Empty;

                Action<Web> webAction = web =>
                {
                    id = web.Id;
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("WebAction", webAction),
                    new CommandParameter("WebProperties", new[] { "Id" })
                );

                Assert.IsTrue(id != Guid.Empty, "Id is empty");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionPostWebAction()
        {
            using (var scope = new PSTestScope(true))
            {
                Guid id = Guid.Empty;

                Action<Web> webPostAction = web =>
                {
                    id = web.Id;
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("PostWebAction", webPostAction),
                    new CommandParameter("WebProperties", new[] { "Id" })
                );

                Assert.IsTrue(id != Guid.Empty, "Id is empty");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedPostWebCount: 1
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionWebActionWithSubWebs()
        {
            using (var scope = new PSTestScope(true))
            {
                List<Guid> ids = new List<Guid>();
                List<string> titles = new List<string>();

                using (var context = TestCommon.CreateClientContext())
                {
                    SetupSubWebs(context.Web, false);
                }

                Action<Web> webAction = web =>
                {
                    ids.Add(web.Id);
                    titles.Add(web.Title);
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("WebAction", webAction),
                    new CommandParameter("WebProperties", new[] { "Id", "Title" }),
                    new CommandParameter("SubWebs", true)
                );

                using (var context = TestCommon.CreateClientContext())
                {
                    DeleteSubWebs(context.Web);
                }

                Assert.IsTrue(ids.Count == 8, "Wrong count on ids");

                foreach (var item in ids)
                {
                    Assert.IsTrue(item != Guid.Empty, "Id is empty");
                }

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 8
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionWebActionWithMultipleWebs()
        {
            using (var scope = new PSTestScope(true))
            {
                List<ClientContext> clientContexts = new List<ClientContext>()
                {
                    TestCommon.CreateClientContext(),
                    TestCommon.CreateClientContext(),
                    TestCommon.CreateClientContext()
                };
                
                Web[] webs = clientContexts.Select(item => item.Web).ToArray();

                List<Guid> ids = new List<Guid>();
                List<string> titles = new List<string>();

                Action<Web> webAction = web =>
                {
                    ids.Add(web.Id);
                    titles.Add(web.Title);
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("Webs", webs),
                    new CommandParameter("WebAction", webAction),
                    new CommandParameter("WebProperties", new[] { "Id", "Title" })
                );

                foreach (var clientContext in clientContexts)
                {
                    clientContext.Dispose();
                }

                Assert.IsTrue(ids.Count == webs.Length, "Id counts does not match web count");

                for (int i = 0; i < webs.Length; i++)
                {
                    Assert.IsTrue(ids[i] != Guid.Empty, "id is Empty");
                    Assert.IsTrue(ids[i] == webs[i].Id, "ids are not in correct order");
                }

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: webs.Count()
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionListAction()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listNames = new List<string>();

                Action<List> listAction = list =>
                {
                    listNames.Add(list.Title);
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListAction", listAction),
                    new CommandParameter("ListProperties", new[] { "Title" })
                );

                Assert.IsTrue(listNames.Count > 3, "Wrong count on lists");

                Assert.IsTrue(listNames.Contains("PnPTestList1"), "PnPTestList1 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList2"), "PnPTestList2 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList3"), "PnPTestList3 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1
                );

                Assert.IsTrue(result.ProcessedListCount > 3, "Wrong count on proccessed list");
            }
        }

        [TestMethod]
        public void InvokeWebActionPostListAction()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listNames = new List<string>();

                Action<List> listPostAction = list =>
                {
                    listNames.Add(list.Title);
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("PostListAction", listPostAction),
                    new CommandParameter("ListProperties", new[] { "Title" })
                );

                Assert.IsTrue(listNames.Count > 3, "Wrong count on lists");

                Assert.IsTrue(listNames.Contains("PnPTestList1"), "PnPTestList1 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList2"), "PnPTestList2 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList3"), "PnPTestList3 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1
                );

                Assert.IsTrue(result.ProcessedPostListCount > 3, "Wrong count on proccessed list");
            }
        }

        [TestMethod]
        public void InvokeWebActionListActionWithShouldProcess()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listNames = new List<string>();

                Action<List> listAction = list =>
                {
                    listNames.Add(list.Title);
                };

                Func<List, bool> shouldProcessListAction = list =>
                {
                    return list.Title.Contains("PnPTestList");
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListAction", listAction),
                    new CommandParameter("ShouldProcessListAction", shouldProcessListAction),
                    new CommandParameter("ListProperties", new[] { "Title" })
                );

                Assert.IsTrue(listNames.Count == 3, "Wrong count on lists");

                Assert.IsTrue(listNames.Contains("PnPTestList1"), "PnPTestList1 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList2"), "PnPTestList2 is missing");
                Assert.IsTrue(listNames.Contains("PnPTestList3"), "PnPTestList3 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1,
                    processedListCount: 3
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionListActionWithListPropertiesRootFolder()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listUrls = new List<string>();

                Action<List> listAction = list =>
                {
                    listUrls.Add(list.RootFolder.ServerRelativeUrl);
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListAction", listAction),
                    new CommandParameter("ListProperties", new[] { "Title", "RootFolder" })
                );

                Assert.IsTrue(listUrls.Count > 3, "Wrong count on lists");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                foreach (var item in listUrls)
                    Assert.IsTrue(!string.IsNullOrEmpty(item), "Failed to load property RootFolder");

                AssertInvokeActionResult(result,
                    processedWebCount: 1
                );

                Assert.IsTrue(result.ProcessedListCount > 2, "Wrong count on proccessed list");
            }
        }

        [TestMethod]
        public void InvokeWebActionListItemAction()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listItemTitles = new List<string>();

                Action<ListItem> listItemAction = listItem =>
                {
                    listItemTitles.Add(listItem["Title"]?.ToString());
                };

                Func<List, bool> shouldProcessListAction = list =>
                {
                    return list.Title.Contains("PnPTestList");
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListItemAction", listItemAction),
                    new CommandParameter("ShouldProcessListAction", shouldProcessListAction)
                );

                Assert.IsTrue(listItemTitles.Count == 5, "Wrong count on listItems");

                Assert.IsTrue(listItemTitles.Contains("Test1-1"), "Test1-1 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test1-2"), "Test1-2 is missing");

                Assert.IsTrue(listItemTitles.Contains("Test2-1"), "Test2-1 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test2-2"), "Test2-2 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test2-3"), "Test2-3 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1,
                    processedListCount: 3,
                    processedListItemCount: 5
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionListItemActionWithSkipCounting()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listItemTitles = new List<string>();

                Action<ListItem> listItemAction = listItem =>
                {
                    listItemTitles.Add(listItem["Title"]?.ToString());
                };

                Func<List, bool> shouldProcessListAction = list =>
                {
                    return list.Title.Contains("PnPTestList");
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListItemAction", listItemAction),
                    new CommandParameter("ShouldProcessListAction", shouldProcessListAction),
                    new CommandParameter("SkipCounting", true)
                );

                Assert.IsTrue(listItemTitles.Count == 5, "Wrong count on listItems");

                Assert.IsTrue(listItemTitles.Contains("Test1-1"), "Test1-1 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test1-2"), "Test1-2 is missing");

                Assert.IsTrue(listItemTitles.Contains("Test2-1"), "Test2-1 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test2-2"), "Test2-2 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test2-3"), "Test2-3 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1,
                    processedListCount: 3,
                    processedListItemCount: 5
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionListItemActionWithListName()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listItemTitles = new List<string>();

                Action<ListItem> listItemAction = listItem =>
                {
                    listItemTitles.Add(listItem["Title"]?.ToString());
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListItemAction", listItemAction),
                    new CommandParameter("ListName", "PnPTestList1")
                );

                Assert.IsTrue(listItemTitles.Count == 2, "Wrong count on listItems");

                Assert.IsTrue(listItemTitles.Contains("Test1-1"), "Test1-1 is missing");
                Assert.IsTrue(listItemTitles.Contains("Test1-2"), "Test1-2 is missing");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1,
                    processedListCount: 1,
                    processedListItemCount: 2
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionListItemActionWithShouldProcess()
        {
            using (var scope = new PSTestScope(true))
            {
                List<string> listItemTitles = new List<string>();

                Action<ListItem> listItemAction = listItem =>
                {
                    listItemTitles.Add(listItem["Title"]?.ToString());
                };

                Func<List, bool> shouldProcessListAction = list =>
                {
                    return list.Title.Contains("PnPTestList");
                };

                Func<ListItem, bool> shouldProcessListItemAction = listItem =>
                {
                    string title = listItem["Title"]?.ToString();
                    if (title == null)
                        title = string.Empty;

                    int number = Convert.ToInt32(title[title.Length -1]);

                    return number % 2 == 0;
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("ListItemAction", listItemAction),
                    new CommandParameter("ShouldProcessListAction", shouldProcessListAction),
                    new CommandParameter("ShouldProcessListItemAction", shouldProcessListItemAction)
                );

                Assert.IsTrue(listItemTitles.Count == 2, "Wrong count on listItems");

                Assert.IsFalse(listItemTitles.Contains("Test1-1"), "Test1-1 should not exist");
                Assert.IsTrue(listItemTitles.Contains("Test1-2"), "Test1-2 is missing");

                Assert.IsFalse(listItemTitles.Contains("Test2-1"), "Test2-1 should not exist");
                Assert.IsTrue(listItemTitles.Contains("Test2-2"), "Test2-2 is missing");
                Assert.IsFalse(listItemTitles.Contains("Test2-3"), "Test2-3 should not exist");

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                AssertInvokeActionResult(result,
                    processedWebCount: 1,
                    processedListCount: 3,
                    processedListItemCount: 2
                );
            }
        }

        [TestMethod]
        public void InvokeWebActionWithWhatIf()
        {
            using (var scope = new PSTestScope(true))
            {
                bool webActionFired = false;
                bool webPostActionFired = false;
                bool listActionFired = false;
                bool listPostActionFired = false;
                bool listItemActionFired = false;

                Action<Web> webAction = web =>
                {
                    webActionFired = true;
                };

                Action<Web> webPostAction = web =>
                {
                    webPostActionFired = true;
                };

                Func<List, bool> shouldProcessListAction = list =>
                {
                    return list.Title.Contains("PnPTestList");
                };

                Action<List> listAction = list =>
                {
                    listActionFired = true;
                };

                Action<List> listPostAction = list =>
                {
                    listPostActionFired = true;
                };

                Action<ListItem> listItemAction = listItem =>
                {
                    listItemActionFired = true;
                };

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
                    new CommandParameter("WebAction", webAction),
                    new CommandParameter("PostWebAction", webPostAction),
                    new CommandParameter("ShouldProcessListAction", shouldProcessListAction),
                    new CommandParameter("ListAction", listAction),
                    new CommandParameter("PostListAction", listPostAction),
                    new CommandParameter("ListItemAction", listItemAction),
                    new CommandParameter("WhatIf", true)
                );

                InvokeWebActionResult result = results.Last().BaseObject as InvokeWebActionResult;

                Assert.AreEqual(1, result.ProcessedWebCount, "Total proccessed web count does not match");

                Assert.IsFalse(webActionFired, "web action fired");
                Assert.IsFalse(webPostActionFired, "web post action fired");
                Assert.IsFalse(listActionFired, "list action fired");
                Assert.IsFalse(listPostActionFired, "list post action fired");
                Assert.IsFalse(listItemActionFired, "list item action fired");
            }
        }

        private void AssertInvokeActionResult(InvokeWebActionResult result,
            int? processedWebCount = null,
            int? processedPostWebCount = null,
            int? processedListCount = null,
            int? processedPostListCount = null,
            int? processedListItemCount = null,
            int? processedPostListItemCount = null)
        {
            Assert.IsNotNull(result, "InvokeActionResult is null");

            if (processedWebCount.HasValue)
            {
                Assert.AreEqual(processedWebCount.Value, result.ProcessedWebCount, "Total proccessed web count does not match");

                if(result.AverageWebTime.HasValue)
                    Assert.IsTrue(result.AverageWebTime > 0, "Average web time is 0");
            }

            if (processedPostWebCount.HasValue)
            {
                Assert.AreEqual(processedPostWebCount.Value, result.ProcessedPostWebCount, "Total proccessed web count does not match");

                if (result.AveragePostWebTime.HasValue)
                    Assert.IsTrue(result.AveragePostWebTime > 0, "Average web time is 0");
            }

            if (processedListCount.HasValue)
            {
                Assert.AreEqual(processedListCount.Value, result.ProcessedListCount, "Total proccessed list count does not match");

                if (result.AverageListTime.HasValue)
                    Assert.IsTrue(result.AverageListTime > 0, "Average list time is 0");
            }

            if (processedPostListCount.HasValue)
            {
                Assert.AreEqual(processedPostListCount.Value, result.ProcessedPostListCount, "Total proccessed list count does not match");

                if (result.AveragePostListTime.HasValue)
                    Assert.IsTrue(result.AveragePostListTime > 0, "Average list time is 0");
            }

            if (processedListItemCount.HasValue)
            {
                Assert.AreEqual(processedListItemCount.Value, result.ProcessedListItemCount, "Total proccessed list item count does not match");

                if (result.AverageListItemTime.HasValue)
                    Assert.IsTrue(result.AverageListItemTime > 0, "Average list item time is 0");
            }

            Assert.IsTrue(result.StartDate >= DateTime.Today, "Incorrect start date");
            Assert.IsTrue(result.EndDate >= DateTime.Today, "Incorrect end date");
            Assert.IsTrue(result.EndDate > result.StartDate, "End date is not greater than start date");
        }
    }
}
