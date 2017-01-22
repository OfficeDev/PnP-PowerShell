using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteCollectionTermStore", SupportsShouldProcess = true)]
    [CmdletHelp(@"Returns the site collection term store", 
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TermStore),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termstore.aspx")]
    [CmdletExample
        (Code = @"PS:> Get-PnPSiteCollectionTermStore",
        Remarks = "Returns the site collection term store.",
        SortOrder = 1)]
    public class GetPnPSiteCollectionTermStore : SPOCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            TaxonomySession session = TaxonomySession.GetTaxonomySession(ClientContext);
            var termStore = session.GetDefaultSiteCollectionTermStore();
            ClientContext.Load(termStore, t => t.Id, t => t.Name, t => t.Groups, t => t.KeywordsTermSet);
            ClientContext.ExecuteQueryRetry();
            WriteObject(termStore);
        }

    }
}
