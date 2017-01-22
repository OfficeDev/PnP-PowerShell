using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Copy, "PnPFile", SupportsShouldProcess = true)]
    [CmdletAlias("Copy-SPOFile")]
    [CmdletHelp("Copies a file to a different location",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the copy.",
        Code = @"PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx",
        SortOrder = 1)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in the document library called Documents located in the current site to the Documents library in the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the copy.",
        Code = @"PS:>Move-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetUrl /sites/otherproject/Documents/company.docx",
        SortOrder = 2)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it will still perform the copy and replace the original company.aspx file.",
        Code = @"PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists",
        SortOrder = 3)]

    public class CopyFile : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SERVER", HelpMessage = "Server relative Url specifying the file to move. Must include the file name.")]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SITE", HelpMessage = "Site relative Url specifying the file to move. Must include the file name.")]
        public string SiteRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Server relative Url where to copy the file to. Must include the file name.")]
        public string TargetUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "If provided, if a file already exists at the TargetUrl, it will be overwritten. If ommitted, the copy operation will be canceled if the file already exists at the TargetUrl location.")]
        public SwitchParameter OverwriteIfAlreadyExists;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "SITE")
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                ServerRelativeUrl = UrlUtility.Combine(webUrl, SiteRelativeUrl);
            }

            var file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

            ClientContext.Load(file, f => f.Name);
            ClientContext.ExecuteQueryRetry();

            if (Force || ShouldContinue(string.Format(Resources.CopyFile0To1, ServerRelativeUrl, TargetUrl), Resources.Confirm))
            {
                file.CopyTo(TargetUrl, OverwriteIfAlreadyExists);

                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
 