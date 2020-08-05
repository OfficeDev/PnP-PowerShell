#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteScript", SupportsShouldProcess = true)]
    [CmdletHelp(@"Creates a new Site Script on the current tenant.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Add-PnPSiteScript -Title ""My Site Script"" -Description ""A more detailed description"" -Content $script",
        Remarks = "Adds a new Site Script, where $script variable contains the script.",
        SortOrder = 1)]
    public class AddSiteScript : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The title of the site script")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The description of the site script")]
        public string Description;     

        [Parameter(Mandatory = true, HelpMessage = "A JSON string containing the site script")]
        public string Content;

        protected override void ExecuteCmdlet()
        {
            TenantSiteScriptCreationInfo siteScriptCreationInfo = new TenantSiteScriptCreationInfo
            {
                Title = Title,
                Description = Description,
                Content = Content
            };
            var script = Tenant.CreateSiteScript(siteScriptCreationInfo);
            ClientContext.Load(script);
            ClientContext.ExecuteQueryRetry();
            WriteObject(script);
        }
    }
}         
#endif