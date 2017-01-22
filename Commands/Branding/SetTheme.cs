using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPTheme")]
    [CmdletAlias("Set-SPOTheme")]
    [CmdletHelp("Sets the theme of the current web.", DetailedDescription = " Sets the theme of the current web, if any of the attributes is not set, that value will be set to null", Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"PS:> Set-PnPTheme", Remarks = "Removes the current theme and resets it to the default.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/teamsite/style library/background.png'", SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/teamsite/style library/background.png' -ResetSubwebsToInherit", SortOrder = 4, Remarks = @"Sets the theme to the web, and updates all subwebs to inherit the theme from this web.")]
    public class SetTheme : PnPWebCmdlet
    {
        private const string PROPBAGKEY = "_PnP_ProvisioningTemplateComposedLookInfo";

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Color Palette Url based on the site relative url")]
        public string ColorPaletteUrl;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Font Scheme Url based on the server relative url")]
        public string FontSchemeUrl = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Background Image Url based on the server relative url")]
        public string BackgroundImageUrl = null;

        [Parameter(Mandatory = false, HelpMessage = "true if the generated theme files should be placed in the root web, false to store them in this web. Default is false")]
        [Obsolete("This parameter is obsolete and its usage has no effect. Generated theme files will be placed in the root web by default.")]
        public SwitchParameter ShareGenerated = false;

        [Parameter(Mandatory = false, HelpMessage = "Resets subwebs to inherit the theme from the rootweb")]
        public SwitchParameter ResetSubwebsToInherit = false;

        [Parameter(Mandatory = false, HelpMessage = "Updates only the rootweb, even if subwebs are set to inherit the theme.")]
        public SwitchParameter UpdateRootWebOnly = false;


        protected override void ExecuteCmdlet()
        {
            var serverRelativeUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
            if (ColorPaletteUrl == null)
            {
                ColorPaletteUrl = "/_catalogs/theme/15/palette001.spcolor";
            }
            if (!ColorPaletteUrl.ToLower().StartsWith(serverRelativeUrl.ToLower()))
            {
                ColorPaletteUrl = ColorPaletteUrl = UrlUtility.Combine(serverRelativeUrl, "/_catalogs/theme/15/palette001.spcolor");
            }
            SelectedWeb.SetThemeByUrl(ColorPaletteUrl, FontSchemeUrl, BackgroundImageUrl, ResetSubwebsToInherit, UpdateRootWebOnly);

            ClientContext.ExecuteQueryRetry();

            if (!SelectedWeb.IsNoScriptSite())
            {
                ComposedLook composedLook;
                // Set the corresponding property bag value which is used by the provisioning engine
                if (SelectedWeb.PropertyBagContainsKey(PROPBAGKEY))
                {
                    composedLook =
                        JsonConvert.DeserializeObject<ComposedLook>(SelectedWeb.GetPropertyBagValueString(PROPBAGKEY, ""));

                }
                else
                {
                    composedLook = new ComposedLook {BackgroundFile = ""};
                    SelectedWeb.EnsureProperty(w => w.AlternateCssUrl);
                    composedLook.ColorFile = "";
                    SelectedWeb.EnsureProperty(w => w.MasterUrl);
                    composedLook.FontFile = "";
                    SelectedWeb.EnsureProperty(w => w.SiteLogoUrl);
                }

                composedLook.Name = composedLook.Name ?? "Custom by PnP PowerShell";
                composedLook.ColorFile = ColorPaletteUrl ?? composedLook.ColorFile;
                composedLook.FontFile = FontSchemeUrl ?? composedLook.FontFile;
                composedLook.BackgroundFile = BackgroundImageUrl ?? composedLook.BackgroundFile;
                var composedLookJson = JsonConvert.SerializeObject(composedLook);

                SelectedWeb.SetPropertyBagValue(PROPBAGKEY, composedLookJson);
            }
        }
    }
}
