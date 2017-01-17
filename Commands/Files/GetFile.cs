using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFile", DefaultParameterSetName = "URLASFILEOBJECT")]
    [CmdletAlias("Get-SPOFile")]
    [CmdletHelp("Downloads a file.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(File),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor",
        Remarks = "Retrieves the file and downloads it to the current folder",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor",
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
        Code = @"PS:> Get-PnPFile -Url _catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor",
        Remarks = "Retrieves the file by site relative URL and downloads it to c:\\temp\\company.spcolor",
        SortOrder = 6)]

    public class GetFile : SPOWebCmdlet
    {
        private const string URLTOPATH = "URLTOPATH";
        private const string URLASSTRING = "URLASSTRING";
        private const string URLASLISTITEM = "URLASLISTITEM";
        private const string URLASFILEOBJECT = "URLASFILEOBJECT";

        [Parameter(Mandatory = true, ParameterSetName = URLASFILEOBJECT, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLASLISTITEM, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLTOPATH, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = URLASSTRING, HelpMessage = "The URL (server or site relative) to the file", Position = 0, ValueFromPipeline = true)]
        [Alias(new[] { "ServerRelativeUrl", "SiteRelativeUrl" })]
        public string Url;

        [Parameter(Mandatory = false, ParameterSetName = URLTOPATH, HelpMessage = "Local path where the file should be saved")]
        public string Path = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = URLTOPATH, HelpMessage = "Name for the local file")]
        public string Filename = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = URLTOPATH)]
        public SwitchParameter AsFile;

        [Parameter(Mandatory = false, ParameterSetName = URLASLISTITEM)]
        public SwitchParameter AsListItem;

        [Parameter(Mandatory = false, ParameterSetName = URLASSTRING, HelpMessage = "Retrieve the file contents as a string")]
        public SwitchParameter AsString;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeUrl = "";
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

            File file = null;

            switch (ParameterSetName)
            {
                case URLTOPATH:
                    SelectedWeb.SaveFileToLocal(serverRelativeUrl, Path, Filename);
                    break;
                case URLASFILEOBJECT:
                    file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);

                    ClientContext.Load(file, f => f.Author, f => f.Length,
                        f => f.ModifiedBy, f => f.Name, f => f.TimeCreated,
                        f => f.TimeLastModified, f => f.Title);

                    ClientContext.ExecuteQueryRetry();

                    WriteObject(file);
                    break;
                case URLASLISTITEM:
                    file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);

                    ClientContext.Load(file, f => f.Exists, f => f.ListItemAllFields);

                    ClientContext.ExecuteQueryRetry();
                    if (file.Exists)
                    {
                        WriteObject(file.ListItemAllFields);
                    }
                    break;
                case URLASSTRING:
                    WriteObject(SelectedWeb.GetFileAsString(serverRelativeUrl));
                    break;
            }
        }
    }
}
