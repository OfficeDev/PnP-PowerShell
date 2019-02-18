#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.SiteDesigns
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteDesignRunStatus", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieves and displays a list of all site script actions executed for a specified site design applied to a site. ",
       Category = CmdletHelpCategory.TenantAdmin,
        Description = @"Retrieves and displays a list of all site script actions executed for a specified site design applied to a site.",
       SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> $myrun = Get-PnPSiteDesignRun -WebUrl ""https://contoso.sharepoint.com/sites/project-playbook"" -SiteDesignId cefd782e-sean-4814-a68a-b33b116c302f
PS:> Get-PnPSiteDesignRunStatus -Run $myrun",
       Remarks = "This example gets the run for a specific site design applied to a site and sets it to a variable. This variable is then passed into the command -Run parameter. The result is a display of all the site script actions applied for that site design run, including the script action title and outcome. ",
       SortOrder = 1)]
    public class GetSiteDesignRunStatus : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The site design run for the desired set of script action details.")]
        public TenantSiteDesignRun Run;

        protected override void ExecuteCmdlet()
        {
            var status = Tenant.GetSiteDesignRunStatus(Run.SiteId, Run.WebId, Run.Id);
            Tenant.Context.Load(status);
            Tenant.Context.ExecuteQueryRetry();
            WriteObject(status, true);
        }
    }
}
#endif