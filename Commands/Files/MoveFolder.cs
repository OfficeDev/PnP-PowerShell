#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Move, "PnPFolder")]
    [CmdletHelp("Move a folder to another location in the current web",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Move-PnPFolder -Folder Documents/Reports -TargetLocation 'Archived Reports'",
        Remarks = "This will move the folder Reports in the Documents library to the 'Archived Reports' library",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Move-PnPFolder -Folder 'Shared Documents/Reports/2016/Templates' -TargetLocation 'Shared Documents/Reports'",
        Remarks = "This will move the folder Templates to the new location in 'Shared Documents/Reports'",
        SortOrder = 2)]
    public class MoveFolder : SPOWebCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "The folder to move")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The new parent location to which the folder should be moved to")]
        public string TargetFolder = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            Folder sourceFolder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder));
            ClientContext.Load(sourceFolder, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            var targetPath = string.Concat(TargetFolder,"/",sourceFolder.Name);
            sourceFolder.MoveTo(targetPath);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif