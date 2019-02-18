#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantSequence", SupportsShouldProcess = true)]
    [Alias("Get-PnPProvisioningSequence")]
    [CmdletHelp("Returns one ore more provisioning sequence object(s) from a tenant template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequence -Template $myhierarchy",
       Remarks = "Returns all sequences from the specified tenant template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantSequence -Template $myhierarchy -Identity ""mysequence""",
       Remarks = "Returns the specified sequence from the specified tenant template",
       SortOrder = 2)]
    public class GetTenantSequence : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The hierarchy to retrieve the sequence from", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ProvisioningHierarchy Template;

        [Parameter(Mandatory = false, HelpMessage = "Optional Id of the sequence", ParameterSetName = ParameterAttribute.AllParameterSets, ValueFromPipeline = true)]
        public ProvisioningSequencePipeBind Identity;
        protected override void ProcessRecord()
        {

            if (MyInvocation.InvocationName.ToLower() == "get-pnpprovisioningsequence")
            {
                WriteWarning("Get-PnPProvisioningSequence has been deprecated. Use Get-PnPTenantSequence instead.");
            }
            if (!MyInvocation.BoundParameters.ContainsKey("Identity"))
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