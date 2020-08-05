using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Remove, "PnPTaxonomyItem", SupportsShouldProcess = true)]
    [CmdletHelp(@"Removes a taxonomy item",
         Category = CmdletHelpCategory.Taxonomy)]
    public class RemoveTaxonomyItem : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true,
             HelpMessage =
                 "The path, delimited by | of the taxonomy item to remove, alike GROUPLABEL|TERMSETLABEL|TERMLABEL")]
        [Alias("Term")]
        public string TermPath;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var item = ClientContext.Site.GetTaxonomyItemByPath(TermPath);
            if (item != null)
            {
                if (item is TermGroup)
                {
                    var group = item as TermGroup;
                    group.EnsureProperties(g => g.Name, g => g.TermSets);

                    if (Force ||
                        ShouldContinue(
                            string.Format(Resources.RemoveTermGroup0AndAllUnderlyingTermSetsAndTerms, group.Name),
                            Resources.Confirm))
                    {
                        if (group.TermSets.Any())
                        {
                            foreach (var termSet in group.TermSets)
                            {
                                termSet.DeleteObject();
                            }
                        }
                        item.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
                else if (item is TermSet)
                {
                    var termSet = item as TermSet;
                    termSet.EnsureProperty(t => t.Name);
                    if (Force ||
                        ShouldContinue(
                            string.Format(Resources.RemoveTermSet0, termSet.Name),
                            Resources.Confirm))
                    {
                        termSet.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }

                }
                else if (item is Term)
                {
                    var term = item as Term;
                    term.EnsureProperty(t => t.Name);
                    if (Force ||
                        ShouldContinue(
                            string.Format(Resources.RemoveTerm0AndAllUnderlyingTerms, term.Name),
                            Resources.Confirm))
                    {
                        term.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }
                }

            }
            else
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Cannot find taxonomy item"), "INCORRECTIDENTIFIER", ErrorCategory.ObjectNotFound, TermPath));

            }
        }
    }
}
