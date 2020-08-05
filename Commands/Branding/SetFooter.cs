#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPFooter")]
    [CmdletHelp("Configures the footer of the current web",
        DetailedDescription = "Allows the footer to be enabled or disabled and fine tuned in the current web. For modifying the navigation links shown in the footer, use Add-PnPNavigationNode -Location Footer.",
        Category = CmdletHelpCategory.Branding,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPFooter -Enabled:$true",
        Remarks = "Enables the footer to be shown on the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPFooter -Enabled:$true -Layout Extended -BackgroundTheme Neutral",
        Remarks = "Enables the footer to be shown on the current web with the extended layout using a neutral background",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPFooter -Title ""Contoso Inc."" -LogoUrl ""/sites/communication/Shared Documents/logo.png""",
        Remarks = "Sets the title and logo shown in the footer",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPFooter -LogoUrl """"",
        Remarks = "Removes the current logo shown in the footer",
        SortOrder = 4)]
    public class SetFooter : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Indicates if the footer should be shown on the current web ($true) or if it should be hidden ($false)")]
        public SwitchParameter Enabled;

        [Parameter(Mandatory = false, HelpMessage = "Defines how the footer should look like")]
        public FooterLayoutType Layout;

        [Parameter(Mandatory = false, HelpMessage = "Defines the background emphasis of the content in the footer")]
        public FooterVariantThemeType BackgroundTheme;

        [Parameter(Mandatory = false, HelpMessage = "Defines the title displayed in the footer")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Defines the server relative URL to the logo to be displayed in the footer. Provide an empty string to remove the current logo.")]
        public string LogoUrl;

        protected override void ExecuteCmdlet()
        {
            bool isDirty = false;

            if (ParameterSpecified(nameof(Enabled)))
            {
                SelectedWeb.FooterEnabled = Enabled.ToBool();
                isDirty = true;
            }

            if (ParameterSpecified(nameof(Layout)))
            {
                SelectedWeb.FooterLayout = Layout;
                isDirty = true;
            }

            if (ParameterSpecified(nameof(BackgroundTheme)))
            {
                SelectedWeb.FooterEmphasis = BackgroundTheme;
                isDirty = true;
            }

            if (ParameterSpecified(nameof(Title)))
            {
                SelectedWeb.SetFooterTitle(Title);
                // No isDirty is needed here as the above request will directly perform the update
            }

            if (ParameterSpecified(nameof(LogoUrl)))
            {
                if (LogoUrl == string.Empty)
                {
                    SelectedWeb.RemoveFooterLogoUrl();
                }
                else
                {
                    SelectedWeb.SetFooterLogoUrl(LogoUrl);
                }
                // No isDirty is needed here as the above request will directly perform the update
            }

            if (isDirty)
            {
                SelectedWeb.Update();
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif
