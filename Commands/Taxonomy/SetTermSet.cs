using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using OfficeDevPnP.Core.Extensions;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Set, "PnPTermSet", SupportsShouldProcess = false)]
    [CmdletHelp(@"Updates a taxonomy term set",
         Category = CmdletHelpCategory.Taxonomy,
         OutputType = typeof(TermSet),
         OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx")]
    [CmdletExample(
         Code = @"PS:> Set-PnPTermSet -TermGroup ""Corporate"" -Identity ""Finance"" -Name ""Financial""",
         Remarks = @"Updates the termset called ""Finance"" and renames it to ""Financial""",
         SortOrder = 0)]
    public class SetTermSet : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The Id or Name of a termset")]
        public GenericObjectNameIdPipeBind<TermSet> Identity;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the term group to check.")]
        public TermGroupPipeBind TermGroup;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public GenericObjectNameIdPipeBind<TermStore> TermStore;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public string Owner;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public string Contact;

        [Parameter(Mandatory = false, HelpMessage = "Sets custom properties for this term set. Notice setting this will replace all existing custom properties.")]
        public Hashtable CustomProperties;

        [Parameter(Mandatory = false, HelpMessage = "Adds a new Stakeholder for this termset")]
        public string StakeholderToAdd;

        [Parameter(Mandatory = false, HelpMessage = "Deletes an existing Stakeholder for this termset")]
        public string StakeholderToDelete;

        [Parameter(Mandatory = false, HelpMessage = "Sets if the term set is avialble for tagging")]
        public bool IsAvailableForTagging;

        [Parameter(Mandatory = false, HelpMessage = "Sets if the termset is open for term creation")]
        public bool IsOpenForTermCreation;

        [Parameter(Mandatory = false, HelpMessage = "Sets if the termset is to be used for site navigation")]
        public bool UseForSiteNavigation;

        [Parameter(Mandatory = false, HelpMessage = "Sets if the termset is to be used for faceted navigation")]
        public bool UseForFacetedNavigation;

        [Parameter(Mandatory = false, HelpMessage = "Sets the target page for terms in this termset")]
        public string SetTargetPageForTerms;

        [Parameter(Mandatory = false, HelpMessage = "Removes the target page for terms in this termset")]
        public SwitchParameter RemoveTargetPageforTerms;

        [Parameter(Mandatory = false, HelpMessage = "Sets the page for categories in this termset")]
        public string SetCatalogItemPageForCategories;

        [Parameter(Mandatory = false, HelpMessage = "Removes the page for categories in this termset")]
        public SwitchParameter RemoveCatalogItemPageForCategories;
        protected override void ExecuteCmdlet()
        {
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

                ClientContext.Load(termSet, t => t.CustomProperties);
                ClientContext.ExecuteQueryRetry();
                if (termSet.ServerObjectIsNull.Value == false)
                {
                    bool updateRequired = false;
                    if (ParameterSpecified(nameof(Name)))
                    {
                        termSet.Name = Name;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(Description)))
                    {
                        termSet.Description = Description;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(Owner)))
                    {
                        termSet.Owner = Owner;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(Contact)))
                    {
                        termSet.Contact = Contact;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(CustomProperties)))
                    {
                        var enumerator = termSet.CustomProperties.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            var prop = enumerator.Current;
                            if (!prop.Key.StartsWith("_Sys_"))
                            {
                                termSet.DeleteCustomProperty(prop.Key);
                            }
                        }

                        foreach (var entry in CustomProperties.Cast<DictionaryEntry>().ToDictionary(kvp => (string)kvp.Key, kvp => (string)kvp.Value))
                        {
                            termSet.SetCustomProperty(entry.Key, entry.Value);
                        }
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(StakeholderToAdd)))
                    {
                        termSet.AddStakeholder(StakeholderToAdd);
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(StakeholderToDelete)))
                    {
                        termSet.DeleteStakeholder(StakeholderToDelete);
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(IsAvailableForTagging)))
                    {
                        termSet.IsAvailableForTagging = IsAvailableForTagging;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(IsOpenForTermCreation)))
                    {
                        termSet.IsOpenForTermCreation = IsOpenForTermCreation;
                        updateRequired = true;
                    }
                    if (ParameterSpecified(nameof(UseForSiteNavigation)))
                    {
                        if (UseForSiteNavigation)
                        {
                            if (!termSet.CustomProperties.ContainsKey("_Sys_Nav_IsNavigationTermSet"))
                            {
                                termSet.SetCustomProperty("_Sys_Nav_IsNavigationTermSet", "True");
                                updateRequired = true;
                            }
                        }
                        else
                        {
                            if (termSet.CustomProperties.ContainsKey("_Sys_Nav_IsNavigationTermSet"))
                            {
                                termSet.DeleteCustomProperty("_Sys_Nav_IsNavigationTermSet");
                                updateRequired = true;
                            }
                        }
                    }
                    if (ParameterSpecified(nameof(UseForFacetedNavigation)))
                    {
                        if (UseForFacetedNavigation)
                        {
                            if (!termSet.CustomProperties.ContainsKey("_Sys_Facet_IsFacetedTermSet"))
                            {
                                termSet.SetCustomProperty("_Sys_Facet_IsFacetedTermSet", "True");
                                updateRequired = true;
                            }
                        }
                        else
                        {
                            if (termSet.CustomProperties.ContainsKey("_Sys_Facet_IsFacetedTermSet"))
                            {
                                termSet.DeleteCustomProperty("_Sys_Facet_IsFacetedTermSet");
                                updateRequired = true;
                            }
                        }
                    }
                    if (ParameterSpecified(nameof(SetTargetPageForTerms)) && ParameterSpecified(nameof(RemoveTargetPageforTerms)))
                    {
                        throw new PSArgumentException("Cannot both set and remove the target page for this termset. Either use SetTargetPageForTerms or RemoveTargetPageForTerms");
                    }
                    else
                    {
                        if (ParameterSpecified(nameof(SetTargetPageForTerms)))
                        {
                            termSet.SetCustomProperty("_Sys_Nav_TargetUrlForChildTerms", SetTargetPageForTerms);
                            updateRequired = true;
                        }
                        if(ParameterSpecified(nameof(RemoveTargetPageforTerms)))
                        {
                            termSet.DeleteCustomProperty("_Sys_Nav_TargetUrlForChildTerms");
                            updateRequired = true;
                        }
                    }

                    if (ParameterSpecified(nameof(SetCatalogItemPageForCategories)) && ParameterSpecified(nameof(RemoveCatalogItemPageForCategories)))
                    {
                        throw new PSArgumentException("Cannot both set and remove the catalog page for this termset. Either use SetCatalogItemPageForCategories or RemoveCatalogItemPageForCategories");
                    }
                    else
                    {
                        if (ParameterSpecified(nameof(SetCatalogItemPageForCategories)))
                        {
                            termSet.SetCustomProperty("_Sys_Nav_CatalogTargetUrlForChildTerms", SetTargetPageForTerms);
                            updateRequired = true;
                        }
                        if (ParameterSpecified(nameof(RemoveCatalogItemPageForCategories)))
                        {
                            termSet.DeleteCustomProperty("_Sys_Nav_CatalogTargetUrlForChildTerms");
                            updateRequired = true;
                        }
                    }

                    if (updateRequired)
                    {
                        termStore.CommitAll();
                        ClientContext.ExecuteQuery();
                    }
                }
                else
                {
                    throw new PSArgumentException("Cannot find termset");
                }
            }
        }

    }
}
