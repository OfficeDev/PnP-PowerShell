using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Find, "PnPFile", DefaultParameterSetName = "Web")]
    [CmdletHelp("Finds a file in the virtual file system of the web.",
         Category = CmdletHelpCategory.Files,
         OutputType = typeof(File),
         OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
    [CmdletExample(
         Code = @"PS:> Find-PnPFile -Match *.master",
         Remarks = "Will return all masterpages located in the current web.",
         SortOrder = 1)]
      [CmdletExample(
         Code = @"PS:> Find-PnPFile -List ""Documents"" -Match *.pdf",
         Remarks = "Will return all pdf files located in given list.",
         SortOrder = 2)]
     [CmdletExample(
         Code = @"PS:> Find-PnPFile -Folder ""Shared Documents/Sub Folder"" -Match *.docx",
         Remarks = "Will return all docx files located in given folder.",
         SortOrder = 3)]
    public class FindFile : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Wildcard query", ValueFromPipeline = true, ParameterSetName = "Web", Position = 0)]
        [Parameter(Mandatory = true, HelpMessage = "Wildcard query", ValueFromPipeline = true, ParameterSetName = "List", Position = 0)]
        [Parameter(Mandatory = true, HelpMessage = "Wildcard query", ValueFromPipeline = true, ParameterSetName = "Folder", Position = 0)]
        public string Match = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "List title, url or an actual List object to query", ParameterSetName = "List")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "Folder object or relative url of a folder to query", ParameterSetName = "Folder")]
        public FolderPipeBind Folder;

        protected override void ExecuteCmdlet()
        {
            switch (ParameterSetName)
            {
                case "List":
                {
                    var list = List.GetList(SelectedWeb);
                    WriteObject(list.FindFiles(Match));
                    break;
                }
                case "Folder":
                {
                    var folder = Folder.GetFolder(SelectedWeb);
                    WriteObject(folder.FindFiles(Match));
                    break;
                }
                default:
                {
                    WriteObject(SelectedWeb.FindFiles(Match));
                    break;
                }
            }
        }
    }
}
