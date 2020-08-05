using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.New, "PnPProvisioningTemplate", SupportsShouldProcess = true)]
    [CmdletHelp("Creates a new provisioning template object",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> $template = New-PnPProvisioningTemplate",
       Remarks = "Creates a new instance of a site template object.",
       SortOrder = 1)]
    public class NewProvisioningTemplate : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var result = new ProvisioningTemplate();
            WriteObject(result);
        }
    }
}
