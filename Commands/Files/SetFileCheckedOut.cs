using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Set, "PnPFileCheckedOut")]
    [CmdletHelp("Checks out a file",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:>Set-PnPFileCheckedOut -Url ""/sites/testsite/subsite/Documents/Contract.docx""",
        SortOrder = 1,
        Remarks = @"Checks out the file ""Contract.docx"" in the ""Documents"" library.")]
    public class SetFileCheckedOut : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position=0, ValueFromPipeline=true, HelpMessage = @"The server relative url of the file to check out")]
        public string Url = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.CheckOutFile(Url);
        }
    }
}
