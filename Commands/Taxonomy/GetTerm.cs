using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Taxonomy
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
         Code = @"PS:> Get-PnPTerm -Identity ""Finance"" -TermSet ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns the term named ""Finance"" in the termset ""Departments"" from the termgroup called ""Corporate"" from the site collection termstore",
         SortOrder = 1)]
    [CmdletExample(
         Code = @"PS:> Get-PnPTerm -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermSet ""Departments"" -TermGroup ""Corporate""",
         Remarks = @"Returns the term named with the given id, from the ""Departments"" termset in a term group called ""Corporate"" from the site collection termstore",
         SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTerm -Identity ""Small Finance"" -TermSet ""Departments"" -TermGroup ""Corporate"" -Recursive",
        Remarks = @"Returns the term named ""Small Finance"", from the ""Departments"" termset in a term group called ""Corporate"" from the site collection termstore even if it's a subterm below ""Finance""",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> $term = Get-PnPTerm -Identity ""Small Finance"" -TermSet ""Departments"" -TermGroup ""Corporate"" -Include Labels
PS:> $term.Labels",
        Remarks = @"Returns all the localized labels for the term named ""Small Finance"", from the ""Departments"" termset in a term group called ""Corporate""",
        SortOrder = 4)]
    public class GetTerm : PnPRetrievalsCmdlet<Term>
    {
        private const string ParameterSet_TERM = "By Term Id";
        private const string ParameterSet_TERMSET = "By Termset";
        private Term term;

        [Parameter(Mandatory = true, HelpMessage = "The Id or Name of a Term", ParameterSetName = ParameterSet_TERM)]
        [Parameter(Mandatory = false, HelpMessage = "The Id or Name of a Term", ParameterSetName = ParameterSet_TERMSET)]
        public GenericObjectNameIdPipeBind<TermSet> Identity;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the termset to check.", ParameterSetName = ParameterSet_TERMSET)]
        public TaxonomyItemPipeBind<TermSet> TermSet;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the termgroup to check.", ParameterSetName = ParameterSet_TERMSET)]
        public TermGroupPipeBind TermGroup;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.", ParameterSetName = ParameterSet_TERM)]
        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.", ParameterSetName = ParameterSet_TERMSET)]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        [Parameter(Mandatory = false, HelpMessage = "Find the first term recursively matching the label in a term hierarchy.", ParameterSetName = ParameterSet_TERMSET)]
        public SwitchParameter Recursive;

        [Parameter(Mandatory = false, HelpMessage = "Includes the hierarchy of child terms if available", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter IncludeChildTerms;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Term, object>>[] { g => g.Name, g => g.TermsCount, g => g.Id };
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

            if (ParameterSetName == ParameterSet_TERM)
            {
                if (Identity.IdValue != Guid.Empty)
                {
                    term = termStore.GetTerm(Identity.IdValue);
                    ClientContext.Load(term, RetrievalExpressions);
                    ClientContext.ExecuteQueryRetry();
                    if (IncludeChildTerms.IsPresent && term.TermsCount > 0)
                    {
                        LoadChildTerms(term);
                    }
                    WriteObject(term);
                } else
                {
                    WriteError(new ErrorRecord(new Exception("Insufficient parameters"), "INSUFFICIENTPARAMETERS", ErrorCategory.SyntaxError, this));
                }
            }
            else
            {
                TermGroup termGroup = null;

                if (TermGroup != null && TermGroup.Id != Guid.Empty)
                {
                    termGroup = termStore.Groups.GetById(TermGroup.Id);
                }
                else if (TermGroup != null && !string.IsNullOrEmpty(TermGroup.Name))
                {
                    termGroup = termStore.Groups.GetByName(TermGroup.Name);
                }

                TermSet termSet = null;
                if (TermSet != null)
                {
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
                }
                if (Identity != null)
                {
                    term = null;
                    if (Identity.IdValue != Guid.Empty)
                    {
                        term = termStore.GetTerm(Identity.IdValue);
                    }
                    else
                    {
                        var termName = TaxonomyExtensions.NormalizeName(Identity.StringValue);
                        if (!Recursive)
                        {
                            term = termSet.Terms.GetByName(termName);
                        }
                        else
                        {
                            var lmi = new LabelMatchInformation(ClientContext)
                            {
                                TrimUnavailable = true,
                                TermLabel = termName
                            };

                            var termMatches = termSet.GetTerms(lmi);
                            ClientContext.Load(termMatches);
                            ClientContext.ExecuteQueryRetry();

                            if (termMatches.AreItemsAvailable)
                            {
                                term = termMatches.FirstOrDefault();
                            }
                        }
                    }
                    ClientContext.Load(term, RetrievalExpressions);
                    ClientContext.ExecuteQueryRetry();
                    if (IncludeChildTerms.IsPresent && term.TermsCount > 0)
                    {
                        LoadChildTerms(term);
                    }
                    WriteObject(term);
                }
                else
                {
                    var query = termSet.Terms.IncludeWithDefaultProperties(RetrievalExpressions);
                    var terms = ClientContext.LoadQuery(query);
                    ClientContext.ExecuteQueryRetry();
                    if (IncludeChildTerms.IsPresent)
                    {
                        foreach (var collectionTerm in terms)
                        {
                            if (collectionTerm.TermsCount > 0)
                            {
                                LoadChildTerms(collectionTerm);
                            }
                        }
                    }
                    WriteObject(terms, true);
                }
            }
        }

        private void LoadChildTerms(Term incomingTerm)
        {
            ClientContext.Load(incomingTerm.Terms, ts => ts.IncludeWithDefaultProperties(RetrievalExpressions));
            ClientContext.ExecuteQueryRetry();
            foreach (var childTerm in incomingTerm.Terms)
            {
                if (childTerm.TermsCount > 0)
                {
                    LoadChildTerms(childTerm);
                }
            }
        }
    }
}