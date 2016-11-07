using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet("Ensure", "SPOFolder")]
    [CmdletHelp("Returns a folder from a given site relative path, and will create it if it does not exist.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(Folder),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.folder.aspx")]
    [CmdletExample(
        Code = @"PS:> Ensure-SPOFolder -SiteRelativePath ""demofolder/subfolder""",
        Remarks = "Creates a folder called subfolder in a folder called demofolder located in the root folder of the site. If the folder hierarchy does not exist, it will be created.",
        SortOrder = 1)]
    public class EnsureFolder : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Site Relative Folder Path")]
        public string SiteRelativePath = string.Empty;

        protected override void ExecuteCmdlet()
        {
            WriteObject(SelectedWeb.EnsureFolderPath(SiteRelativePath));
        }
    }
}
