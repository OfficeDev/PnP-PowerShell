using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.New, "PnPTerm", SupportsShouldProcess = false)]
    [CmdletHelp(@"Creates a taxonomy term",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(Term),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.term.aspx")]
    [CmdletExample
        (Code = @"PS:> New-PnPTerm -TermSet ""Departments"" -TermGroup ""Corporate"" -Name ""Finance""",
        Remarks = @"Creates a new taxonomy term named ""Finance"" in the termset Departments which is located in the ""Corporate"" termgroup",
        SortOrder = 1)]
    public class NewTerm : PnPCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The name of the term.")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "The Id to use for the term; if not specified, or the empty GUID, a random GUID is generated and used.")]
        public Guid Id = Guid.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The locale id to use for the term. Defaults to the current locale id.")]
        public int Lcid = CultureInfo.CurrentCulture.LCID;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The termset to add the term to.")]
        public TaxonomyItemPipeBind<TermSet> TermSet;

        [Parameter(Mandatory = true, ValueFromPipeline = true,
            HelpMessage = "The termgroup to create the term in.")]
        public TermGroupPipeBind TermGroup;

        
        [Parameter(Mandatory = false, HelpMessage = "Descriptive text to help users understand the intended use of this term.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage="Custom Properties")]
        public Hashtable CustomProperties;

        [Parameter(Mandatory = false, HelpMessage = "Custom Properties")]
        public Hashtable LocalCustomProperties;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Term store to check; if not specified the default term store is used.")]
        [Alias("TermStoreName")]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        protected override void ExecuteCmdlet()
        {
            var taxonomySession = TaxonomySession.GetTaxonomySession(ClientContext);
            // Get Term Store
            var termStore = default(TermStore);
            if (TermStore != null)
            {
                if (TermStore.IdValue != Guid.Empty)
                {
                    termStore = taxonomySession.TermStores.GetById(TermStore.IdValue);
                }
                else if (!string.IsNullOrEmpty(TermStore.StringValue))
                {
                    termStore = taxonomySession.TermStores.GetByName(TermStore.StringValue);
                }
                else
                {
                    termStore = TermStore.Item;
                }
            }
            else
            {
                termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
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

            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            var termName = TaxonomyExtensions.NormalizeName(Name);
            var term = termSet.CreateTerm(termName, Lcid, Id);
            ClientContext.Load(term);
            ClientContext.ExecuteQueryRetry();
            term.SetDescription(Description, Lcid);
            
            var customProperties  = CustomProperties ?? new Hashtable();
            foreach (var key in customProperties.Keys)
            {
                term.SetCustomProperty(key as string, customProperties[key] as string);
            }

            var localCustomProperties = LocalCustomProperties ?? new Hashtable();
            foreach (var key in localCustomProperties.Keys)
            {
                term.SetLocalCustomProperty(key as string, localCustomProperties[key] as string);
            }
            termStore.CommitAll();
            ClientContext.Load(term);
            ClientContext.ExecuteQueryRetry();
            WriteObject(term);
        }

    }
}
