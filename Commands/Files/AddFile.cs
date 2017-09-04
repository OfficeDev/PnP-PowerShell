using System.Collections;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Add, "PnPFile")]
    [CmdletHelp("Uploads a file to Web",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(Microsoft.SharePoint.Client.File),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
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
        Remarks = "This will upload the file sample.doc to the Shared Documnets folder. After uploading it will set the Modified date to 1/1/2016.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -FileName sample.doc -Folder ""Shared Documents"" -Stream $fileStream -Values @{Modified=""1/1/2016""}",
        Remarks = "This will add a file sample.doc with the contents of the stream into the Shared Documents folder. After adding it will set the Modified date to 1/1/2016.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -FileName sample.doc -Folder ""Shared Documents"" -ContentType ""Document"" -Values @{Modified=""1/1/2016""}",
        Remarks = "This will add a file sample.doc to the Shared Documents folder, with a ContentType of 'Documents'. After adding it will set the Modified date to 1/1/2016.",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFile -FileName sample.docx -Folder ""Documents"" -Values @{Modified=""1/1/2016""; Created=""1/1/2017""; Editor=23}",
        Remarks = "This will add a file sample.docx to the Documents folder and will set the Modified date to 1/1/2016, Created date to 1/1/2017 and the Modified By field to the user with ID 23. To find out about the proper user ID to relate to a specific user, use Get-PnPUser.",
        SortOrder = 6)]

    public class AddFile : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, ParameterSetName = "AsFile", HelpMessage = "The local file path.")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The destination folder in the site")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "AsStream", HelpMessage = "Name for file")]
        public string FileName = string.Empty;
        [Parameter(Mandatory = true, ParameterSetName = "AsStream", HelpMessage = "Stream with the file contents")]
        public Stream Stream;


        [Parameter(Mandatory = false, HelpMessage = "If versioning is enabled, this will check out the file first if it exists, upload the file, then check it in again.")]
        public SwitchParameter Checkout;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the checkin.")]
        public string CheckInComment = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Will auto approve the uploaded file.")]
        public SwitchParameter Approve;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the approval.")]
        public string ApproveComment = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Will auto publish the file.")]
        public SwitchParameter Publish;

        [Parameter(Mandatory = false, HelpMessage = "The comment added to the publish action.")]
        public string PublishComment = string.Empty;

        [Parameter(Mandatory = false)]
        public SwitchParameter UseWebDav;

        [Parameter(Mandatory = false, HelpMessage = "Use the internal names of the fields when specifying field names")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = "Use to assign a ContentType to the file.")]
        public ContentTypePipeBind ContentType;

        protected override void ExecuteCmdlet()
        {

            if (ParameterSetName == "AsFile")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                FileName = System.IO.Path.GetFileName(Path);
            }

            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            var folder = SelectedWeb.EnsureFolder(SelectedWeb.RootFolder, Folder);
            var fileUrl = UrlUtility.Combine(folder.ServerRelativeUrl, FileName);

            ContentType targetContentType = null;
            //Check to see if the Content Type exists.. If it doesn't we are going to throw an exception and block this transaction right here.
            if (ContentType != null)
            {

                try
                {
                    var list = SelectedWeb.GetListByUrl(folder.ServerRelativeUrl);


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
                    var existingFile = SelectedWeb.GetFileByServerRelativeUrl(fileUrl);
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
            if (ParameterSetName == "AsFile")
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

                foreach (var key in Values.Keys)
                {
                    item[key as string] = Values[key];
                }

                item.Update();

                ClientContext.ExecuteQueryRetry();
            }
            if (ContentType != null)
            {
                var item = file.ListItemAllFields;
                item["ContentTypeId"] = targetContentType.Id.StringValue;
                item.Update();
                ClientContext.ExecuteQueryRetry();
            }

            if (Checkout)
                SelectedWeb.CheckInFile(fileUrl, CheckinType.MajorCheckIn, CheckInComment);


            if (Publish)
                SelectedWeb.PublishFile(fileUrl, PublishComment);

            if (Approve)
                SelectedWeb.ApproveFile(fileUrl, ApproveComment);

            WriteObject(file);
        }
    }
}
