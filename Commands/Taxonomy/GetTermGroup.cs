using System;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPTermGroup", SupportsShouldProcess = false)]
    [CmdletHelp(@"Returns a taxonomy term group",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TermGroup),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPTermGroup",
        Remarks = @"Returns all Term Groups in the site collection termstore",
        SortOrder = 0)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTermGroup -Identity ""Departments""",
        Remarks = @"Returns the termgroup named ""Departments"" from the site collection termstore",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTermGroup -Identity ab2af486-e097-4b4a-9444-527b251f1f8d",
        Remarks = @"Returns the termgroup with the given ID from the site collection termstore",
        SortOrder = 2)]
    public class GetTermGroup : PnPRetrievalsCmdlet<TermGroup>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the taxonomy term group to retrieve.")]
        [Alias("GroupName")]
        public TaxonomyItemPipeBind<TermGroup> Identity;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        [Alias("TermStoreName")]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<TermGroup, object>>[] { g => g.Name, g => g.Id };
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
            // Get Group
            if (termStore != null)
            {

                if (Identity != null)
                {
                    TermGroup group = null;
                    if (Identity.Id != Guid.Empty)
                    {
                        group = termStore.Groups.GetById(Identity.Id);
                    }
                    else
                    {
                        group = termStore.Groups.GetByName(Identity.Title);
                    }
                    group.EnsureProperties(RetrievalExpressions);
                    WriteObject(group);
                }
                else
                {
                    var query = termStore.Groups.IncludeWithDefaultProperties(RetrievalExpressions);
                    var termGroups = ClientContext.LoadQuery(query);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(termGroups, true);
                }
            }
            else
            {
                WriteError(new ErrorRecord(new ArgumentException("Cannot find termstore"), "INCORRECTTERMSTORE", ErrorCategory.ObjectNotFound, TermStore));
            }
        }

    }
}
