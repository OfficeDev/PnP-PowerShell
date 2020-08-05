using System;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPTermSet", SupportsShouldProcess = false)]
    [CmdletHelp(@"Returns a taxonomy term set",
         Category = CmdletHelpCategory.Taxonomy,
         OutputType = typeof(TermSet),
         OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx")]
    [CmdletExample(
         Code = @"PS:> Get-PnPTermSet -TermGroup ""Corporate""",
         Remarks = @"Returns all termsets in the group ""Corporate"" from the site collection termstore",
         SortOrder = 0)]
    [CmdletExample(
         Code = @"PS:> Get-PnPTermSet -Identity ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns the termset named ""Departments"" from the termgroup called ""Corporate"" from the site collection termstore",
         SortOrder = 1)]
    [CmdletExample(
         Code = @"PS:> Get-PnPTermSet -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermGroup ""Corporate",
         Remarks = @"Returns the termset with the given id from the termgroup called ""Corporate"" from the site collection termstore",
         SortOrder = 2)]
    public class GetTermSet : PnPRetrievalsCmdlet<TermSet>
    {
        [Parameter(Mandatory = false, HelpMessage = "The Id or Name of a termset")]
        public GenericObjectNameIdPipeBind<TermSet> Identity;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the term group to check.")]
        public TermGroupPipeBind TermGroup;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<TermSet, object>>[] { g => g.Name, g => g.Id };
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
            if (Identity != null)
            {
                var termSet = default(TermSet);
                if (Identity.IdValue != Guid.Empty)
                {
                    termSet = termGroup.TermSets.GetById(Identity.IdValue);
                }
                else
                {
                    termSet = termGroup.TermSets.GetByName(Identity.StringValue);
                }
                ClientContext.Load(termSet, RetrievalExpressions);
                ClientContext.ExecuteQueryRetry();
                WriteObject(termSet);
            }
            else
            {
                var query = termGroup.TermSets.IncludeWithDefaultProperties(RetrievalExpressions);
                var termSets = ClientContext.LoadQuery(query);
                ClientContext.ExecuteQueryRetry();
                WriteObject(termSets, true);

            }

        }

    }
}
