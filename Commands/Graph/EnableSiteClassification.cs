using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsLifecycle.Enable, "PnPSiteClassification")]
    [CmdletHelp("Enables Site Classifications for the tenant. Requires a connection to the Microsoft Graph.",
        Category = CmdletHelpCategory.Graph, 
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -DefaultClassification ""LBI""",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The default value will be set to ""LBI""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -UsageGuidelinesUrl http://aka.ms/sppnp",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The usage guideliness will be set to the specified URL.",
        SortOrder = 2)]
    public class EnableSiteClassification : PnPGraphCmdlet
    {

        [Parameter(Mandatory = true)]
        public List<string> Classifications;

        [Parameter(Mandatory = true)]
        public string DefaultClassification;

        [Parameter(Mandatory = false)]
        public string UsageGuidelinesUrl = "";

        protected override void ExecuteCmdlet()
        {
            OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.EnableSiteClassifications(AccessToken, Classifications, DefaultClassification, UsageGuidelinesUrl);
        }
    }
}
