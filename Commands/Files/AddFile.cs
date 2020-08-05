using System.Collections;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Add, "PnPFile")]
    [CmdletHelp("Uploads a file to Web",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(Microsoft.SharePoint.Client.File),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee539248(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path c:\temp\company.master -Folder ""_catalogs/masterpage""",
        Remarks = "This will upload the file company.master to the masterpage catalog",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path .\displaytemplate.html -Folder ""_catalogs/masterpage/display templates/test""",
        Remarks = "This will upload the file displaytemplate.html to the test folder in the display templates folder. If the test folder does not exist it will create it.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path .\sample.doc -Folder ""Shared Documents"" -Values @{Modified=""1/1/2016""}",
        Remarks = "This will upload the file sample.doc to the Shared Documents folder. After uploading it will set the Modified date to 1/1/2016.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -FileName sample.doc -Folder ""Shared Documents"" -Stream $fileStream -Values @{Modified=""1/1/2016""}",
        Remarks = "This will add a file sample.doc with the contents of the stream into the Shared Documents folder. After adding it will set the Modified date to 1/1/2016.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path sample.doc -Folder ""Shared Documents"" -ContentType ""Document"" -Values @{Modified=""1/1/2016""}",
        Remarks = "This will add a file sample.doc to the Shared Documents folder, with a ContentType of 'Documents'. After adding it will set the Modified date to 1/1/2016.",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path sample.docx -Folder ""Documents"" -Values @{Modified=""1/1/2016""; Created=""1/1/2017""; Editor=23}",
        Remarks = "This will add a file sample.docx to the Documents folder and will set the Modified date to 1/1/2016, Created date to 1/1/2017 and the Modified By field to the user with ID 23. To find out about the proper user ID to relate to a specific user, use Get-PnPUser.",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -Path sample.docx -Folder ""Documents"" -NewFileName ""differentname.docx""",
        Remarks = "This will upload a local file sample.docx to the Documents folder giving it the filename differentname.docx on SharePoint",
        SortOrder = 7)]

    public class AddFile : PnPWebCmdlet
    {
        private const string ParameterSet_ASFILE = "Upload file";
        private const string ParameterSet_ASSTREAM = "Upload file from stream";

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ASFILE, HelpMessage = "The local file path")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The destination folder in the site")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ASSTREAM, HelpMessage = "Name for file")]
        public string FileName = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ASFILE, HelpMessage = "Filename to give the file on SharePoint")]
        public string NewFileName = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ASSTREAM, HelpMessage = "Stream with the file contents")]
        public Stream Stream;

        [Parameter(Mandatory = false, HelpMessage = "If versioning is enabled, this will check out the file first if it exists, upload the file, then check it in again")]
        public SwitchParameter Checkout;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the checkin")]
        public string CheckInComment = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Will auto approve the uploaded file")]
        public SwitchParameter Approve;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the approval")]
        public string ApproveComment = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Will auto publish the file")]
        public SwitchParameter Publish;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the publish action")]
        public string PublishComment = string.Empty;

        [Parameter(Mandatory = false)]
        public SwitchParameter UseWebDav;

        [Parameter(Mandatory = false, HelpMessage = "Use the internal names of the fields when specifying field names." +
                                                     "\n\nSingle line of text: -Values @{\"Title\" = \"Title New\"}" +
                                                     "\n\nMultiple lines of text: -Values @{\"MultiText\" = \"New text\\n\\nMore text\"}" +
                                                     "\n\nRich text: -Values @{\"MultiText\" = \"<strong>New</strong> text\"}" +
             "\n\nChoice: -Values @{\"Choice\" = \"Value 1\"}" +
             "\n\nNumber: -Values @{\"Number\" = \"10\"}" +
             "\n\nCurrency: -Values @{\"Number\" = \"10\"}" +
             "\n\nCurrency: -Values @{\"Currency\" = \"10\"}" +
             "\n\nDate and Time: -Values @{\"DateAndTime\" = \"03/10/2015 14:16\"}" +
             "\n\nLookup (id of lookup value): -Values @{\"Lookup\" = \"2\"}" +
             "\n\nMulti value lookup (id of lookup values as array 1): -Values @{\"MultiLookupField\" = \"1\",\"2\"}" +
             "\n\nMulti value lookup (id of lookup values as array 2): -Values @{\"MultiLookupField\" = 1,2}" +
             "\n\nMulti value lookup (id of lookup values as string): -Values @{\"MultiLookupField\" = \"1,2\"}" +
             "\n\nYes/No: -Values @{\"YesNo\" = $false}" +
             "\n\nPerson/Group (id of user/group in Site User Info List or email of the user, separate multiple values with a comma): -Values @{\"Person\" = \"user1@domain.com\",\"21\"}" +
             "\n\nManaged Metadata (single value with path to term): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\"}" +
             "\n\nManaged Metadata (single value with id of term): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\"} with Id of term" +
             "\n\nManaged Metadata (multiple values with paths to terms): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\",\"CORPORATE|DEPARTMENTS|HR\"}" +
             "\n\nManaged Metadata (multiple values with ids of terms): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\",\"52d88107-c2a8-4bf0-adfa-04bc2305b593\"}" +
             "\n\nHyperlink or Picture: -Values @{\"Hyperlink\" = \"https://github.com/OfficeDev/, OfficePnp\"}")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = "Use to assign a ContentType to the file")]
        public ContentTypePipeBind ContentType;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == ParameterSet_ASFILE)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                if (string.IsNullOrEmpty(NewFileName))
                {
                    FileName = System.IO.Path.GetFileName(Path);
                }
                else
                {
                    FileName = NewFileName;
                }
            }

            var folder = EnsureFolder();
            var fileUrl = UrlUtility.Combine(folder.ServerRelativeUrl, FileName);

            ContentType targetContentType = null;
            // Check to see if the Content Type exists. If it doesn't we are going to throw an exception and block this transaction right here.
            if (ContentType != null)
            {
                try
                {
                    var list = SelectedWeb.GetListByUrl(Folder);

                    if (!string.IsNullOrEmpty(ContentType.Id))
                    {
                        targetContentType = list.GetContentTypeById(ContentType.Id);
                    }
                    else if (!string.IsNullOrEmpty(ContentType.Name))
                    {
                        targetContentType = list.GetContentTypeByName(ContentType.Name);
                    }
                    else if (ContentType.ContentType != null)
                    {
                        targetContentType = ContentType.ContentType;
                    }
                    if (targetContentType == null)
                    {
                        ThrowTerminatingError(new ErrorRecord(new ArgumentException($"Content Type Argument: {ContentType} does not exist in the list: {list.Title}"), "CONTENTTYPEDOESNOTEXIST", ErrorCategory.InvalidArgument, this));
                    }
                }
                catch
                {
                    ThrowTerminatingError(new ErrorRecord(new ArgumentException($"The Folder specified ({folder.ServerRelativeUrl}) does not have a corresponding List, the -ContentType parameter is not valid."), "RELATIVEPATHNOTINLIBRARY", ErrorCategory.InvalidArgument, this));
                }
            }

            // Check if the file exists
            if (Checkout)
            {
                try
                {
#if ONPREMISES                    
                    var existingFile = SelectedWeb.GetFileByServerRelativeUrl(fileUrl);
#else               
                    var existingFile = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(fileUrl));
#endif                          
                    existingFile.EnsureProperty(f => f.Exists);
                    if (existingFile.Exists)
                    {
                        SelectedWeb.CheckOutFile(fileUrl);
                    }
                }
                catch
                { // Swallow exception, file does not exist 
                }
            }
            Microsoft.SharePoint.Client.File file;
            if (ParameterSetName == ParameterSet_ASFILE)
            {

                file = folder.UploadFile(FileName, Path, true);
            }
            else
            {
                file = folder.UploadFile(FileName, Stream, true);
            }

            if (Values != null)
            {
                var item = file.ListItemAllFields;

                ListItemHelper.UpdateListItem(item, Values, ListItemUpdateType.UpdateOverwriteVersion,
                    (warning) =>
                    {
                        WriteWarning(warning);
                    },
                    (terminatingErrorMessage, terminatingErrorCode) =>
                    {
                        ThrowTerminatingError(new ErrorRecord(new Exception(terminatingErrorMessage), terminatingErrorCode, ErrorCategory.InvalidData, this));
                    });
            }
            if (ContentType != null)
            {
                var item = file.ListItemAllFields;
                item["ContentTypeId"] = targetContentType.Id.StringValue;
#if !ONPREMISES
                item.UpdateOverwriteVersion();
#else
                item.Update();
#endif
                ClientContext.ExecuteQueryRetry();
            }

            if (Checkout)
                SelectedWeb.CheckInFile(fileUrl, CheckinType.MajorCheckIn, CheckInComment);


            if (Publish)
                SelectedWeb.PublishFile(fileUrl, PublishComment);

            if (Approve)
                SelectedWeb.ApproveFile(fileUrl, ApproveComment);
            ClientContext.Load(file);
            ClientContext.ExecuteQueryRetry();
            WriteObject(file);
        }

        /// <summary>
        /// Ensures the folder to which the file is to be uploaded exists. Changed from using the EnsureFolder implementation in PnP Sites Core as that requires at least member rights to the entire site to work.
        /// </summary>
        /// <returns>The folder to which the file needs to be uploaded</returns>
        private Folder EnsureFolder()
        {
            // First try to get the folder if it exists already. This avoids an Access Denied exception if the current user doesn't have Full Control access at Web level
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
            var Url = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);

            Folder folder = null;
            try
            {
#if ONPREMISES
                folder = SelectedWeb.GetFolderByServerRelativeUrl(Url);
#else
                folder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(Url));
#endif
                folder.EnsureProperties(f => f.ServerRelativeUrl);
                return folder;
            }
            // Exception will be thrown if the folder does not exist yet on SharePoint
            catch (ServerException serverEx) when (serverEx.ServerErrorCode == -2147024894)
            {
                // Try to create the folder
                folder = SelectedWeb.EnsureFolder(SelectedWeb.RootFolder, Folder);
                folder.EnsureProperties(f => f.ServerRelativeUrl);
                return folder;
            }
        }
    }
}
