using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsLifecycle.Disable, "PnPResponsiveUI")]
    [CmdletAlias("Disable-SPOResponsiveUI")]
    [CmdletHelp("Disables the PnP Responsive UI implementation on a classic SharePoint Site", Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Disable-PnPResponsiveUI",
        Remarks = @"If enabled previously, this will remove the PnP Responsive UI from a site.",
        SortOrder = 1)]
    public class DisableResponsiveUI : SPOWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var site = ClientContext.Site;
            site.DisableReponsiveUI();
        }
    }
}
