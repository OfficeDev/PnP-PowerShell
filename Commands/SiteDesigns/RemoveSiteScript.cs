#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPSiteScript", SupportsShouldProcess = true)]
    [CmdletHelp(@"Removes a Site Script",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSiteScript -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Removes the specified site script",
        SortOrder = 1)]
    public class RemoveSiteScript : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The ID of the Site Script to remove")]
        public TenantSiteScriptPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If specified you will not be asked to confirm removing the specified Site Script")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(Properties.Resources.RemoveSiteScript, Properties.Resources.Confirm))
            {
                Tenant.DeleteSiteScript(Identity.Id);
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}         
#endif