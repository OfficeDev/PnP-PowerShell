#if !CLIENTSDKV15
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOWebTemplates")]
    [CmdletHelp(@"Office365 only: Returns the available web templates.", Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(Code = @"PS:> Get-SPOWebTemplates", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-SPOWebTemplates -LCID 1033", Remarks = @"Returns all webtemplates for the Locale with ID 1033 (English)", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-SPOWebTemplates -CompatibilityLevel 15", Remarks = @"Returns all webtemplates for the compatibility level 15", SortOrder = 2)]
    [CmdletRelatedLink(Text = "Locale IDs", Url = "http://go.microsoft.com/fwlink/p/?LinkId=242911Id=242911")]
    public class GetWebTemplates : SPOAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The language id like 1033 for English")]
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