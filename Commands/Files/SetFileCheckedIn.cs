using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Set, "PnPFileCheckedIn")]
    [CmdletHelp("Checks in a file", 
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:>Set-PnPFileCheckedIn -Url ""/Documents/Contract.docx""",
        SortOrder = 1,
        Remarks = @"Checks in the file ""Contract.docx"" in the ""Documents"" library")]
    [CmdletExample(
        Code = @"PS:>Set-PnPFileCheckedIn -Url ""/Documents/Contract.docx"" -CheckinType MinorCheckin -Comment ""Smaller changes""",
        SortOrder = 2,
        Remarks = @"Checks in the file ""Contract.docx"" in the ""Documents"" library as a minor version and adds the check in comment ""Smaller changes""")]
    public class SetFileCheckedIn : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position=0, ValueFromPipeline=true, HelpMessage = @"The server relative url of the file to check in")]
        public string Url = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = @"The check in type to use. Defaults to Major")]
        public CheckinType CheckinType = CheckinType.MajorCheckIn;

        [Parameter(Mandatory = false, HelpMessage = @"The check in comment")]
        public string Comment = "";

        [Parameter(Mandatory = false, HelpMessage = "Approve file")]
        public SwitchParameter Approve;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.CheckInFile(Url, CheckinType, Comment);
            if (Approve)
                SelectedWeb.ApproveFile(Url, Comment);
        }
    }
}
