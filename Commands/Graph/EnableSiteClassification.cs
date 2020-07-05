#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsLifecycle.Enable, "PnPSiteClassification")]
    [CmdletHelp("Enables Site Classifications for the tenant",
        Category = CmdletHelpCategory.Graph, 
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -DefaultClassification ""LBI""",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The default value will be set to ""LBI""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -UsageGuidelinesUrl https://aka.ms/sppnp",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The usage guideliness will be set to the specified URL.",
        SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
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
            OfficeDevPnP.Core.Framework.Graph.SiteClassificationsUtility.EnableSiteClassifications(AccessToken, Classifications, DefaultClassification, UsageGuidelinesUrl);
        }
    }
}
#endif
