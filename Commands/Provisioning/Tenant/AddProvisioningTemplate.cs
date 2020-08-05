#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Add, "PnPProvisioningTemplate", SupportsShouldProcess = true)]
    [CmdletHelp("Adds a PnP Provisioning Template object to a tenant template",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnpProvisioningTemplate -TenantTemplate $tenanttemplate -SiteTemplate $sitetemplate",
       Remarks = "Adds an existing site template to an existing tenant template object",
       SortOrder = 1)]
    public class AddProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The template to add to the tenant template")]
        public ProvisioningTemplate SiteTemplate;

        [Parameter(Mandatory = true, HelpMessage = "The tenant template to add the template to", ValueFromPipeline = true)]
        public ProvisioningHierarchy TenantTemplate;

        protected override void ProcessRecord()
        {
            if(TenantTemplate.Templates.FirstOrDefault(t => t.Id == SiteTemplate.Id) == null)
            {
                TenantTemplate.Templates.Add(SiteTemplate);
            } else { 
                WriteError(new ErrorRecord(new Exception($"Template with ID {SiteTemplate.Id} already exists in template"), "DUPLICATETEMPLATE", ErrorCategory.InvalidData, SiteTemplate));
            }
        }
    }
}
#endif