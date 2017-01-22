using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.New, "PnPProvisioningTemplate", SupportsShouldProcess = true)]
    [CmdletAlias("New-SPOProvisioningTemplate")]
    [CmdletHelp("Creates a new provisioning template object",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> $template = New-PnPProvisioningTemplate",
       Remarks = "Creates a new instance of a provisioning template object.",
       SortOrder = 1)]
    public class NewProvisioningTemplate : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var result = new ProvisioningTemplate();
            WriteObject(result);
        }
    }
}
