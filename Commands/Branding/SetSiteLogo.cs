using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPSiteLogo")]
    [CmdletHelp("Sets the Site Logo of the current web.", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSiteLogo -RootFolderRelativeUrl /sites/contosos/SiteAssets/images/logo.jpg",
        Remarks = "Sets the home page to the home.aspx file which resides in the SitePages library",
        SortOrder = 1)]
    public class SetSiteLogo : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The root folder relative url of the Site Logo, e.g. '/sites/contosos/SiteAssets/images/logo.jpg'", Position = 0, ValueFromPipeline = true)]
        [Alias("Path")]
        public string RootFolderRelativeUrl = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.SiteLogoUrl = SelectedWeb.ServerRelativeUrl + RootFolderRelativeUrl;
            SelectedWeb.Update();
            SelectedWeb.Context.ExecuteQuery();
        }
    }

}
