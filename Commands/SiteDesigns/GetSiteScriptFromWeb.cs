#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteScriptFromWeb", SupportsShouldProcess = true)]
    [CmdletHelp(@"Generates a Site Script from an existing site",
        DetailedDescription = "This command allows a Site Script to be generated off of an existing site on your tenant. Connect to your SharePoint Online Admin site before executing this command.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScriptFromWeb -Url https://contoso.sharepoint.com/sites/teamsite -IncludeAll",
        Remarks = "Returns the generated Site Script JSON containing all supported components from the site at the provided Url",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScriptFromWeb -Url https://contoso.sharepoint.com/sites/teamsite -IncludeAll -Lists ""Shared Documents"",""Lists\MyList""",
        Remarks = @"Returns the generated Site Script JSON containing all supported components from the site at the provided Url including the lists ""Shared Documents"" and ""MyList""",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteScriptFromWeb -Url https://contoso.sharepoint.com/sites/teamsite -IncludeBranding -IncludeLinksToExportedItems",
        Remarks = "Returns the generated Site Script JSON containing the branding and navigation links from the site at the provided Url",
        SortOrder = 3)]
    public class GetSiteScriptFromWeb : PnPAdminCmdlet
    {
        private const string ParameterSet_ALLCOMPONENTS = "All components";
        private const string ParameterSet_SPECIFICCOMPONENTS = "Specific components";

        [Parameter(Mandatory = true, HelpMessage = "Specifies the URL of the site to generate a Site Script from", ValueFromPipeline = true)]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = @"Allows specifying one or more site relative URLs of lists that should be included into the Site Script, i.e. ""Shared Documents"",""List\MyList""")]
        public string[] Lists;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include all supported components into the Site Script", ParameterSetName = ParameterSet_ALLCOMPONENTS)]
        public SwitchParameter IncludeAll;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include the branding of the site into the Site Script", ParameterSetName = ParameterSet_SPECIFICCOMPONENTS)]
        public SwitchParameter IncludeBranding;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include navigation links into the Site Script", ParameterSetName = ParameterSet_SPECIFICCOMPONENTS)]
        public SwitchParameter IncludeLinksToExportedItems;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include the regional settings into the Site Script", ParameterSetName = ParameterSet_SPECIFICCOMPONENTS)]
        public SwitchParameter IncludeRegionalSettings;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include the external sharing configuration into the Site Script", ParameterSetName = ParameterSet_SPECIFICCOMPONENTS)]
        public SwitchParameter IncludeSiteExternalSharingCapability;

        [Parameter(Mandatory = false, HelpMessage = "If specified will include the branding of the site into the Site Script", ParameterSetName = ParameterSet_SPECIFICCOMPONENTS)]
        public SwitchParameter IncludeTheme;

        protected override void ExecuteCmdlet()
        {
            var tenantSiteScriptSerializationInfo = new TenantSiteScriptSerializationInfo
            {
                IncludeBranding = IncludeBranding || IncludeAll,
                IncludedLists = Lists,
                IncludeLinksToExportedItems = IncludeLinksToExportedItems || IncludeAll,
                IncludeRegionalSettings = IncludeRegionalSettings || IncludeAll,
                IncludeSiteExternalSharingCapability = IncludeSiteExternalSharingCapability || IncludeAll,
                IncludeTheme = IncludeTheme || IncludeAll
            };
            var script = Tenant.GetSiteScriptFromSite(Url, tenantSiteScriptSerializationInfo);
            ClientContext.ExecuteQueryRetry();
            WriteObject(script.Value.JSON);
        }
    }
}
#endif