#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantSequenceSite", SupportsShouldProcess = true)]
    [CmdletHelp("Returns one ore more sites from a tenant template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequenceSite -Sequence $mysequence",
       Remarks = "Returns all sites from the specified sequence",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequenceSite -Sequence $mysequence -Identity 8058ea99-af7b-4bb7-b12a-78f93398041e",
       Remarks = "Returns the specified site from the specified sequence",
       SortOrder = 2)]
    public class GetTenantSequenceSite : BasePSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The sequence to retrieve the site from", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ProvisioningSequence Sequence;

        [Parameter(Mandatory = false, HelpMessage = "Optional Id of the site", ParameterSetName = ParameterAttribute.AllParameterSets, ValueFromPipeline = true)]
        public ProvisioningSitePipeBind Identity;
        protected override void ProcessRecord()
        {
            if (!ParameterSpecified(nameof(Identity)))
            {
                WriteObject(Sequence.SiteCollections, true);
            }
            else
            {
                WriteObject(Identity.GetSiteFromSequence(Sequence));
            }
        }
    }
}
#endif