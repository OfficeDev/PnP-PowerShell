#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPOrgAssetsLibrary")]
    [CmdletHelp("Returns the list of all the configured organizational asset libraries",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Get-PnPOrgAssetsLibrary",
     Remarks = @"Returns the list of all the configured organizational asset sites", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> (Get-PnPOrgAssetsLibrary)[0].OrgAssetsLibraries[0].LibraryUrl.DecodedUrl",
     Remarks = @"Returns the server relative url of the first document library which has been flagged as organizational asset library, i.e. ""sites/branding/logos""", SortOrder = 2)]
    //
    public class GetOrgAssetsLibrary : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var results = Tenant.GetOrgAssets();
            ClientContext.ExecuteQueryRetry();
            WriteObject(results.Value, true);
        }
    }
}
#endif