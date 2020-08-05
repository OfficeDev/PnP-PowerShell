using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Move, "PnPFile", SupportsShouldProcess = true, DefaultParameterSetName = ParameterSet_SITE)]
    [CmdletHelp("Moves a file or folder to a different location",
#if !ONPREMISES
        DetailedDescription = "Allows moving a file or folder to a different location inside the same document library, such as in a subfolder, to a different document library on the same site collection or to a document library on another site collection",
#else
        DetailedDescription = "Allows moving a file to a different location inside the same document library, such as in a subfolder or to a different document library on the same site collection. It is not possible to move files between site collections.",
#endif
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Remarks = @"Moves a file named Document.docx located in the document library named ""Shared Documents"" in the current site to the document library named ""Archive"" in the same site, renaming the file to Document2.docx. If a file named Document2.docx already exists at the destination, it won't perform the move.",
        Code = @"PS:>Move-PnPFile -SiteRelativeUrl ""Shared Documents/Document.docx"" -TargetUrl ""/sites/project/Archive/Document2.docx""",
        SortOrder = 1)]
    [CmdletExample(
        Remarks = @"Moves a file named Document.docx located in the document library named ""Shared Documents"" in the current site to the document library named ""Archive"" in the same site. If a file named Document.docx already exists at the destination, it will overwrite it.",
        Code = @"PS:>Move-PnPFile -ServerRelativeUrl ""/sites/project/Shared Documents/Document.docx -TargetUrl ""/sites/project/Archive/Document.docx"" -OverwriteIfAlreadyExists",
        SortOrder = 2)]
#if !ONPREMISES
    [CmdletExample(
        Remarks = @"Moves a file named Document.docx located in the document library named ""Shared Documents"" in the current site to the document library named ""Shared Documents"" in another site collection ""otherproject"" allowing it to overwrite an existing file Document.docx in the destination, allowing the fields to be different on the destination document library from the source document library and allowing a lower document version limit on the destination compared to the source.",
        Code = @"PS:>Move-PnPFile -ServerRelativeUrl ""/sites/project/Shared Documents/Document.docx"" -TargetServerRelativeLibrary ""/sites/otherproject/Shared Documents"" -OverwriteIfAlreadyExists -AllowSchemaMismatch -AllowSmallerVersionLimitOnDestination",
        SortOrder = 3)]
    [CmdletExample(
        Remarks = @"Moves a folder named Archive located in the document library named ""Shared Documents"" in the current site to the document library named ""Project"" in another site collection ""archive"" not allowing it to overwrite an existing folder named ""Archive"" in the destination, allowing the fields to be different on the destination document library from the source document library and allowing a lower document version limit on the destination compared to the source.",
        Code = @"PS:>Move-PnPFile -ServerRelativeUrl ""/sites/project/Shared Documents/Archive"" -TargetServerRelativeLibrary ""/sites/archive/Project"" -AllowSchemaMismatch -AllowSmallerVersionLimitOnDestination",
        SortOrder = 4)]
#endif

    public class MoveFile : PnPWebCmdlet
    {
        private const string ParameterSet_SERVER = "Server Relative";
        private const string ParameterSet_SITE = "Site Relative";
#if !ONPREMISES
        private const string ParameterSet_OTHERSITE = "Other Site Collection";
#endif

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSet_SERVER, HelpMessage = "Server relative Url specifying the file to move. Must include the file name.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "Server relative Url specifying the file or folder to move. Must include the file name if it regards a file or the folder name if it regards a folder.")]
#endif
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSet_SITE, HelpMessage = "Site relative Url specifying the file or folder to move. Must include the file or folder name.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "Site relative Url specifying the file or folder to move. Must include the file or folder name.")]
#endif
        public string SiteRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_SITE, Position = 1, HelpMessage = "Server relative Url where to move the file or folder to. Must include the file or folder name.")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_SERVER, Position = 1, HelpMessage = "Server relative Url where to move the file or folder to. Must include the file or folder name.")]
        public string TargetUrl = string.Empty;

#if !ONPREMISES
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "Server relative url of a document library where to move the fileor folder to. Must not include the file or folder name.")]
        public string TargetServerRelativeLibrary = string.Empty;
#endif

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SERVER, HelpMessage = "If provided, if a file or folder already exists at the TargetUrl, it will be overwritten. If omitted, the move operation will be canceled if the file or folder already exists at the TargetUrl location.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SITE, HelpMessage = "If provided, if a file or folder already exists at the TargetUrl, it will be overwritten. If omitted, the move operation will be canceled if the file or folder already exists at the TargetUrl location.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "If provided, if a file or folder already exists at the TargetServerRelativeLibrary, it will be overwritten. If omitted, the move operation will be canceled if the file or folder already exists at the TargetServerRelativeLibrary location.")]
#endif
        public SwitchParameter OverwriteIfAlreadyExists;

#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "If provided and the target document library specified using TargetServerRelativeLibrary has different fields than the document library where the document is being moved from, the move will succeed. If not provided, it will fail to protect against data loss of metadata stored in fields that cannot be moved along.")]
        public SwitchParameter AllowSchemaMismatch;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "If provided and the target document library specified using TargetServerRelativeLibrary is configured to keep less historical versions of documents than the document library where the document is being moved from, the move will succeed. If not provided, it will fail to protect against data loss of historical versions that cannot be moved along.")]
        public SwitchParameter AllowSmallerVersionLimitOnDestination;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_OTHERSITE, HelpMessage = "If provided, only the latest version of the document will be moved and its history will be discared. If not provided, all historical versions will be moved along.")]
        public SwitchParameter IgnoreVersionHistory;
#endif
        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
#if !ONPREMISES
            // Ensure that with ParameterSet_OTHERSITE we either receive a ServerRelativeUrl or SiteRelativeUrl
            if (ParameterSetName == ParameterSet_OTHERSITE && !ParameterSpecified(nameof(ServerRelativeUrl)) && !ParameterSpecified(nameof(SiteRelativeUrl)))
            {
                throw new PSArgumentException($"Either provide {nameof(ServerRelativeUrl)} or {nameof(SiteRelativeUrl)}");
            }
#endif

            if (ParameterSpecified(nameof(SiteRelativeUrl)))
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                ServerRelativeUrl = UrlUtility.Combine(webUrl, SiteRelativeUrl);
            }
#if ONPREMISES
            var file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

            ClientContext.Load(file, f => f.Name);
            ClientContext.ExecuteQueryRetry();

            if (Force || ShouldContinue(string.Format(Resources.MoveFile0To1, ServerRelativeUrl, TargetUrl), Resources.Confirm))
            {
                file.MoveTo(TargetUrl, OverwriteIfAlreadyExists ? MoveOperations.Overwrite : MoveOperations.None);

                ClientContext.ExecuteQueryRetry();
            }
#else
            var file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(ServerRelativeUrl));

            ClientContext.Load(file, f => f.Name, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            if (Force || ShouldContinue(string.Format(Resources.MoveFile0To1, ServerRelativeUrl, TargetUrl), Resources.Confirm))
            {
                switch(ParameterSetName)
                {
                    case ParameterSet_SITE:
                    case ParameterSet_SERVER:
                        file.MoveToUsingPath(ResourcePath.FromDecodedUrl(TargetUrl), OverwriteIfAlreadyExists.ToBool() ? MoveOperations.Overwrite : MoveOperations.None);
                        break;

                    case ParameterSet_OTHERSITE:
                        SelectedWeb.EnsureProperties(w => w.Url, w => w.ServerRelativeUrl);

                        // Create full URLs including the SharePoint domain to the source and destination
                        var source = UrlUtility.Combine(SelectedWeb.Url.Remove(SelectedWeb.Url.Length - SelectedWeb.ServerRelativeUrl.Length + 1, SelectedWeb.ServerRelativeUrl.Length - 1), file.ServerRelativeUrl);
                        var destination = UrlUtility.Combine(SelectedWeb.Url.Remove(SelectedWeb.Url.Length - SelectedWeb.ServerRelativeUrl.Length + 1, SelectedWeb.ServerRelativeUrl.Length - 1), TargetServerRelativeLibrary);

                        ClientContext.Site.CreateCopyJobs(new[] { source }, destination, new CopyMigrationOptions { IsMoveMode = true, 
                                                                                                                    AllowSchemaMismatch = AllowSchemaMismatch.ToBool(), 
                                                                                                                    AllowSmallerVersionLimitOnDestination = AllowSmallerVersionLimitOnDestination.ToBool(), 
                                                                                                                    IgnoreVersionHistory = IgnoreVersionHistory.ToBool(), 
                                                                                                                    NameConflictBehavior = OverwriteIfAlreadyExists.ToBool() ? MigrationNameConflictBehavior.Replace : MigrationNameConflictBehavior.Fail });
                        break;

                    default:
                        throw new PSInvalidOperationException(string.Format(Resources.ParameterSetNotImplemented, ParameterSetName));
                }

                ClientContext.ExecuteQueryRetry();
            }
#endif
        }
    }
}
 