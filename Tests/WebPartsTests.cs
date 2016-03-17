using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Client;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Xml.Linq;
using OfficeDevPnP.Core.Utilities;

namespace OfficeDevPnP.PowerShell.Tests
{
    [TestClass]
    public class WebPartsTests
    {
        string serverRelativeHomePageUrl;

        [TestInitialize]
        public void Initialize()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var homePage = ctx.Web.RootFolder.EnsureProperty(f => f.WelcomePage);

                serverRelativeHomePageUrl = UrlUtility.Combine(ctx.Web.EnsureProperty(w => w.ServerRelativeUrl), homePage);
            }
        }
        [TestMethod]
        public void GetWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // retrieve homepage

                var results = scope.ExecuteCommand("Get-SPOWebPart",
                    new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl));

                Assert.IsTrue(results.Count > 0);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.WebParts.WebPartDefinition));
            }
        }

        [TestMethod]
        public void GetWebPartPropertyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    var wps = ctx.Web.GetWebParts(serverRelativeHomePageUrl);

                    if (wps.Any())
                    {
                        var wp = wps.FirstOrDefault();
                        var results = scope.ExecuteCommand("Get-SPOWebPartProperty",
                            new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                            new CommandParameter("Identity", wp.Id));

                        Assert.IsTrue(results.Count > 0);
                        Assert.IsTrue(results[0].BaseObject.GetType() == typeof(OfficeDevPnP.PowerShell.Commands.PropertyBagValue));

                    }
                    else
                    {
                        Assert.Fail("No webparts on page.");
                    }

                }
            }
        }

        [TestMethod]
        public void GetWebPartXmlTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    var wps = ctx.Web.GetWebParts(serverRelativeHomePageUrl);

                    if (wps.Any())
                    {
                        var wp = wps.FirstOrDefault();
                        var results = scope.ExecuteCommand("Get-SPOWebPartXml",
                            new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                            new CommandParameter("Identity", wp.Id));

                        Assert.IsTrue(results.Count > 0);
                        Assert.IsTrue(results[0].BaseObject.GetType() == typeof(string));
                        try
                        {
                            var xelement = XElement.Parse(results[0].ToString());
                        }
                        catch
                        {
                            Assert.Fail("Returned data is not valid XML");
                        }
                    }
                    else
                    {
                        Assert.Fail("No webparts on page.");
                    }

                }
            }
        }

        [TestMethod]
        public void AddWebPartToWikiPageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    var results = scope.ExecuteCommand("Add-SPOWebPartToWikiPage",
                        new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                        new CommandParameter("Path", "..\\..\\Resources\\webpart.xml"),
                        new CommandParameter("Row", 1),
                        new CommandParameter("Column", 1));

                    Assert.IsFalse(results.Any());

                    var wps = ctx.Web.GetWebParts(serverRelativeHomePageUrl);

                    foreach (var wp in wps)
                    {
                        if (wp.WebPart.Title == "Get started with your site")
                        {
                            wp.DeleteWebPart();
                            ctx.ExecuteQueryRetry();
                            break;
                        }
                    }
                }
            }
        }


        [TestMethod]
        public void RemoveWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    var results = scope.ExecuteCommand("Add-SPOWebPartToWikiPage",
                        new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                        new CommandParameter("Path", "..\\..\\Resources\\webpart.xml"),
                        new CommandParameter("Row", 1),
                        new CommandParameter("Column", 1));


                    Assert.IsFalse(results.Any());

                    results = scope.ExecuteCommand("Remove-SPOWebPart",
                          new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                        new CommandParameter("Title", "Get start with your site"));

                    Assert.IsFalse(results.Any());
                }
            }
        }

        [TestMethod]
        public void SetWebPartPropertyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                using (var ctx = TestCommon.CreateClientContext())
                {
                    var results = scope.ExecuteCommand("Add-SPOWebPartToWikiPage",
                        new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                        new CommandParameter("Path", "..\\..\\Resources\\webpart.xml"),
                        new CommandParameter("Row", 1),
                        new CommandParameter("Column", 1));


                    Assert.IsFalse(results.Any());

                    var wps = ctx.Web.GetWebParts(serverRelativeHomePageUrl);

                    foreach (var wp in wps)
                    {
                        if (wp.WebPart.Title == "Get started with your site")
                        {
                            results = scope.ExecuteCommand("Set-SPOWebPartProperty",
                                new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                                new CommandParameter("Identity", wp.Id),
                                new CommandParameter("Key", "Title"),
                                new CommandParameter("Value", "TESTTESTTEST"));

                            Assert.IsFalse(results.Any());

                            results = scope.ExecuteCommand("Remove-SPOWebPart",
                                new CommandParameter("ServerRelativePageUrl", serverRelativeHomePageUrl),
                                new CommandParameter("Title", "TESTTESTTEST"));
                            Assert.IsFalse(results.Any());

                            break;
                        }
                    }
                }
            }
        }
    }
}
