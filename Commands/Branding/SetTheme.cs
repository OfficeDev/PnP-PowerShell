using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOTheme")]
    [CmdletHelp("Sets the theme of the current web.", DetailedDescription = " Sets the theme of the current web, if any of the attributes is not set, that value will be set to null",Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"PS:> Set-SPOTheme", Remarks = "Removes the current theme", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Set-SPOTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Set-SPOTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/teamsite/style library/background.png'", SortOrder = 3)]
    public class SetTheme : SPOWebCmdlet
    {
        private const string PROPBAGKEY = "_PnP_ProvisioningTemplateComposedLookInfo";

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Color Palette Url based on the site relative url")]
        public string ColorPaletteUrl = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Font Scheme Url based on the server relative url")]
        public string FontSchemeUrl = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the Background Image Url based on the server relative url")]
        public string BackgroundImageUrl = null;

        [Parameter(Mandatory = false, HelpMessage = "true if the generated theme files should be placed in the root web, false to store them in this web. Default is false")]
        public SwitchParameter ShareGenerated = false;

        protected override void ExecuteCmdlet()
        {

            SelectedWeb.ApplyTheme(ColorPaletteUrl, FontSchemeUrl, BackgroundImageUrl, ShareGenerated);

            ClientContext.ExecuteQueryRetry();

            var composedLook = new ComposedLook();
            // Set the corresponding property bag value which is used by the provisioning engine
            if (SelectedWeb.PropertyBagContainsKey(PROPBAGKEY))
            {
                composedLook = JsonConvert.DeserializeObject<ComposedLook>(SelectedWeb.GetPropertyBagValueString(PROPBAGKEY, ""));

            }
            else
            {
                composedLook = new ComposedLook();
                composedLook.BackgroundFile = "";
                SelectedWeb.EnsureProperty(w => w.AlternateCssUrl);
                composedLook.AlternateCSS = SelectedWeb.AlternateCssUrl;
                composedLook.ColorFile = "";
                SelectedWeb.EnsureProperty(w => w.MasterUrl);
                composedLook.MasterPage = SelectedWeb.MasterUrl;
                composedLook.FontFile = "";
                SelectedWeb.EnsureProperty(w => w.SiteLogoUrl);
                composedLook.SiteLogo = SelectedWeb.SiteLogoUrl;
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
