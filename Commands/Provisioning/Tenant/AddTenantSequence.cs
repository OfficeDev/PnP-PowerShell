#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Add, "PnPTenantSequence", SupportsShouldProcess = true)]
    [CmdletHelp("Adds a tenant sequence object to a tenant template",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPTenantSequence -Template $mytemplate -Sequence $mysequence",
       Remarks = "Adds an existing sequence object to an existing template object",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> New-PnPTenantSequence -Id ""MySequence"" | Add-PnPTenantSequence -Template $template",
       Remarks = "Creates a new instance of a provisioning sequence object and sets the Id to the value specified, then the sequence is added to an existing template object",
       SortOrder = 2)]
    public class AddTenantSequence : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The template to add the sequence to", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ProvisioningHierarchy Template;

        [Parameter(Mandatory = true, HelpMessage = "Optional Id of the sequence", ParameterSetName = ParameterAttribute.AllParameterSets, ValueFromPipeline = true)]
        public ProvisioningSequence Sequence;

        protected override void ProcessRecord()
        {
            if (Template.Sequences.FirstOrDefault(s => s.ID == Sequence.ID) == null)
            {
                Template.Sequences.Add(Sequence);
                WriteObject(Template);
            }
            else
            {
                WriteError(new ErrorRecord(new Exception($"Sequence with ID {Sequence.ID} already exists in template"), "DUPLICATESEQUENCEID", ErrorCategory.InvalidData, Sequence));
            }
        }
    }
}
#endif