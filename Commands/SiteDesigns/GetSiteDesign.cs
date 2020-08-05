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
    [Cmdlet(VerbsCommon.Get, "PnPSiteDesign", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieve Site Designs that have been registered on the current tenant.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesign",
        Remarks = "Returns all registered site designs",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesign -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Returns a specific registered site designs",
        SortOrder = 2)]
    public class GetSiteDesign : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "If specified will retrieve the specified site design")]
        public TenantSiteDesignPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                WriteObject(Identity.GetTenantSiteDesign(Tenant));
            }
            else
            {
                var designs = Tenant.GetSiteDesigns();
                ClientContext.Load(designs);
                ClientContext.ExecuteQueryRetry();
                WriteObject(designs.ToList(), true);
            }
        }
    }
}         
#endif