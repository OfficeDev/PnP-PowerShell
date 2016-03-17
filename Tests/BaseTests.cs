using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.ObjectModel;
using OfficeDevPnP.PowerShell.Commands.Base;
using OfficeDevPnP.PowerShell.Tests;
using System.Configuration;

namespace OfficeDevPnP.PowerShell.Tests
{
    [TestClass]
    public class BaseTests
    {
        [TestMethod]
        public void ConnectSPOnlineTest1()
        {
            using (var scope = new PSTestScope(false))
            {
                

                var script = String.Format(@" [ValidateNotNullOrEmpty()] $userPassword = ""{1}""
                                              $userPassword = ConvertTo-SecureString -String {1} -AsPlainText -Force
                                              $cred = New-Object –TypeName System.Management.Automation.PSCredential –ArgumentList {0}, $userPassword
                                              Connect-SPOnline -Url {2} -Credentials $cred"
                                    , ConfigurationManager.AppSettings["SPOUserName"],
                                    ConfigurationManager.AppSettings["SPOPassword"],
                                    ConfigurationManager.AppSettings["SPODevSiteUrl"]);
                var results = scope.ExecuteScript(script);
                Assert.IsTrue(results.Count == 0);
            }
        }

        [TestMethod]
        public void ConnectSPOnlineTest2()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-SPOContext");

                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject.GetType().BaseType == typeof(Microsoft.SharePoint.Client.ClientContext));

            }
        }

        [TestMethod]
        public void GetPropertyTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                using (var scope = new PSTestScope(true))
                {

                    var results = scope.ExecuteCommand("Get-SPOProperty",
                        new CommandParameter("ClientObject", ctx.Web),
                        new CommandParameter("Property", "Lists"));
                    Assert.IsTrue(results.Count == 1);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListCollection));
                }
            }
        }
    }
}
