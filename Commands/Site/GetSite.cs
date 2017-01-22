using System;
using System.Linq.Expressions;
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
    public class GetSite : PnPRetrievalsCmdlet<Microsoft.SharePoint.Client.Site>
    {
        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Microsoft.SharePoint.Client.Site, object>>[] { s => s.Url, s => s.CompatibilityLevel};
            var site = ClientContext.Site;
            ClientContext.Load(site, RetrievalExpressions);
            ClientContext.ExecuteQueryRetry();
            WriteObject(site);
        }
    }
}
