#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Add, "PnPTenantSequenceSite", SupportsShouldProcess = true)]
    [CmdletHelp("Adds a existing tenant sequence site object to a tenant template",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPTenantSequenceSite -Site $myteamsite -Sequence $mysequence",
       Remarks = "Adds an existing site object to an existing template sequence",
        SortOrder = 1)]
    public class AddTenantSequenceSite : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public ProvisioningSitePipeBind Site;

        [Parameter(Mandatory = true, HelpMessage = "The sequence to add the site to", ValueFromPipeline = true)]
        public ProvisioningSequence Sequence;

        protected override void ProcessRecord()
        {
            Sequence.SiteCollections.Add(Site.Site);
            WriteObject(Sequence);
        }
    }
}
#endif