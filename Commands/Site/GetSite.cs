using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSite")]
    [CmdletHelp("Returns the current site collection from the context",
        Category = CmdletHelpCategory.Sites,
        DetailedDescription = "This command returns the site collection of the current context. If you wish to return another site collection, use Connect-PnPOnline -Url <other site collection> to connect to it first.",
        OutputType = typeof(Microsoft.SharePoint.Client.Site),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.site.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPSite",
                Remarks = @"Returns the site collection of the current context",
                SortOrder = 1)]
    public class GetSite : PnPRetrievalsCmdlet<Microsoft.SharePoint.Client.Site>
    {
        protected override void ExecuteCmdlet()
        {
            ClientContext.Load(ClientContext.Site);
            ClientContext.ExecuteQueryRetry();

            var siteProperties = Utilities.PSObjectConverter.ConvertGenericObject(ClientContext.Site, this);
            WriteObject(siteProperties);
        }
    }
}
