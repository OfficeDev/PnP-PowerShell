#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteScriptFromList", SupportsShouldProcess = true)]
    [CmdletHelp(@"Generates a Site Script from an existing list",
        DetailedDescription = "This command allows a Site Script to be generated off of an existing list on your tenant. Connect to your SharePoint Online Admin site before executing this command.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScriptFromList -Url https://contoso.sharepoint.com/sites/teamsite/lists/MyList",
        Remarks = @"Returns the generated Site Script JSON from the list ""MyList"" at the provided Url",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScriptFromList -Url ""https://contoso.sharepoint.com/sites/teamsite/Shared Documents""",
        Remarks = "Returns the generated Site Script JSON from the default document library at the provided Url",
        SortOrder = 2)]
    public class GetSiteScriptFromList : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specifies the URL of the list to generate a Site Script from", ValueFromPipeline = true)]
        public string Url;

        protected override void ExecuteCmdlet()
        {
            var script = Tenant.GetSiteScriptFromList(ClientContext, Url);
            ClientContext.ExecuteQueryRetry();
            WriteObject(script.Value);
        }
    }
}
#endif