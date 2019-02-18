using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using SPFile = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPFilesToProvisioningTemplate")]
    [CmdletHelp("Adds files to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFilesToProvisioningTemplate -Path template.pnp -SourceFolder $sourceFolder -Folder $targetFolder",
       Remarks = "Adds files to a PnP Provisioning Template from a local folder",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.xml -SourceFolder $sourceFolder -Folder $targetFolder",
       Remarks = "Adds files reference to a PnP Provisioning XML Template",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder ""./myfolder"" -Folder ""folderinsite"" -FileLevel Published -FileOverwrite:$false",
       Remarks = "Adds files to a PnP Provisioning Template, specifies the level as Published and defines to not overwrite the files if it exists in the site.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder ""./myfolder"" -Recurse",
       Remarks = "Adds files to a PnP Provisioning Template from a local folder recursively.",
       SortOrder = 4)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder $sourceFolder -Folder $targetFolder -Container $container",
       Remarks = "Adds files to a PnP Provisioning Template with a custom container for the files",
       SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolderUrl $urlOfFolder",
        Remarks = "Adds files to a PnP Provisioning Template retrieved from the currently connected web. The url can be either full, server relative or Web relative url.",
        SortOrder = 6)]
    public class AddFilesToProvisioningTemplate : BaseFileProvisioningCmdlet
    {
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_LOCAL_SOURCE, HelpMessage = "The source folder to add to the in-memory template, optionally including full path.")]
        public string SourceFolder;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_REMOTE_SOURCE, HelpMessage = "The source folder to add to the in-memory template, specifying its url in the current connected Web.")]
        public string SourceFolderUrl;

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = PSNAME_LOCAL_SOURCE, HelpMessage = "The target Folder for the source folder to add to the in-memory template.")]
        public string Folder;

        [Parameter(Mandatory = true, Position = 7, ParameterSetName = PSNAME_LOCAL_SOURCE, HelpMessage = "The target Folder for the source folder to add to the in-memory template.")]
        public SwitchParameter Recurse = false;

        protected override void ProcessRecord()
        {
            var template = LoadTemplate();
            if (this.ParameterSetName == PSNAME_REMOTE_SOURCE)
            {
                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                var sourceUri = new Uri(SourceFolderUrl, UriKind.RelativeOrAbsolute);
                // Get the server relative url of the folder, whatever the input url is (absolute, server relative or web relative form)
                var serverRelativeUrl =
                    sourceUri.IsAbsoluteUri ? sourceUri.AbsolutePath : // The url is absolute, extract the absolute path (http://server/sites/web/folder/file)
                    SourceFolderUrl.StartsWith("/", StringComparison.Ordinal) ? SourceFolderUrl : // The url is server relative. Take it as is (/sites/web/folder/file)
                    SelectedWeb.ServerRelativeUrl.TrimEnd('/') + "/" + SourceFolderUrl; // The url is web relative, prepend by the web url (folder/file)


                var folder = SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);

                var files = EnumRemoteFiles(folder, Recurse).OrderBy(f => f.ServerRelativeUrl);
                foreach (var file in files)
                {
                    AddSPFileToTemplate(template, file);
                }
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(SourceFolder))
                {
                    SourceFolder = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, SourceFolder);
                }

                var files = System.IO.Directory.GetFiles(SourceFolder, "*", Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).OrderBy(f => f);

                foreach (var file in files)
                {
                    var localFileFolder = System.IO.Path.GetDirectoryName(file);
                    // relative folder of the leaf file within the directory structure, under the source folder
                    var relativeFolder = Folder + localFileFolder.Substring(SourceFolder.Length);
                    // Load the file and add it to the .PNP file
                    AddLocalFileToTemplate(template, file, relativeFolder);
                }
            }
        }

        private IEnumerable<SPFile> EnumRemoteFiles(Microsoft.SharePoint.Client.Folder folder, bool recurse)
        {
            var ctx = folder.Context;

            ctx.Load(folder.Files, files => files.Include(f => f.ServerRelativeUrl, f => f.Name));
            ctx.ExecuteQueryRetry();

            foreach (var file in folder.Files)
            {
                yield return file;
            }

            if (recurse)
            {
                ctx.Load(folder.Folders);
                ctx.ExecuteQueryRetry();

                foreach (var subFolder in folder.Folders)
                {
                    foreach (var file in EnumRemoteFiles(subFolder, recurse))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}