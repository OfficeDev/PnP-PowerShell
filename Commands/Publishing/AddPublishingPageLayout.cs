using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPPublishingPageLayout")]
    [CmdletAlias("Add-SPOPublishingPageLayout")]
    [CmdletHelp("Adds a publishing page layout",
      Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Add-PnPPublishingPageLayout -Title 'Our custom page layout' -SourceFilePath 'customlayout.aspx' -Description 'A custom page layout' -AssociatedContentTypeID 0x01010901",
        Remarks = "Uploads the pagelayout 'customlayout.aspx' to the current site as a 'web part page' pagelayout",
        SortOrder = 1)]
    public class AddPublishingPageLayout : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Path to the file which will be uploaded")]
        public string SourceFilePath = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Title for the page layout")]
        public string Title = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Description for the page layout")]
        public string Description = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Associated content type ID")]
        public string AssociatedContentTypeID;

        [Parameter(Mandatory = false, HelpMessage = "Folder hierarchy where the html page layouts will be deployed")]
        public string DestinationFolderHierarchy;

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(SourceFilePath))
            {
                SourceFilePath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, SourceFilePath);
            }
            SelectedWeb.DeployPageLayout(SourceFilePath, Title, Description, AssociatedContentTypeID, DestinationFolderHierarchy);
        }
    }
}
