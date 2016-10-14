using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "SPOFile")]
    [CmdletHelp("Downloads a file.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(File),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor",
        Remarks = "Retrieves the file and downloads it to the current folder",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor",
        Remarks = "Retrieves the file and downloads it to c:\\temp\\company.spcolor",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsString",
        Remarks = "Retrieves the file and outputs its contents to the console",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsFile",
        Remarks = "Retrieves the file and returns it as a File object",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsListItem",
        Remarks = "Retrieves the file and returns it as a ListItem object",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-SPOFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor",
        Remarks = "Retrieves the file by site relative URL and downloads it to c:\\temp\\company.spcolor",
        SortOrder = 6)]

    public class GetFile : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "SERVER", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "ServerAsString", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "ServerAsFile", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "ServerAsListItem", Position = 0, ValueFromPipeline = true)]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "SITE", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "SiteAsString", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "SiteAsFile", Position = 0, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "SiteAsListItem", Position = 0, ValueFromPipeline = true)]
        public string SiteRelativeUrl = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = "SERVER")]
        [Parameter(Mandatory = false, ParameterSetName = "SITE")]
        public string Path = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = "SERVER")]
        [Parameter(Mandatory = false, ParameterSetName = "SITE")]
        public string Filename = string.Empty;

        [Parameter(Mandatory = false, ParameterSetName = "ServerAsFile")]
        [Parameter(Mandatory = false, ParameterSetName = "SiteAsFile")]
        public SwitchParameter AsFile;

        [Parameter(Mandatory = false, ParameterSetName = "ServerAsListItem")]
        [Parameter(Mandatory = false, ParameterSetName = "SiteAsListItem")]
        public SwitchParameter AsListItem;

        [Parameter(Mandatory = false, ParameterSetName = "ServerAsString")]
        [Parameter(Mandatory = false, ParameterSetName = "SiteAsString")]
        public SwitchParameter AsString;

        protected override void ExecuteCmdlet()
        {
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

            if (MyInvocation.BoundParameters.ContainsKey("SiteRelativeUrl"))
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                ServerRelativeUrl = UrlUtility.Combine(webUrl, SiteRelativeUrl);
            }

            File file = null;

            switch (ParameterSetName)
            {
                case "SERVER":
                case "SITE":
                    SelectedWeb.SaveFileToLocal(ServerRelativeUrl, Path, Filename);
                    break;
                case "ServerAsFile":
                case "SiteAsFile":
                    file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

                    ClientContext.Load(file, f => f.Author, f => f.Length,
                        f => f.ModifiedBy, f => f.Name, f => f.TimeCreated,
                        f => f.TimeLastModified, f => f.Title);

                    ClientContext.ExecuteQueryRetry();

                    WriteObject(file);
                    break;

                case "ServerAsListItem":
                case "SiteAsListItem":
                    file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

                    ClientContext.Load(file, f => f.Exists, f => f.ListItemAllFields);

                    ClientContext.ExecuteQueryRetry();
                    if (file.Exists)
                    {
                        WriteObject(file.ListItemAllFields);
                    }
                    break;

                case "ServerAsString":
                case "SiteAsString":
                    WriteObject(SelectedWeb.GetFileAsString(ServerRelativeUrl));
                    break;
            }
        }
    }
}
