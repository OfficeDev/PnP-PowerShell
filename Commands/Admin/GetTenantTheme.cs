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
    [Cmdlet(VerbsCommon.Get, "PnPTenantTheme")]
    [CmdletHelp("Returns all or a specific theme",
        DetailedDescription = @"Returns all or a specific tenant theme.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantTheme",
        Remarks = @"Returns all themes", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantTheme -Name ""MyCompanyTheme""",
        Remarks = @"Returns the specified theme", SortOrder = 1)]
    public class GetTenantTheme : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The name of the theme to retrieve")]
        public string Name;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Name"))
            {
                var theme = Tenant.GetTenantTheme(Name);
                ClientContext.Load(theme);
                ClientContext.ExecuteQueryRetry();
                WriteObject(new SPOTheme(theme.Name, theme.Palette, theme.IsInverted));
            }
            else
            {
                var themes = Tenant.GetAllTenantThemes();
                ClientContext.Load(themes);
                ClientContext.ExecuteQueryRetry();
                WriteObject(themes.Select(t => new SPOTheme(t.Name, t.Palette, t.IsInverted)), true);
            }

        }
    }
}
#endif