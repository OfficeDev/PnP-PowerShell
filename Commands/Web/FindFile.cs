using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Find, "SPOFile")]
    [CmdletHelp("Finds a file in the virtual file system of the web.",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Find-SPOFile -Match *.master", 
        Remarks = "Will return all masterpages located in the current web.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Find-SPOFile -list $list -Match *.pdf",
        Remarks = "Will return all pdf files located in given list.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Find-SPOFile -folder $folder -Match *.docx",
        Remarks = "Will return all docx files located in given folder.",
        SortOrder = 3)]
    public class FindFile : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Wildcard query", ValueFromPipeline = true)]
        public string Match = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "List object to query to")]
        public List list;

        [Parameter(Mandatory = false, HelpMessage = "Folder object to query to")]
        public Folder folder;

        protected override void ExecuteCmdlet()
        {
            if (list != null)
            {
                WriteObject(SelectedWeb.FindFiles(list, Match));
            }
            else if (folder != null)
            {
                WriteObject(SelectedWeb.FindFiles(folder, Match));
            }
            else
            {
                WriteObject(SelectedWeb.FindFiles(Match));
            }
        }
    }
}
