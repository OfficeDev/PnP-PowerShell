#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Rename, "PnPFolder")]
    [CmdletHelp("Renames a folder",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Rename-PnPFolder -Folder Documents/Reports -TargetFolderName 'Archived Reports'",
        Remarks = "This will rename the folder Reports in the Documents library to 'Archived Reports'",
        SortOrder = 1)]
    public class RenameFolder : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "The folder to rename")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The new folder name")]
        public string TargetFolderName = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

#if ONPREMISES
            Folder sourceFolder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder));
            ClientContext.Load(sourceFolder, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            var targetPath = string.Concat(sourceFolder.ServerRelativeUrl.Remove(sourceFolder.ServerRelativeUrl.Length - sourceFolder.Name.Length), TargetFolderName);
            sourceFolder.MoveTo(targetPath);
            ClientContext.ExecuteQueryRetry();
#else
            var sourceFolderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);
            Folder sourceFolder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(sourceFolderUrl));
            ClientContext.Load(sourceFolder, f => f.Name, f => f.ServerRelativePath);
            ClientContext.ExecuteQueryRetry();

            var targetPath = string.Concat(sourceFolder.ServerRelativePath.DecodedUrl.Remove(sourceFolder.ServerRelativePath.DecodedUrl.Length - sourceFolder.Name.Length), TargetFolderName);
            sourceFolder.MoveToUsingPath(ResourcePath.FromDecodedUrl(targetPath));
            ClientContext.ExecuteQueryRetry();
#endif
        }
    }
}
#endif