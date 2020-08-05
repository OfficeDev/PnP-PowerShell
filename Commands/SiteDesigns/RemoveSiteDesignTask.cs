#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPSiteDesignTask", SupportsShouldProcess = true)]
    [CmdletHelp(@"Removes a Site Design Task",
        Category = CmdletHelpCategory.TenantAdmin,
        Description = @"Removes a Site Design Task. If the execution of the associated site script has already started the execution will not be terminated.",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSiteDesignTask -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Removes the specified site design task",
        SortOrder = 1)]
    public class RemoveSiteDesignTask : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The ID of the site design to remove")]
        public TenantSiteDesignTaskPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If specified you will not be asked to confirm removing the specified Site Design")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(Properties.Resources.RemoveSiteDesignTask, Properties.Resources.Confirm))
            {
                Tenant.RemoveSiteDesignTask(ClientContext, Identity.Id);
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}         
#endif