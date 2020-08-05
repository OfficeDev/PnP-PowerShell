using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.IO;
using System.Management.Automation;
using File = Microsoft.SharePoint.Client.File;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFile", DefaultParameterSetName = "Return as file object")]
    [CmdletHelp("Downloads a file.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(File),
        OutputTypeLink = "https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ee539248(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor",
        Remarks = "Retrieves the file and downloads it to the current folder",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor -AsFile",
        Remarks = "Retrieves the file and downloads it to c:\\temp\\company.spcolor",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsString",
        Remarks = "Retrieves the file and outputs its contents to the console",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsFile",
        Remarks = "Retrieves the file and returns it as a File object",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsListItem",
        Remarks = "Retrieves the file and returns it as a ListItem object",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url _catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor -AsFile",
        Remarks = "Retrieves the file by site relative URL and downloads it to c:\\temp\\company.spcolor",
        SortOrder = 6)]

    public class GetFile : PnPWebCmdlet
    {
        private const string URLTOPATH = "Save to local path";
        private const string URLASSTRING = "Return as string";
        private const string URLASLISTITEM = "Return as list item";
        private const string URLASFILEOBJECT = "Return as file object";

        [Parameter(Mandatory = true, ParameterSetName = URLASFILEOBJECT, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLASLISTITEM, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLTOPATH, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLASSTRING, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Alias("ServerRelativeUrl", "SiteRelativeUrl")]
        public string Url;

        [Parameter(Mandatory = false, ParameterSetName = URLTOPATH, HelpMessage = "Local path where the file should be saved")]
        public string Path = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = URLTOPATH, HelpMessage = "Name for the local file")]
        public string Filename = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = URLTOPATH)]
        public SwitchParameter AsFile;

        [Parameter(Mandatory = false, ParameterSetName = URLASLISTITEM, HelpMessage = "Returns the file as a listitem showing all its properties")]
        public SwitchParameter AsListItem;

        [Parameter(Mandatory = false, ParameterSetName = URLASLISTITEM, HelpMessage = "If provided in combination with -AsListItem, a System.ArgumentException will be thrown if the file specified in the -Url argument does not exist. Otherwise it will return nothing instead.")]
        public SwitchParameter ThrowExceptionIfFileNotFound;

        [Parameter(Mandatory = false, ParameterSetName = URLASSTRING, HelpMessage = "Retrieve the file contents as a string")]
        public SwitchParameter AsString;

        [Parameter(Mandatory = false, ParameterSetName = URLTOPATH, HelpMessage = "Overwrites the file if it exists.")]
        public SwitchParameter Force;
        
        [Parameter(Mandatory = false, ParameterSetName = URLASFILEOBJECT, HelpMessage = "Retrieve the file contents as a file object.")]
        public SwitchParameter AsFileObject;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeUrl = string.Empty;
            if (string.IsNullOrEmpty(Path))
            {
                Path = SessionState.Path.CurrentFileSystemLocation.Path;
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
            }

            var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!Url.ToLower().StartsWith(webUrl.ToLower()))
            {
                serverRelativeUrl = UrlUtility.Combine(webUrl, Url);
            }
            else
            {
                serverRelativeUrl = Url;
            }

            File file;

            switch (ParameterSetName)
            {
                case URLTOPATH:

                    SaveFileToLocal(SelectedWeb, serverRelativeUrl, Path, Filename, (fileToSave) =>
                    {
                        if (!Force)
                        {
                            WriteWarning($"File '{fileToSave}' exists already. use the -Force parameter to overwrite the file.");
                        }
                        return Force;
                    });
                    break;
                case URLASFILEOBJECT:
#if ONPREMISES
                    file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
                    file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif
                    try
                    {
                        ClientContext.Load(file, f => f.Author, f => f.Length, f => f.ModifiedBy, f => f.Name, f => f.TimeCreated, f => f.TimeLastModified, f => f.Title);
                        ClientContext.ExecuteQueryRetry();
                    }                    
                    catch (ServerException e) when (e.Message == "User cannot be found.")
                    {
                        // Fallback in case the creator or person having last modified the file no longer exists in the environment such that the file can still be downloaded
                        ClientContext.Load(file, f => f.Length, f => f.Name, f => f.TimeCreated, f => f.TimeLastModified, f => f.Title);
                        ClientContext.ExecuteQueryRetry();
                    }                    

                    WriteObject(file);
                    break;
                case URLASLISTITEM:
#if ONPREMISES
                    file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
                    file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif

                    ClientContext.Load(file, f => f.Exists, f => f.ListItemAllFields);

                    ClientContext.ExecuteQueryRetry();
                    if (file.Exists)
                    {
                        WriteObject(file.ListItemAllFields);
                    }
                    else
                    {
                        if (ThrowExceptionIfFileNotFound)
                        {
                            throw new PSArgumentException($"No file found with the provided Url {serverRelativeUrl}", "Url");
                        }
                    }
                    break;
                case URLASSTRING:
                    WriteObject(SelectedWeb.GetFileAsString(serverRelativeUrl));
                    break;
            }
        }

        private void SaveFileToLocal(Web web, string serverRelativeUrl, string localPath, string localFileName = null, Func<string, bool> fileExistsCallBack = null)
        {

#if SP2013 || SP2016
            var file = web.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
            var file = web.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif

            var clientContext = web.Context as ClientContext;
            clientContext.Load(file);
            clientContext.ExecuteQueryRetry();

            ClientResult<Stream> stream = file.OpenBinaryStream();
            clientContext.ExecuteQueryRetry();

            var fileOut = System.IO.Path.Combine(localPath, !string.IsNullOrEmpty(localFileName) ? localFileName : file.Name);

            if (!System.IO.File.Exists(fileOut) || (fileExistsCallBack != null && fileExistsCallBack(fileOut)))
            {
                using (Stream fileStream = new FileStream(fileOut, FileMode.Create))
                {
                    CopyStream(stream.Value, fileStream);
                }
            }
        }

        private void CopyStream(Stream source, Stream destination)
        {
            byte[] buffer = new byte[32768];
            int bytesRead;

            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
        }
    }
}
