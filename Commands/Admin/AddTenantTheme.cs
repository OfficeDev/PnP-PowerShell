#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using SharePointPnP.PowerShell.Commands.Model;
using Newtonsoft.Json;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Add, "PnPTenantTheme")]
    [CmdletHelp("Adds or updates a theme to the tenant.",
        DetailedDescription = @"Adds or updates atheme to the tenant.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> $themepalette = @{
  ""themePrimary"" = ""#00ffff"";
  ""themeLighterAlt"" = ""#f3fcfc"";
  ""themeLighter"" = ""#daffff"";
  ""themeLight"" = ""#affefe"";
  ""themeTertiary"" = ""#76ffff"";
  ""themeSecondary"" = ""#39ffff"";
  ""themeDarkAlt"" = ""#00c4c4"";
  ""themeDark"" = ""#009090"";
  ""themeDarker"" = ""#005252"";
  ""neutralLighterAlt"" = ""#f8f8f8"";
  ""neutralLighter"" = ""#f4f4f4"";
  ""neutralLight"" = ""#eaeaea"";
  ""neutralQuaternaryAlt"" = ""#dadada"";
  ""neutralQuaternary"" = ""#d0d0d0"";
  ""neutralTertiaryAlt"" = ""#c8c8c8"";
  ""neutralTertiary"" = ""#a6a6a6"";
  ""neutralSecondaryAlt"" = ""#767676"";
  ""neutralSecondary"" = ""#666666"";
  ""neutralPrimary"" = ""#333"";
  ""neutralPrimaryAlt"" = ""#3c3c3c"";
  ""neutralDark"" = ""#212121"";
  ""black"" = ""#000000"";
  ""white"" = ""#fff"";
  ""primaryBackground"" = ""#fff"";
  ""primaryText"" = ""#333""
 }
PS:>Add-PnPTenantTheme -Identity ""MyCompanyTheme"" -Palette $themepalette -IsInverted $false",
        Remarks = @"This example adds a theme to the current tenant.", SortOrder = 1)]
    public class AddTenantTheme : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = @"The name of the theme to add or update")]
        public ThemePipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = @"The palette to add. See examples for more information.")]
        public ThemePalettePipeBind Palette;

        [Parameter(Mandatory = true, HelpMessage = @"If the theme is inverted or not")]
        public bool IsInverted;

        [Parameter(Mandatory = false, HelpMessage = @"If a theme is already present, specifying this will overwrite the existing theme")]
        public SwitchParameter Overwrite { get; set; }

        protected override void ExecuteCmdlet()
        {
            var theme = new SPOTheme(Identity.Name, Palette.ThemePalette, IsInverted);

            var themes = Tenant.GetAllTenantThemes();
            ClientContext.Load(themes);
            ClientContext.ExecuteQueryRetry();
            if (themes.FirstOrDefault(t => t.Name == Identity.Name) != null)
            {
                if (Overwrite.ToBool())
                {
                    Tenant.UpdateTenantTheme(Identity.Name, JsonConvert.SerializeObject(theme));
                    ClientContext.ExecuteQueryRetry();
                } else
                {
                    WriteError(new ErrorRecord(new Exception($"Theme exists"), "THEMEEXISTS", ErrorCategory.ResourceExists, Identity.Name));
                }
            } else {
                Tenant.AddTenantTheme(Identity.Name, JsonConvert.SerializeObject(theme));
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif