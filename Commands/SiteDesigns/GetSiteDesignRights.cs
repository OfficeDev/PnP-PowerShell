#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteDesignRights", SupportsShouldProcess = true)]
    [CmdletHelp(@"Returns the principals with design rights on a specific Site Design",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesignRights -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Returns the principals with rights on a specific site design",
        SortOrder = 1)]
    public class GetSiteDesignRights : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline =true, HelpMessage = "The ID of the Site Design to receive the rights for")]
        public TenantSiteDesignPipeBind Identity;
        
        protected override void ExecuteCmdlet()
        {
            var principles = Tenant.GetSiteDesignRights(ClientContext,Identity.Id);
            ClientContext.Load(principles);
            ClientContext.ExecuteQueryRetry();
            WriteObject(principles, true);
        }
    }
}         
#endif