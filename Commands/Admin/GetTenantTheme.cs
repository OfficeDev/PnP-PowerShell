#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using PnP.PowerShell.Commands.Model;
using System.Text.Json;

namespace PnP.PowerShell.Commands.Admin
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
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantTheme -Name ""MyCompanyTheme"" -AsJson",
        Remarks = @"Returns the specified theme formatted as JSON", SortOrder = 2)]
    public class GetTenantTheme : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The name of the theme to retrieve")]
        public string Name;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "Outputs the themes as JSON")]
        public SwitchParameter AsJson;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Name)))
            {
                var theme = Tenant.GetTenantTheme(Name);
                ClientContext.Load(theme);
                ClientContext.ExecuteQueryRetry();
                if (AsJson)
                {
                    WriteObject(JsonSerializer.Serialize(theme.Palette));
                }
                else
                {
                    WriteObject(new SPOTheme(theme.Name, theme.Palette, theme.IsInverted));
                }
            }
            else
            {
                var themes = Tenant.GetAllTenantThemes();
                ClientContext.Load(themes);
                ClientContext.ExecuteQueryRetry();
                if (AsJson)
                {
                    WriteObject(JsonSerializer.Serialize(themes.Select(t => new SPOTheme(t.Name, t.Palette, t.IsInverted))));
                }
                else
                {
                    WriteObject(themes.Select(t => new SPOTheme(t.Name, t.Palette, t.IsInverted)), true);
                }
            }

        }
    }
}
#endif