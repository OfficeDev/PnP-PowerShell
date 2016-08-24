using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPOFile")]
    [CmdletHelp("Uploads a file to Web", 
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Add-SPOFile -Path c:\temp\company.master -Folder ""_catalogs/masterpage""", 
        Remarks = "This will upload the file company.master to the masterpage catalog",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-SPOFile -Path .\displaytemplate.html -Folder ""_catalogs/masterpage/display templates/test""", 
        Remarks = "This will upload the file displaytemplate.html to the test folder in the display templates folder. If the test folder not exists it will create it.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-SPOFile -Path .\sample.doc -Folder ""Shared Documents"" -Values @{Modified=""1/1/2016""}",
        Remarks = "This will upload the file sample.doc to the Shared Documnets folder. After uploading it will set the Modified date to 1/1/2016.",
        SortOrder = 3)]
    public class AddFile : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The local file path.")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The destination folder in the site")]
        public string Folder = string.Empty;

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

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
            
            var folder = SelectedWeb.EnsureFolder(SelectedWeb.RootFolder, Folder);
            
            var fileUrl = UrlUtility.Combine(folder.ServerRelativeUrl, System.IO.Path.GetFileName(Path));

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

            var file = folder.UploadFile(new FileInfo(Path).Name, Path, true);

       
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
