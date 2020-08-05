using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPTaxonomyItem", SupportsShouldProcess = true)]
    [CmdletHelp(@"Returns a taxonomy item", 
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TaxonomyItem),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.taxonomyitem.aspx")]
    public class GetTaxonomyItem : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The path, delimited by | of the taxonomy item to retrieve, alike GROUPLABEL|TERMSETLABEL|TERMLABEL")]
        [Alias("Term")]
        public string TermPath;

        protected override void ExecuteCmdlet()
        {
            var item = ClientContext.Site.GetTaxonomyItemByPath(TermPath);
            WriteObject(item);
        }

    }
}
