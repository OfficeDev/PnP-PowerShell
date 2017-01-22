using System;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using File = System.IO.File;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Set, "PnPWikiPageContent")]
    [CmdletAlias("Set-SPOWikiPageContent")]
    [CmdletHelp("Sets the contents of a wikipage",
        Category = CmdletHelpCategory.Publishing)]
    public class SetWikiPageContent : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "STRING")]
        public string Content = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "FILE")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "FILE", HelpMessage = "Site Relative Page Url")]
        [Parameter(Mandatory = true, ParameterSetName = "STRING", HelpMessage = "Site Relative Page Url")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "FILE")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                if (File.Exists(Path))
                {
                    var fileStream = new StreamReader(Path);
                    var contentString = fileStream.ReadToEnd();
                    fileStream.Close();
                    SelectedWeb.AddHtmlToWikiPage(ServerRelativePageUrl, contentString);
                }
                else
                {
                    throw new Exception($"File {Path} does not exist");
                }
            }
            else
            {
                SelectedWeb.AddHtmlToWikiPage(ServerRelativePageUrl, Content);
            }
        }
    }
}
