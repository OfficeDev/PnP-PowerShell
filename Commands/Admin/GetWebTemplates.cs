using PnP.PowerShell.CmdletHelpAttributes;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWebTemplates")]
    [CmdletHelp(@"Returns the available classic web templates", 
        DetailedDescription = "Will list all available classic templates one can use to create a site. Modern templates will not be returned.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.All,
        OutputType = typeof(Microsoft.Online.SharePoint.TenantAdministration.SPOTenantWebTemplateCollection),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-csom/dn174817(v=office.15)")]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates -LCID 1033", Remarks = @"Returns all webtemplates for the Locale with ID 1033 (English)", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates -CompatibilityLevel 15", Remarks = @"Returns all webtemplates for the compatibility level 15", SortOrder = 2)]
    [CmdletRelatedLink(Text = "Locale IDs", Url = "https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint")]
    public class GetWebTemplates : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The language ID. For instance: 1033 for English. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: Get-PnPAvailableLanguage.")]
        public uint Lcid;

        [Parameter(Mandatory = false, HelpMessage = "The compatibily level of SharePoint where 14 is SharePoint 2010, 15 is SharePoint 2013 and 16 is SharePoint 2016 and later including SharePoint Online")]
        public int CompatibilityLevel;

        protected override void ProcessRecord()
        {
            WriteObject(Tenant.GetWebTemplates(Lcid, CompatibilityLevel),true);
        }
    }
}
