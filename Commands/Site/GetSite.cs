using System;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSite")]
    [CmdletHelp("Returns the current site collection from the context.",
        Category = CmdletHelpCategory.Sites,
        OutputType = typeof(Microsoft.SharePoint.Client.Site),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.site.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPSite",
        Remarks = "Gets the current site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSite -Includes RootWeb,ServerRelativeUrl",
        Remarks = "Gets the current site specifying to include RootWeb and ServerRelativeUrl properties. For the full list of properties see https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee538579(v%3doffice.15)",
        SortOrder = 2)]
        
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
