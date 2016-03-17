using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOHomePage")]
    [CmdletHelp("Sets the home page of the current web.", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-SPOHomePage -RootFolderRelativeUrl SitePages/Home.aspx",
        Remarks = "Sets the home page to the home.aspx file which resides in the SitePages library",
        SortOrder = 1)]
    public class SetHomePage : SPOWebCmdlet
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
