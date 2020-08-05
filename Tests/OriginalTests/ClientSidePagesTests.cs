#if !ONPREMISES
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using System.Linq;
using OfficeDevPnP.Core.Pages;
using System.Collections;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class ClientSidePagesTests
    {

        public const string PageTestNewDefaultName = "Page 1.aspx";
        public const string PageTestNewName = "NewPage.aspx";
        public const string PageTestWithoutExtensionName = "PageWithoutExtensionTest";
        public const string PageTestWithExtensionName = "PageWithExtensionTest.aspx";
        public const string PagePipedTestName = "PagePipedTest.aspx";
        public const string PageGetTestName = "PageGetTest.aspx";
        public const string PageRemoveTestName = "PageRemoveTest.aspx";
        public const string PageSetTestName = "PageSetTest.aspx";
        public const string PageSet2TestName = "PageSet2Test.aspx";
        public const string PageNotExistingTestName = "PageNotExisting.aspx";
        public const string PageAddSectionTestName = "PageAddSection.aspx";
        public const string PageAddWebPartTestName = "PageAddWebPart.aspx";
        public const string ClassicPageTestNewName = "table_1.aspx";
        public const string ClassicWikiPagesLibName = "WikiPages_table_1";

        private void CleanupPageIfExists(ClientContext ctx, string pageName)
        {
            try
            {
                pageName = pageName.EndsWith(".aspx") ? pageName : pageName + ".aspx";
                var p = ClientSidePage.Load(ctx, pageName);
                p.Delete();
            }
            catch (Exception) { }
        }

        private void CleanupListIfExists(ClientContext ctx, string listTitle)
        {
            var list = ctx.Web.GetListByTitle(listTitle);
            if (list != null)
            {
                list.DeleteObject();
                ctx.ExecuteQueryRetry();
            }
        }

        private void CreateWikiPagesLib(ClientContext ctx, string listTitle)
        {
            var info = new ListCreationInformation();
            var templates = ctx.Web.ListTemplates;
            ctx.Load(templates);
            ctx.ExecuteQueryRetry();
            info.ListTemplate = templates.First(t => t.InternalName == "webpagelib");
            info.Title = listTitle;
            info.Description = "Created by PnP unit tests";
            var list = ctx.Web.Lists.Add(info);
            ctx.ExecuteQueryRetry();
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                CreateWikiPagesLib(ctx, ClassicWikiPagesLibName);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                // Delete all the test pages if they exist
                CleanupPageIfExists(ctx, PageTestNewDefaultName);
                CleanupPageIfExists(ctx, PageTestNewName);
                CleanupPageIfExists(ctx, PageTestWithoutExtensionName);
                CleanupPageIfExists(ctx, PageTestWithExtensionName);
                CleanupPageIfExists(ctx, PagePipedTestName);
                CleanupPageIfExists(ctx, PageGetTestName);
                CleanupPageIfExists(ctx, PageRemoveTestName);
                CleanupPageIfExists(ctx, PageSetTestName);
                CleanupPageIfExists(ctx, PageSet2TestName);
                CleanupPageIfExists(ctx, PageAddSectionTestName);
                CleanupPageIfExists(ctx, PageAddWebPartTestName);

                CleanupListIfExists(ctx, ClassicWikiPagesLibName);
            }
        }

        [TestMethod]
        public void ConvertBasicWikiPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddWikiPage(ClassicWikiPagesLibName, ClassicPageTestNewName);
                }

                var results = scope.ExecuteCommand("ConvertTo-PnPClientSidePage",
                    new CommandParameter("Identity", ClassicPageTestNewName), new CommandParameter("Library", ClassicWikiPagesLibName), new CommandParameter("Overwrite", true));

                Assert.AreEqual(0, results.Count, "Wiki pages suddenly can be transformed. Great! Adjust this assertion to check for one result.");
            }
        }


        [TestMethod]
        public void AddClientSidePageWithNameWithoutExtensionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPClientSidePage",
                    new CommandParameter("Name", PageTestWithoutExtensionName));

                var page = results[0].BaseObject as ClientSidePage;
                string pageName = page.PageListItem["FileLeafRef"] as string;
                Assert.IsTrue(page != null && pageName == PageTestWithoutExtensionName + ".aspx");
            }
        }

        [TestMethod]
        public void AddClientSidePageWithNameWithExtensionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Add-PnPClientSidePage",
                    new CommandParameter("Name", PageTestWithExtensionName));

                var page = results[0].BaseObject as ClientSidePage;
                string pageName = page.PageListItem["FileLeafRef"] as string;
                Assert.IsTrue(page != null && pageName == PageTestWithExtensionName);
            }
        }


        [TestMethod]
        public void GetClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddClientSidePage(PageGetTestName, true);
                }

                var results = scope.ExecuteCommand("Get-PnPClientSidePage",
                    new CommandParameter("Identity", PageGetTestName));

                var page = results[0].BaseObject as ClientSidePage;
                string pageName = page.PageListItem["FileLeafRef"] as string;
                Assert.IsTrue(page != null && pageName == PageGetTestName);
            }
        }


        [TestMethod]
        public void GetClientSidePageNotExistingTest()
        {
            using (var scope = new PSTestScope(true))
            {
                try
                {
                    scope.ExecuteCommand("Get-PnPClientSidePage",
                    new CommandParameter("Identity", PageNotExistingTestName));
                    Assert.Fail();
                }
                catch (Exception)
                {
                    // An exception should be thrown
                    Assert.IsTrue(true);
                }

            }
        }

        [TestMethod]
        public void SetClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddClientSidePage(PageSetTestName, true);


                    var results = scope.ExecuteCommand("Set-PnPClientSidePage",
                        new CommandParameter("Identity", PageSetTestName),
                         new CommandParameter("LayoutType", ClientSidePageLayoutType.Home),
                          new CommandParameter("Name", PageSet2TestName));

                    var page = ClientSidePage.Load(ctx, PageSet2TestName);

                    Assert.IsTrue(page.LayoutType == ClientSidePageLayoutType.Home);
                }
            }
        }

        // TODO Add more test cases

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
   "The page does not exist.")]
        public void RemoveClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddClientSidePage(PageRemoveTestName, true);

                    scope.ExecuteCommand("Remove-PnPClientSidePage",
                         new CommandParameter("Identity", PageRemoveTestName),
                         new CommandParameter("Force"));


                    var p = ClientSidePage.Load(ctx, PageRemoveTestName);
                }
            }
        }

        [TestMethod]
        public void AddClientSidePageSectionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddClientSidePage(PageAddSectionTestName, true);


                    var results = scope.ExecuteCommand("Add-PnPClientSidePageSection",
                        new CommandParameter("Page", PageAddSectionTestName),
                         new CommandParameter("SectionTemplate", CanvasSectionTemplate.ThreeColumn),
                          new CommandParameter("Order", 10));

                    var page = ClientSidePage.Load(ctx, PageAddSectionTestName);

                    Assert.IsTrue(page.Sections[0].Columns.Count == 3);
                }
            }
        }

        [TestMethod]
        public void AddClientSideWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    ctx.Web.AddClientSidePage(PageAddWebPartTestName, true);

                    var results = scope.ExecuteCommand("Add-PnPClientSideWebPart",
                        new CommandParameter("Page", PageAddWebPartTestName),
                         new CommandParameter("DefaultWebPartType", DefaultClientSideWebParts.Image),
                          new CommandParameter("WebPartProperties", new Hashtable()
                          {
                            {"imageSourceType",  2},
                            {"siteId", "c827cb03-d059-4956-83d0-cd60e02e3b41" },
                            {"webId","9fafd7c0-e8c3-4a3c-9e87-4232c481ca26" },
                            {"listId","78d1b1ac-7590-49e7-b812-55f37c018c4b" },
                            {"uniqueId","3C27A419-66D0-4C36-BF24-BD6147719052" },
                            {"imgWidth", 500 },
                            {"imgHeight", 235 }
                          }
                          ));

                    var page = ClientSidePage.Load(ctx, PageAddWebPartTestName);
             
                    Assert.AreEqual(page.Controls.Count , 1);
                }
            }
        }
    }
}
#endif