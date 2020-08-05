#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.New, "PnPTenantTemplate", SupportsShouldProcess = true)]
    [CmdletHelp("Creates a new tenant template object",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> $template = New-PnPTenantTemplate",
       Remarks = "Creates a new instance of a tenant template object.",
       SortOrder = 1)]
    public class NewTenantTemplate : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Author;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false)]
        public string DisplayName;

        [Parameter(Mandatory = false)]
        public string Generator;

        protected override void ProcessRecord()
        {
            var result = new ProvisioningHierarchy();
            result.Author = Author;
            result.Description = Description;
            result.DisplayName = DisplayName;
            result.Generator = Generator;
            WriteObject(result);
        }
    }
}
#endif