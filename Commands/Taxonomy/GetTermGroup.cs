using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPTermGroup", SupportsShouldProcess = false)]
    [CmdletAlias("Get-SPOTermGroup")]
    [CmdletHelp(@"Returns a taxonomy term group",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(TermGroup),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx")]
    public class GetTermGroup : SPOCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0,
            HelpMessage = "Name of the taxonomy term group to retrieve.")]
        public string GroupName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Term store to check; if not specified the default term store is used.")]
        public string TermStoreName;

        protected override void ExecuteCmdlet()
        {
            var taxonomySession = TaxonomySession.GetTaxonomySession(ClientContext);
            // Get Term Store
            TermStore termStore;
            if (string.IsNullOrEmpty(TermStoreName))
            {
                termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            }
            else
            {
                termStore = taxonomySession.TermStores.GetByName(TermStoreName);
            }
            // Get Group
            if (termStore != null)
            {
                var group = termStore.GetTermGroupByName(GroupName);
                group.EnsureProperties(g => g.Name, g => g.Id);
                WriteObject(@group);
            }
            else
            {
                WriteError(new ErrorRecord(new Exception("Cannot find termstore"), "INCORRECTTERMSTORE", ErrorCategory.ObjectNotFound, TermStoreName));
            }
        }

    }
}
