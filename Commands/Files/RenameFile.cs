using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Rename, "PnPFile", SupportsShouldProcess = true)]
    [CmdletHelp("Renames a file in its current location",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Remarks = "Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.docx. If a file named mycompany.aspx already exists, it won't perform the rename.",
        Code = @"PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx",
        SortOrder = 1)]
    [CmdletExample(
        Remarks = "Renames a file named company.docx located in the document library called Documents located in the current site to mycompany.aspx. If a file named mycompany.aspx already exists, it won't perform the rename.",
        Code = @"PS:>Rename-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetFileName mycompany.docx",
        SortOrder = 2)]
    [CmdletExample(
        Remarks = "Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.aspx. If a file named mycompany.aspx already exists, it will still perform the rename and replace the original mycompany.aspx file.",
        Code = @"PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx -OverwriteIfAlreadyExists",
        SortOrder = 3)]

    public class RenameFile : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SERVER", HelpMessage = "Server relative Url specifying the file to rename. Must include the file name.")]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SITE", HelpMessage = "Site relative Url specifying the file to rename. Must include the file name.")]
        public string SiteRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "File name to rename the file to. Should only be the file name and not include the path to its location. Use Move-PnPFile to move the file to another location.")]
        public string TargetFileName = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "If provided, if a file already exist with the provided TargetFileName, it will be overwritten. If omitted, the rename operation will be canceled if a file already exists with the TargetFileName file name.")]
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
#if ONPREMISES
            var file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

            ClientContext.Load(file, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();
           
            if (Force || ShouldContinue(string.Format(Resources.RenameFile0To1, file.Name, TargetFileName), Resources.Confirm))
            {
                var targetPath = string.Concat(file.ServerRelativeUrl.Remove(file.ServerRelativeUrl.Length - file.Name.Length), TargetFileName);
                file.MoveTo(targetPath, OverwriteIfAlreadyExists ? MoveOperations.Overwrite : MoveOperations.None);

                ClientContext.ExecuteQueryRetry();
            }
#else
            var file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(ServerRelativeUrl));

            ClientContext.Load(file, f => f.Name, f => f.ServerRelativePath);
            ClientContext.ExecuteQueryRetry();
           
            if (Force || ShouldContinue(string.Format(Resources.RenameFile0To1, file.Name, TargetFileName), Resources.Confirm))
            {
                var targetPath = string.Concat(file.ServerRelativePath.DecodedUrl.Remove(file.ServerRelativePath.DecodedUrl.Length - file.Name.Length), TargetFileName);
                file.MoveToUsingPath(ResourcePath.FromDecodedUrl(targetPath), OverwriteIfAlreadyExists ? MoveOperations.Overwrite : MoveOperations.None);

                ClientContext.ExecuteQueryRetry();
            }
#endif
        }
    }
}
