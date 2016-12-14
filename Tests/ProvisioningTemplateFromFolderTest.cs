using System;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharePointPnP.PowerShell.Tests
{
    /// <summary>
    /// Class used to test .pnp generation commandlets
    /// </summary>
    [TestClass,Ignore]
    public class ProvisioningTemplateFromFolderTest
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            //System.Environment.CurrentDirectory = @"<your folder>";
        }

        [TestMethod]
        public void CreateXmlTemplateFromFolder()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("New-PnPProvisioningTemplateFromFolder",
                    new CommandParameter("Out", @"dummy.xml"),
                    new CommandParameter("Folder", @".\Dummy"),
                    new CommandParameter("TargetFolder", @""),
                    new CommandParameter("Force")
                    );

            }
        }

        [TestMethod]
        public void CreatePnpTemplateFromFolder()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("New-PnPProvisioningTemplateFromFolder",
                    new CommandParameter("Out", @"dummy.pnp"),
                    new CommandParameter("Folder", @".\Dummy"),
                    new CommandParameter("Force")
                    );

            }
        }

        [TestMethod]
        public void ApplyPnpTemplateFromFolder()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Apply-PnPProvisioningTemplate",
                    new CommandParameter("Path", @"dummy.pnp")
                    );

            }
        }
    }
}
