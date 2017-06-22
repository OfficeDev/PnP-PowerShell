using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPSiteLogo")]
    [CmdletHelp("Sets the Site Logo of the current web.", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSiteLogo -RootFolderRelativeUrl SitePages/Home.aspx",
        Remarks = "Sets the home page to the home.aspx file which resides in the SitePages library",
        SortOrder = 1)]
    public class SetSiteLogo : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The root folder relative url of the homepage, e.g. 'sitepages/home.aspx'", Position = 0, ValueFromPipeline = true)]
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
