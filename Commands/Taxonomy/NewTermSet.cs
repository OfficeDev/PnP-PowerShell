using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.New, "PnPTermSet", SupportsShouldProcess = false)]
    [CmdletHelp(@"Creates a taxonomy term set",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TermSet),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx")]
    [CmdletExample
        (Code = @"PS:> New-PnPTermSet -Name ""Department"" -TermGroup ""Corporate""",
        Remarks = @"Creates a new termset named ""Department"" in the group named ""Corporate""",
        SortOrder = 1)]
    public class NewTermSet : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The name of the termset.")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "The Id to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.")]
        public Guid Id = default(Guid);

        [Parameter(Mandatory = false, HelpMessage = "The locale id to use for the term set. Defaults to the current locale id.")]
        public int Lcid = CultureInfo.CurrentCulture.LCID;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Name, id or actually termgroup to create the termset in.")]
        public TermGroupPipeBind TermGroup;

        [Parameter(Mandatory = false, HelpMessage = "An e-mail address for term suggestion and feedback. If left blank the suggestion feature will be disabled.")]
        public string Contact;

        [Parameter(Mandatory = false, HelpMessage = "Descriptive text to help users understand the intended use of this term set.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "When a term set is closed, only metadata managers can add terms to this term set. When it is open, users can add terms from a tagging application. Not specifying this switch will make the term set closed.")]
        public SwitchParameter IsOpenForTermCreation;

        [Parameter(Mandatory = false, HelpMessage = "By default a term set is available to be used by end users and content editors of sites consuming this term set. Specify this switch to turn this off")]
        public SwitchParameter IsNotAvailableForTagging;

        [Parameter(Mandatory = false, HelpMessage = "The primary user or group of this term set.")]
        public string Owner;

        [Parameter(Mandatory = false, HelpMessage = "People and groups in the organization that should be notified before major changes are made to the term set. You can enter multiple users or groups.")]
        public string[] StakeHolders;

        [Parameter(Mandatory = false)]
        public Hashtable CustomProperties;

        [Parameter(Mandatory = false, HelpMessage = "Term store to check; if not specified the default term store is used.")]
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

            var termGroup = default(TermGroup);
            if (TermGroup != null)
            {
                if (!string.IsNullOrEmpty(TermGroup.Name))
                {
                    termGroup = termStore.Groups.GetByName(TermGroup.Name);
                }
                else if (TermGroup.Id != Guid.Empty)
                {
                    termGroup = termStore.Groups.GetById(TermGroup.Id);
                }
            }
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            var termSet = termGroup.CreateTermSet(Name, Id, Lcid);

            ClientContext.Load(termSet);
            ClientContext.ExecuteQueryRetry();

            termSet.Contact = Contact;
            termSet.Description = Description;
            termSet.IsOpenForTermCreation = IsOpenForTermCreation;

            var customProperties = CustomProperties ?? new Hashtable();
            foreach (var key in customProperties.Keys)
            {
                termSet.SetCustomProperty(key as string, customProperties[key] as string);
            }
            if (IsNotAvailableForTagging)
            {
                termSet.IsAvailableForTagging = false;
            }
            if (!string.IsNullOrEmpty(Owner))
            {
                termSet.Owner = Owner;
            }

            if (StakeHolders != null)
            {
                foreach (var stakeHolder in StakeHolders)
                {
                    termSet.AddStakeholder(stakeHolder);
                }
            }

            termStore.CommitAll();
            ClientContext.ExecuteQueryRetry();
            WriteObject(termSet);
        }

    }
}
