using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.IO;
using System.Management.Automation;
using File = Microsoft.SharePoint.Client.File;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFileVersion", DefaultParameterSetName = "Return as file object")]
    [CmdletHelp("Retrieves all versions of a file.",
        Category = CmdletHelpCategory.Files,
        OutputType = typeof(FileVersion),
        OutputTypeLink = "https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ee543660(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Get-PnPFileVersion -Url Documents/MyDocument.docx",
        Remarks = "Retrieves the file version information for the specified file.",
        SortOrder = 1)]
    public class GetFileVersion : PnPWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Url;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeUrl = string.Empty;

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

#if ONPREMISES
                    file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
            file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif

            ClientContext.Load(file, f => f.Exists, f => f.Versions.IncludeWithDefaultProperties(i => i.CreatedBy));
            ClientContext.ExecuteQueryRetry();

            if (file.Exists)
            {
                var versions = file.Versions;
                ClientContext.ExecuteQueryRetry();
                WriteObject(versions);
            }
        }
    }
}
