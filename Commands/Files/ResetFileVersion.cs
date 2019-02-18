using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Reset, "PnPFileVersion")]
    [CmdletHelp("Resets a file to its previous version", Category = CmdletHelpCategory.Files, SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Reset-PnPFileVersion -ServerRelativeUrl ""/sites/test/office365.png""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Reset-PnPFileVersion -ServerRelativeUrl ""/sites/test/office365.png"" -CheckinType MajorCheckin -Comment ""Restored to previous version""",
        SortOrder = 2)]
    public class ResetFileVersion : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The server relative URL of the file.")]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = @"The check in type to use. Defaults to Major.")]
        public CheckinType CheckinType = CheckinType.MajorCheckIn;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the checkin. Defaults to 'Restored to previous version'.")]
        public string CheckInComment = "Restored to previous version";

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.ResetFileToPreviousVersion(ServerRelativeUrl,CheckinType,CheckInComment);
        }
    }
}
