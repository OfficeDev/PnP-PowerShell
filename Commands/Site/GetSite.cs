using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSite")]
    [CmdletHelp("Returns the current site collection from the context.",
        Category = CmdletHelpCategory.Sites,
        OutputType = typeof(Microsoft.SharePoint.Client.Site),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.site.aspx")]
    public class GetSite : SPOCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var site = ClientContext.Site;
            ClientContext.Load(site, s => s.Url, s => s.CompatibilityLevel);
            ClientContext.ExecuteQueryRetry();
            WriteObject(site);
        }
    }

    [Cmdlet(VerbsCommon.Get, "SPOSite")]
    [CmdletHelp("Returns the current site collection from the context.",
        Category = CmdletHelpCategory.Sites,
        OutputType = typeof(Microsoft.SharePoint.Client.Site),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.site.aspx")]
    [Obsolete("We will remove this cmdlet in the January 2017 release. Please switch to using the Get-PnPSite Cmdlet.")]
    public class GetSPOSite : SPOCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var site = ClientContext.Site;
            ClientContext.Load(site, s => s.Url, s => s.CompatibilityLevel);
            ClientContext.ExecuteQueryRetry();
            WriteObject(site);
        }
    }
}
