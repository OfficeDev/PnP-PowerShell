#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPFooter")]
    [CmdletHelp("Gets the configuration regarding the footer of the current web",
        DetailedDescription = "Allows the current configuration of the footer in the current web to be retrieved. The footer currently only works on Modern Communication sites.",
        Category = CmdletHelpCategory.Branding,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFooter",
        Remarks = "Returns the current footer configuration of the current web",
        SortOrder = 1)]
    public class GettFooter : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperties(w => w.FooterEnabled, w => w.FooterLayout, w => w.FooterEmphasis);

            var footer = new PSObject();
            footer.Properties.Add(new PSVariableProperty(new PSVariable("IsEnabled", SelectedWeb.FooterEnabled)));
            footer.Properties.Add(new PSVariableProperty(new PSVariable("Layout", SelectedWeb.FooterLayout)));
            footer.Properties.Add(new PSVariableProperty(new PSVariable("BackgroundTheme", SelectedWeb.FooterEmphasis)));
            footer.Properties.Add(new PSVariableProperty(new PSVariable("Title", SelectedWeb.GetFooterTitle())));
            footer.Properties.Add(new PSVariableProperty(new PSVariable("LogoUrl", SelectedWeb.GetFooterLogoUrl())));

            WriteObject(footer);
        }
    }
}
#endif
