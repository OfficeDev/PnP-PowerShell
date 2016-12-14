using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXML;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXML.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsData.Convert, "PnPFolderToProvisioningTemplate")]
    [CmdletAlias("Convert-SPOFolderToProvisioningTemplate")]
    [CmdletHelp("Creates a pnp package file of an existing template xml, and includes all files in the current folder",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp",
       Remarks = "Creates a pnp package file of an existing template xml, and includes all files in the current folder",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp -Folder c:\temp",
       Remarks = "Creates a pnp package file of an existing template xml, and includes all files in the c:\\temp folder",
       SortOrder = 2)]
    public class ConvertProvisioningTemplateFromFolder : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to write to, optionally including full path.")]
        public string Out;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "Folder to process. If not specified the current folder will be used.")]
        public string Folder;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(Folder))
            {
                Folder = SessionState.Path.CurrentFileSystemLocation.Path;
            }
            else
            {
                if (!Path.IsPathRooted(Folder))
                {
                    Folder = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Folder);
                    Folder = new DirectoryInfo(Folder).FullName; // normalize away relative ./ paths
                }
            }

            if (!ShouldContinue()) return;

            if (Path.GetExtension(Out).ToLower() == ".pnp")
            {
                byte[] pack = CreatePnPPackageFile();
                File.WriteAllBytes(Out, pack);
            }
            else
            {
                throw new NotSupportedException("Output filename has to end with .pnp");
            }
        }

        private bool ShouldContinue()
        {
            if (!Path.IsPathRooted(Out))
            {
                Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
            }

            bool shouldContinue = true;
            if (File.Exists(Out))
            {
                shouldContinue = (Force ||
                                  ShouldContinue(string.Format(Properties.Resources.File0ExistsOverwrite, Out),
                                      Properties.Resources.Confirm));
            }
            return shouldContinue;
        }

        private byte[] CreatePnPPackageFile()
        {
            PnPInfo info = new PnPInfo
            {
                Manifest = new PnPManifest()
                {
                    Type = PackageType.Full
                },
                Properties = new PnPProperties()
                {
                    Generator = OfficeDevPnP.Core.Utilities.PnPCoreUtilities.PnPCoreVersionTag,
                    Author = string.Empty,
                },
                Files = new List<PnPFileInfo>()
            };
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(Folder));
            string templateFileName = Path.GetFileNameWithoutExtension(Out) + ".xml";
            bool templateFileMissing = dirInfo.GetFiles(templateFileName, SearchOption.TopDirectoryOnly).Length == 0;
            if (templateFileMissing) throw new InvalidOperationException("You need an xml template file (" + templateFileName + ") with the same name as the .pnp outfile in order to pack a folder to a .pnp package file.");

            foreach (var currentFile in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var folder = GetFolderName(currentFile, dirInfo);
                PnPFileInfo fileInfo = new PnPFileInfo
                {
                    InternalName = currentFile.Name.AsInternalFilename(),
                    OriginalName = currentFile.Name,
                    Folder = folder,
                    Content = File.ReadAllBytes(currentFile.FullName)
                };
                WriteVerbose("Adding file:" + currentFile.Name + " - " + folder);
                info.Files.Add(fileInfo);
            }
            byte[] pack = info.PackTemplate().ToArray();
            return pack;
        }

        private string GetFolderName(FileInfo currentFile, DirectoryInfo rootFolderInfo)
        {
            var fileFolder = currentFile.DirectoryName ?? string.Empty;
            fileFolder = fileFolder.Replace('\\', '/').Replace(' ', '_');
            var rootFolder = rootFolderInfo.FullName.Replace('\\', '/').Replace(' ', '_').TrimEnd('/');
            return fileFolder.Replace(rootFolder, "");
        }
    }
}
