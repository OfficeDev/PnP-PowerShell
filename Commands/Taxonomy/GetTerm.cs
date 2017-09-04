using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPTerm", SupportsShouldProcess = false)]
    [CmdletHelp(@"Returns a taxonomy term",
         Category = CmdletHelpCategory.Taxonomy,
         OutputType = typeof(Term),
         OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.term.aspx")]
    [CmdletExample(
         Code = @"PS:> Get-PnPTerm -TermSet ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns all term in the termset ""Departments"" which is in the group ""Corporate"" from the site collection termstore",
         SortOrder = 0)]
    [CmdletExample(
         Code = @"PS:> Get-PnPTermSet -Identity ""Finance"" -TermSet ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns the term named ""Finance"" in the termset ""Departments"" from the termgroup called ""Corporate"" from the site collection termstore",
         SortOrder = 1)]
    [CmdletExample(
         Code = @"PS:> Get-PnPTermSet -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermSet ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns the termset named with the given id, from the ""Departments"" from termgroup called ""Corporate"" from the site collection termstore",
         SortOrder = 2)]
    public class GetTerm : PnPRetrievalsCmdlet<Term>
    {
        [Parameter(Mandatory = false, HelpMessage = "The Id or Name of a Term")]
        public GenericObjectNameIdPipeBind<TermSet> Identity;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the termset to check.")]
        public TaxonomyItemPipeBind<TermSet> TermSet;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the termgroup to check.")]
        public TermGroupPipeBind TermGroup;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Term, object>>[] { g => g.Name, g => g.Id };
            var taxonomySession = TaxonomySession.GetTaxonomySession(ClientContext);
            // Get Term Store
            TermStore termStore = null;
            if (TermStore == null)
            {
                termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            }
            else
            {
                if (TermStore.StringValue != null)
                {
                    termStore = taxonomySession.TermStores.GetByName(TermStore.StringValue);
                }
                else if (TermStore.IdValue != Guid.Empty)
                {
                    termStore = taxonomySession.TermStores.GetById(TermStore.IdValue);
                }
                else
                {
                    if (TermStore.Item != null)
                    {
                        termStore = TermStore.Item;
                    }
                }
            }

            TermGroup termGroup = null;

            if (TermGroup.Id != Guid.Empty)
            {
                termGroup = termStore.Groups.GetById(TermGroup.Id);
            }
            else if (!string.IsNullOrEmpty(TermGroup.Name))
            {
                termGroup = termStore.Groups.GetByName(TermGroup.Name);
            }

            TermSet termSet;
            if (TermSet.Id != Guid.Empty)
            {
                termSet = termGroup.TermSets.GetById(TermSet.Id);
            }
            else if (!string.IsNullOrEmpty(TermSet.Title))
            {
                termSet = termGroup.TermSets.GetByName(TermSet.Title);
            }
            else
            {
                termSet = TermSet.Item;
            }
            if (Identity != null)
            {
                Term term;
                if (Identity.IdValue != Guid.Empty)
                {
                    term = termSet.Terms.GetById(Identity.IdValue);
                }
                else
                {
                    var termName = TaxonomyExtensions.NormalizeName(Identity.StringValue);
                    term = termSet.Terms.GetByName(termName);
                }
                ClientContext.Load(term, RetrievalExpressions);
                ClientContext.ExecuteQueryRetry();
                WriteObject(term);
            }
            else
            {
                var query = termSet.Terms.IncludeWithDefaultProperties(RetrievalExpressions);
                var terms = ClientContext.LoadQuery(query);
                ClientContext.ExecuteQueryRetry();
                WriteObject(terms, true);

            }
        }
    }
}
