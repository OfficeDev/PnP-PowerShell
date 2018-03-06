using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsDiagnostic.Resolve, "PnPFolder")]
    [Alias("Ensure-PnPFolder")]
    [CmdletHelp("Returns a folder from a given site relative path, and will create it if it does not exist.",
        Category = CmdletHelpCategory.Files,
        DetailedDescription = "Returns a folder from a given site relative path, and will create it if it does not exist. If you do not want the folder to be created, for instance just to test if a folder exists, use Get-PnPFolder",
        OutputType = typeof(Folder),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.folder.aspx")]
    [CmdletExample(
        Code = @"PS:> Resolve-PnPFolder -SiteRelativePath ""demofolder/subfolder""",
        Remarks = "Creates a folder called subfolder in a folder called demofolder located in the root folder of the site. If the folder hierarchy does not exist, it will be created.",
        SortOrder = 1)]
    [CmdletRelatedLink(
        Text = "Get-PnPFolder",
        Url = "https://github.com/OfficeDev/PnP-PowerShell/blob/master/Documentation/GetPnPFolder.md")]
    public class ResolveFolder : PnPWebRetrievalsCmdlet<Folder>
    {
        [Parameter(Mandatory = true, HelpMessage = "Site Relative Folder Path", Position = 0)]
        public string SiteRelativePath = string.Empty;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.InvocationName.ToLower() == "ensure-pnpfolder")
            {
                WriteWarning("Ensure-PnPFolder has been deprecated. Use Resolve-PnPFolder with the same parameters instead.");
            }
            WriteObject(SelectedWeb.EnsureFolderPath(SiteRelativePath, RetrievalExpressions));
        }
    }
}
