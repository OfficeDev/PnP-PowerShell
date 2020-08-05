#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantSequence", SupportsShouldProcess = true)]
    [CmdletHelp("Returns one ore more provisioning sequence object(s) from a tenant template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequence -Template $myTemplateObject",
       Remarks = "Returns all sequences from the specified tenant template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequence -Template $myTemplateObject -Identity ""mysequence""",
       Remarks = "Returns the specified sequence from the specified tenant template",
       SortOrder = 2)]
    public class GetTenantSequence : BasePSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The template to retrieve the sequence from", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ProvisioningHierarchy Template;

        [Parameter(Mandatory = false, HelpMessage = "Optional Id of the sequence", ParameterSetName = ParameterAttribute.AllParameterSets, ValueFromPipeline = true)]
        public ProvisioningSequencePipeBind Identity;
        protected override void ProcessRecord()
        {
            if (!ParameterSpecified(nameof(Identity)))
            {
                WriteObject(Template.Sequences, true);
            }
            else
            {
                WriteObject(Identity.GetSequenceFromHierarchy(Template));
            }
        }
    }
}
#endif