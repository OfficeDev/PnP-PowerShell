using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPHomePage")]
    [CmdletAlias("Set-SPOHomePage")]
    [CmdletHelp("Sets the home page of the current web.", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHomePage -RootFolderRelativeUrl SitePages/Home.aspx",
        Remarks = "Sets the home page to the home.aspx file which resides in the SitePages library",
        SortOrder = 1)]
    public class SetHomePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The root folder relative url of the homepage, e.g. 'sitepages/home.aspx'", Position = 0, ValueFromPipeline = true)]
        [Alias("Path")]
        public string RootFolderRelativeUrl = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.SetHomePage(RootFolderRelativeUrl);
        }
    }

}
