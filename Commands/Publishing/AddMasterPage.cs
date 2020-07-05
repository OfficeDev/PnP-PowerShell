using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPMasterPage")]
    [CmdletHelp("Adds a Masterpage",
        Category = CmdletHelpCategory.Publishing,
        OutputType = typeof(File),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
    [CmdletExample(
        Code = @"PS:> Add-PnPMasterPage -SourceFilePath ""page.master"" -Title ""MasterPage"" -Description ""MasterPage for Web"" -DestinationFolderHierarchy ""SubFolder""",
        Remarks = @"Adds a MasterPage from the local file ""page.master"" to the folder ""SubFolder"" in the Masterpage gallery.",
        SortOrder = 1)]
    public class AddMasterPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Path to the file which will be uploaded")]
        public string SourceFilePath = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Title for the Masterpage")]
        public string Title = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Description for the Masterpage")]
        public string Description = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Folder hierarchy where the MasterPage will be deployed")]
        public string DestinationFolderHierarchy;

        [Parameter(Mandatory = false, HelpMessage = "UIVersion of the Masterpage. Default = 15")]
        public string UIVersion = "15";

        [Parameter(Mandatory = false, HelpMessage = "Default CSS file for the MasterPage, this Url is SiteRelative")]
        public string DefaultCssFile;

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(SourceFilePath))
            {
                SourceFilePath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, SourceFilePath);
            }

            var masterPageFile = SelectedWeb.DeployMasterPage(SourceFilePath, Title, Description, UIVersion, DefaultCssFile, DestinationFolderHierarchy);
            WriteObject(masterPageFile);
        }
    }
}