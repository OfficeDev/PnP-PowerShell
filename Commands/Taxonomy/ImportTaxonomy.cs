using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using File = System.IO.File;
using System.Linq;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsData.Import, "PnPTaxonomy", SupportsShouldProcess = true)]
    [CmdletHelp("Imports a taxonomy from either a string array or a file",
        Category = CmdletHelpCategory.Taxonomy)]
    [CmdletExample(
        Code = @"PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm'",
        Remarks = "Creates a new termgroup, 'Company', a termset 'Locations' and a term 'Stockholm'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm|Central','Company|Locations|Stockholm|North'",
        Remarks = "Creates a new termgroup, 'Company', a termset 'Locations', a term 'Stockholm' and two subterms: 'Central', and 'North'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Import-PnPTaxonomy -Path ./mytaxonomyterms.txt",
        Remarks = "Imports the taxonomy from the file specified. Each line has to be in the format TERMGROUP|TERMSET|TERM. See example 2 for examples of the format.",
        SortOrder = 3)]
    public class ImportTaxonomy : PnPSharePointCmdlet
    {

        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "Direct", HelpMessage = "An array of strings describing termgroup, termset, term, subterms using a default delimiter of '|'.")]
        public string[] Terms;

        [Parameter(Mandatory = true, ParameterSetName = "File", HelpMessage = "Specifies a file containing terms per line, in the format as required by the Terms parameter.")]
        public string Path;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public int Lcid = 1033;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Term store to import to; if not specified the default term store is used.")]
        public string TermStoreName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The path delimiter to be used, by default this is '|'")]
        public string Delimiter = "|";

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "If specified, terms that exist in the termset, but are not in the imported data, will be removed.")]
        public SwitchParameter SynchronizeDeletions;

        protected override void ExecuteCmdlet()
        {
            string[] lines;
            if (ParameterSetName == "File")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }

                lines = File.ReadAllLines(Path);
            }
            else
            {
                lines = Terms;
            }

            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            if (!string.IsNullOrEmpty(TermStoreName))
            {
                var taxSession = TaxonomySession.GetTaxonomySession(ClientContext);
                var termStore = taxSession.TermStores.GetByName(TermStoreName);
                ClientContext.Site.ImportTerms(lines, Lcid, termStore, Delimiter, SynchronizeDeletions);
            }
            else
            {
                ClientContext.Site.ImportTerms(lines, Lcid, Delimiter, SynchronizeDeletions);
            }
        }

    }
}
