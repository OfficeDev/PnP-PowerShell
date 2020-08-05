#if !ONPREMISES
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using System;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPIsSiteAliasAvailable")]
    [CmdletHelp("Validates if a certain alias is still available to be used to create a new site collection for. If it is not, it will propose an alternative alias and URL which is still available.",
        OutputTypeDescription = "Returns a boolean IsAvailable indicating if the provided alias is still available to be used for a new site collection, a string ProposedUrl with the full proposed URL and a string ProposedAlias with just the alias to use instead if the provided alias is not available",
        Category = CmdletHelpCategory.TenantAdmin, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPIsSiteAliasAvailable -Identity ""HR""",
        Remarks = @"Validates if the alias ""HR"" is still available to be used",
        SortOrder = 1)]
    public class GetIsSiteAliasAvailable : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Alias you want to check for if it is still available to create a new site collection for", ValueFromPipeline = true)]
        [Alias("Alias")]
        public string Identity;

        protected override void ExecuteCmdlet()
        {
            var proposedUrl = OfficeDevPnP.Core.Sites.SiteCollection.GetValidSiteUrlFromAliasAsync(ClientContext, Identity).Result;

            var record = new PSObject();
            record.Properties.Add(new PSVariableProperty(new PSVariable("IsAvailable", proposedUrl.EndsWith($"/{Identity}", StringComparison.InvariantCultureIgnoreCase))));
            record.Properties.Add(new PSVariableProperty(new PSVariable("ProposedAlias", proposedUrl.Remove(0, proposedUrl.LastIndexOf('/') + 1))));
            record.Properties.Add(new PSVariableProperty(new PSVariable("ProposedUrl", proposedUrl)));

            WriteObject(record);
        }
    }
}
#endif
