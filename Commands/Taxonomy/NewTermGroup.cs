using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.New, "PnPTermGroup", SupportsShouldProcess = false)]
    [CmdletHelp(@"Creates a taxonomy term group",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TermGroup),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx")]
    [CmdletExample
        (Code = @"PS:> New-PnPTermGroup -GroupName ""Countries""",
        Remarks = @"Creates a new taxonomy term group named ""Countries""",
        SortOrder = 1)]
    public class NewTermGroup : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Name of the taxonomy term group to create.")]
        [Alias("GroupName")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "GUID to use for the term group; if not specified, or the empty GUID, a random GUID is generated and used.")]
        [Alias("GroupId")]
        public Guid Id = Guid.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Description to use for the term group.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "Term store to add the group to; if not specified the default term store is used.")]
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
            // Create Group
            var group = termStore.CreateTermGroup(Name, Id, Description);

            WriteObject(group);
        }

    }
}
