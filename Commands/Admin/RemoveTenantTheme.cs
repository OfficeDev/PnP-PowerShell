#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using SharePointPnP.PowerShell.Commands.Model;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Remove, "PnPTenantTheme")]
    [CmdletHelp("Removes a theme",
        DetailedDescription = @"Removes the specified theme from the tenant configuration",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPTenantTheme -Name ""MyCompanyTheme""",
        Remarks = @"Removes the specified theme.", SortOrder = 1)]
    public class RemoveTenantTheme : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The name of the theme to retrieve")]
        [Alias("Name")]
        public ThemePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            Tenant.DeleteTenantTheme(Identity.Name);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif