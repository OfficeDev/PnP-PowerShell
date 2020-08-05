#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteScript", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieve Site Scripts that have been registered on the current tenant.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScript",
        Remarks = "Returns all registered site scripts",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScript -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd",
        Remarks = "Returns a specific registered site script",
        SortOrder = 2)]
    public class GetSiteScript : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "If specified will retrieve the specified site script")]
        public TenantSiteScriptPipeBind Identity;

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "If specified will retrieve the site scripts for this design")]
        public TenantSiteDesignPipeBind SiteDesign;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                var script = Tenant.GetSiteScript(ClientContext, Identity.Id);
                script.EnsureProperties(s => s.Content, s => s.Title, s => s.Id, s => s.Version);
                WriteObject(script);
            }
            else if (ParameterSpecified(nameof(SiteDesign)))
            {
                var scripts = new List<TenantSiteScript>();
                var design = Tenant.GetSiteDesign(ClientContext, SiteDesign.Id);
                design.EnsureProperty(d => d.SiteScriptIds);
                foreach (var scriptId in design.SiteScriptIds)
                {
                    var script = Tenant.GetSiteScript(ClientContext, scriptId);
                    script.EnsureProperties(s => s.Content, s => s.Title, s => s.Id, s => s.Version);
                    scripts.Add(script);
                }
                WriteObject(scripts, true);
            }
            else
            {
                var scripts = Tenant.GetSiteScripts();
                ClientContext.Load(scripts, s => s.Include(sc => sc.Id, sc => sc.Title, sc => sc.Version, sc => sc.Content));
                ClientContext.ExecuteQueryRetry();
                WriteObject(scripts.ToList(), true);
            }
        }
    }
}
#endif