#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWebTemplates")]
    [CmdletHelp(@"Returns the available web templates.", 
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType =typeof(Microsoft.Online.SharePoint.TenantAdministration.SPOTenantWebTemplateCollection),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.spotenantwebtemplatecollection.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates -LCID 1033", Remarks = @"Returns all webtemplates for the Locale with ID 1033 (English)", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPWebTemplates -CompatibilityLevel 15", Remarks = @"Returns all webtemplates for the compatibility level 15", SortOrder = 2)]
    [CmdletRelatedLink(Text = "Locale IDs", Url = "http://go.microsoft.com/fwlink/p/?LinkId=242911Id=242911")]
    public class GetWebTemplates : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The language ID. For instance: 1033 for English")]
        public uint Lcid;

        [Parameter(Mandatory = false, HelpMessage = "The version of SharePoint")]
        public int CompatibilityLevel;

        protected override void ProcessRecord()
        {
            WriteObject(Tenant.GetWebTemplates(Lcid, CompatibilityLevel),true);
        }
    }
}
#endif