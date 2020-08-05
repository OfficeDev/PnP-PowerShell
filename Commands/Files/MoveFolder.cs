#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Move, "PnPFolder")]
    [CmdletHelp("Move a folder to another location in the current web. If you want to move a folder to a different site collection, use the Move-PnPFile cmdlet instead, which also supports moving folders and also accross site collections.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(Folder),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee538057(v=office.15)"
    )]
    [CmdletExample(
        Code = @"PS:> Move-PnPFolder -Folder Documents/Reports -TargetFolder 'Archived Reports'",
        Remarks = "This will move the folder Reports in the Documents library to the 'Archived Reports' library",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Move-PnPFolder -Folder 'Shared Documents/Reports/2016/Templates' -TargetFolder 'Shared Documents/Reports'",
        Remarks = "This will move the folder Templates to the new location in 'Shared Documents/Reports'",
        SortOrder = 2)]
    public class MoveFolder : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "The folder to move")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The new parent location to which the folder should be moved to")]
        public string TargetFolder = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

#if ONPREMISES
            Folder sourceFolder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder));
            ClientContext.Load(sourceFolder, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            var targetPath = string.Concat(TargetFolder, "/", sourceFolder.Name);
            sourceFolder.MoveTo(targetPath);
            ClientContext.ExecuteQueryRetry();

            var folder = SelectedWeb.GetFolderByServerRelativeUrl(targetPath);
            ClientContext.Load(folder, f => f.Name, f => f.ItemCount, f => f.TimeLastModified, f => f.ListItemAllFields);
            ClientContext.ExecuteQueryRetry();
            WriteObject(folder);
#else
            var sourceFolderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);
            Folder sourceFolder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(sourceFolderUrl));
            ClientContext.Load(sourceFolder, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            var targetPath = string.Concat(TargetFolder, "/", sourceFolder.Name);
            sourceFolder.MoveToUsingPath(ResourcePath.FromDecodedUrl(targetPath));
            ClientContext.ExecuteQueryRetry();

            var folder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(targetPath));
            ClientContext.Load(folder, f => f.Name, f => f.ItemCount, f => f.TimeLastModified, f => f.ListItemAllFields);
            ClientContext.ExecuteQueryRetry();
            WriteObject(folder);
#endif
        }
    }
}
#endif