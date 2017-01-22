using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Utilities;

namespace SharePointPnP.PowerShell.Tests
{
    [TestClass]
    public class BaseTests
    {
        [TestMethod]
        public void ConnectSPOnlineTest1()
        {
            using (var scope = new PSTestScope(false))
            {
                var creds = GetCredentials(ConfigurationManager.AppSettings["SPODevSiteUrl"]);
                if (creds != null)
                {
                    var results = scope.ExecuteCommand("Connect-PnPOnline", new CommandParameter("Url", ConfigurationManager.AppSettings["SPODevSiteUrl"]));
                    Assert.IsTrue(results.Count == 0);
                } else
                {
                    Assert.Inconclusive("No Credential Manager Credentials present");
                }
            }
        }

        public void ConnectSPOnlineTest2()
        {
            using (var scope = new PSTestScope(false))
            {
                if (ConfigurationManager.AppSettings["SPOUserName"] != null &&
                ConfigurationManager.AppSettings["SPOPassword"] != null)
                {
                    var script = String.Format(@" [ValidateNotNullOrEmpty()] $userPassword = ""{1}""
                                              $userPassword = ConvertTo-SecureString -String {1} -AsPlainText -Force
                                              $cred = New-Object –TypeName System.Management.Automation.PSCredential –ArgumentList {0}, $userPassword
                                              Connect-PnPOnline -Url {2} -Credentials $cred"
                                           , ConfigurationManager.AppSettings["SPOUserName"],
                                           ConfigurationManager.AppSettings["SPOPassword"],
                                           ConfigurationManager.AppSettings["SPODevSiteUrl"]);
                    var results = scope.ExecuteScript(script);
                    Assert.IsTrue(results.Count == 0);
                } else
                {
                    Assert.Inconclusive("No credentials specified in app.config");
                }
            }
        }

        private PSCredential GetCredentials(string url)
        {
            PSCredential creds = null;

            var connectionURI = new Uri(url);

            // Try to get the credentials by full url

            creds = CredentialManager.GetCredential(url);
            if (creds == null)
            {
                // Try to get the credentials by splitting up the path
                var pathString = string.Format("{0}://{1}", connectionURI.Scheme, connectionURI.IsDefaultPort ? connectionURI.Host : string.Format("{0}:{1}", connectionURI.Host, connectionURI.Port));
                var path = connectionURI.AbsolutePath;
                while (path.IndexOf('/') != -1)
                {
                    path = path.Substring(0, path.LastIndexOf('/'));
                    if (!string.IsNullOrEmpty(path))
                    {
                        var pathUrl = string.Format("{0}{1}", pathString, path);
                        creds = CredentialManager.GetCredential(pathUrl);
                        if (creds != null)
                        {
                            break;
                        }
                    }
                }

                if (creds == null)
                {
                    // Try to find the credentials by schema and hostname
                    creds = CredentialManager.GetCredential(connectionURI.Scheme + "://" + connectionURI.Host);

                    if (creds == null)
                    {
                        // try to find the credentials by hostname
                        creds = CredentialManager.GetCredential(connectionURI.Host);
                    }
                }

            }

            return creds;
        }

        [TestMethod]
        public void ConnectSPOnlineTest3()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPContext");

                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(OfficeDevPnP.Core.PnPClientContext));

            }
        }

        [TestMethod]
        public void GetPropertyTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                using (var scope = new PSTestScope(true))
                {

                    var results = scope.ExecuteCommand("Get-PnPProperty",
                        new CommandParameter("ClientObject", ctx.Web),
                        new CommandParameter("Property", "Lists"));
                    Assert.IsTrue(results.Count == 1);
                    Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListCollection));
                }
            }
        }

        [TestMethod]
        public void IncludesTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                Expression<Func<List, object>> expressionRelativeUrl = l => l.RootFolder.ServerRelativeUrl;

                var type = typeof(List);

                var exp2 = (Expression<Func<List,object>>)SharePointPnP.PowerShell.Commands.Utilities.DynamicExpression.ParseLambda(type, typeof(object), "RootFolder.ServerRelativeUrl", null);
                //var exp = Expression.Lambda<Func<List, object>>(Expression.Convert(body, typeof(object)), exp2);

             
                /*
                var subMemberName = "ServerRelativeUrl";
                var subMemberType = typeof(List).GetProperty(memberName).PropertyType;
                var subparamExpression = Expression.Parameter(subMemberType, "s");
                var subparamMemberExpressoin = Expression.Property(subparamExpression, subMemberName);
                var subcast = Expression.Convert(subparamExpression, subMemberType);
                var subBody = Expression.Property(subcast, subMemberName);
                */
                //exp = Expression.Lambda<Func<List, object>>(Expression.Convert(body, typeof(object)), paramExpression);


                var query = (ctx.Web.Lists.IncludeWithDefaultProperties(new[] {exp2}));
                var lists = ctx.LoadQuery(query);
                ctx.ExecuteQueryRetry();
            }
        }
    }
}
