using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Get, "PnPContentTypePublishingHubUrl")]
    [CmdletHelp("Returns the url to Content Type Publishing Hub",
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = "PS:> $url = Get-PnPContentTypePublishingHubUrl\n" +
        "PS:> Connect-PnPOnline -Url $url\n" +
        "PS:> Get-PnPContentType\n",
        Remarks = @"This will retrieve the url to the content type hub, connect to it, and then retrieve the content types form that site",
        SortOrder = 1)]

    public class GetContentTypePublishingHub : SPOCmdlet
    {
   
        protected override void ExecuteCmdlet()
        {
            TaxonomySession session = TaxonomySession.GetTaxonomySession(ClientContext);
            var termStore = session.GetDefaultSiteCollectionTermStore();
            ClientContext.Load(termStore, t => t.ContentTypePublishingHub);
            ClientContext.ExecuteQueryRetry();
            WriteObject(termStore.ContentTypePublishingHub);
        }

    }
}
