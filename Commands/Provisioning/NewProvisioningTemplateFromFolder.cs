using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXML;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXML.Model;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.New, "PnPProvisioningTemplateFromFolder")]
    [CmdletAlias("New-SPOProvisioningTemplateFromFolder")]
    [CmdletHelp("Generates a provisioning template from a given folder, including only files that are present in that folder",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml",
       Remarks = "Creates an empty provisioning template, and includes all files in the current folder.",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp",
       Remarks = "Creates an empty provisioning template, and includes all files in the c:\\temp folder.",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js",
       Remarks = "Creates an empty provisioning template, and includes all files with a JS extension in the c:\\temp folder.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder ""Shared Documents""",
       Remarks = "Creates an empty provisioning template, and includes all files with a JS extension in the c:\\temp folder and marks the files in the template to be added to the 'Shared Documents' folder",
       SortOrder = 4)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder ""Shared Documents"" -ContentType ""Test Content Type""",
       Remarks = "Creates an empty provisioning template, and includes all files with a JS extension in the c:\\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add a property to the item for the content type.",
       SortOrder = 5)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder ""Shared Documents"" -Properties @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
       Remarks = "Creates an empty provisioning template, and includes all files with a JS extension in the c:\\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add the specified properties to the file entries.",
       SortOrder = 6)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp",
       Remarks = "Creates an empty provisioning template as a pnp package file, and includes all files in the current folder",
       SortOrder = 7)]
    [CmdletExample(
       Code = @"PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp -Folder c:\temp",
       Remarks = "Creates an empty provisioning template as a pnp package file, and includes all files in the c:\\temp folder",
       SortOrder = 8)]
    [CmdletRelatedLink(
       Text ="Encoding", 
       Url = "https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx")]

    public class NewProvisioningTemplateFromFolder : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Filename to write to, optionally including full path.")]
        public string Out;

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Folder to process. If not specified the current folder will be used.")]
        public string Folder;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "Target folder to provision to files to. If not specified, the current folder name will be used.")]
        public string TargetFolder;

        [Parameter(Mandatory = false, HelpMessage = "Optional wildcard pattern to match filenames against. If empty all files will be included.")]
        public string Match = "*.*";

        [Parameter(Mandatory = false, HelpMessage = "An optional content type to use.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Additional properties to set for every file entry in the generated template.")]
        public Hashtable Properties;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "The schema of the output to use, defaults to the latest schema")]
        public XMLPnPSchemaVersion Schema = XMLPnPSchemaVersion.LATEST;

        [Parameter(Mandatory = false, HelpMessage = "If specified, the output will only contain the <pnp:Files> element. This allows the output to be included in another template.")]
        public SwitchParameter AsIncludeFile;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "The encoding type of the XML file, Unicode is default")]
        public System.Text.Encoding Encoding = System.Text.Encoding.Unicode;

        protected override void ExecuteCmdlet()
        {
            Microsoft.SharePoint.Client.ContentType ct = null;
            if (string.IsNullOrEmpty(Folder))
            {
                Folder = SessionState.Path.CurrentFileSystemLocation.Path;
            }
            else
            {
                if (!Path.IsPathRooted(Folder))
                {
                    Folder = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Folder);
                    Folder = new DirectoryInfo(Folder).FullName.TrimEnd('\\', '/'); // normalize away relative ./ paths
                }
            }
            if (ContentType != null)
            {
                ct = ContentType.GetContentType(SelectedWeb);
            }
            if (TargetFolder == null)
            {
                TargetFolder = new DirectoryInfo(SessionState.Path.CurrentFileSystemLocation.Path).Name;
            }

            if (!string.IsNullOrEmpty(Out))
            {
                if (!ShouldContinue()) return;

                if (Path.GetExtension(Out).ToLower() == ".pnp")
                {
                    byte[] pack = CreatePnPPackageFile(ct?.StringId);
                    System.IO.File.WriteAllBytes(Out, pack);
                }
                else
                {
                    var xml = CreateXmlAsStringFrom(ct?.StringId);
                    System.IO.File.WriteAllText(Out, xml, Encoding);
                }
            }
            else
            {
                var xml = CreateXmlAsStringFrom(ct?.StringId);
                WriteObject(xml);
            }
        }

        private bool ShouldContinue()
        {
            if (!Path.IsPathRooted(Out))
            {
                Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
            }

            bool shouldContinue = true;
            if (System.IO.File.Exists(Out))
            {
                shouldContinue = (Force ||
                                  ShouldContinue(string.Format(Commands.Properties.Resources.File0ExistsOverwrite, Out),
                                      Commands.Properties.Resources.Confirm));
            }
            return shouldContinue;
        }

        private byte[] CreatePnPPackageFile(string ctId)
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
            var xml = CreateXmlAsStringFrom(ctId);
            PnPFileInfo templateInfo = new PnPFileInfo
            {
                InternalName = templateFileName.AsInternalFilename(),
                OriginalName = templateFileName,
                Folder = "",
                Content = System.Text.Encoding.UTF8.GetBytes(xml)
            };
            info.Files.Add(templateInfo);

            foreach (var currentFile in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var folder = GetFolderName(currentFile, dirInfo);
                PnPFileInfo fileInfo = new PnPFileInfo
                {
                    InternalName = currentFile.Name.AsInternalFilename(),
                    OriginalName = currentFile.Name,
                    Folder = folder,
                    Content = System.IO.File.ReadAllBytes(currentFile.FullName)
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

        private string CreateXmlAsStringFrom(string ctId)
        {
            var xml = GetFiles(Schema, Folder, ctId);
            if (!AsIncludeFile) return xml;
            XElement xElement = XElement.Parse(xml);
            // Get the Files Element
            XNamespace pnp = XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_12;

            var filesElement = xElement.Descendants(pnp + "Files").FirstOrDefault();
            if (filesElement != null)
            {
                xml = filesElement.ToString();
            }
            return xml;
        }

        private string GetFiles(XMLPnPSchemaVersion schema, string folder, string ctid)
        {
            ProvisioningTemplate template = new ProvisioningTemplate();
            template.Id = "FOLDEREXPORT";
            template.Security = null;
            template.Features = null;
            template.ComposedLook = null;

            template.Files.AddRange(EnumerateFiles(folder, ctid, Properties));

            var formatter = GetTemplateFormatterFromSchema(schema);
            var outputStream = formatter.ToFormattedTemplate(template);
            StreamReader reader = new StreamReader(outputStream);

            return reader.ReadToEnd();
        }

        private static ITemplateFormatter GetTemplateFormatterFromSchema(XMLPnPSchemaVersion schema)
        {
            ITemplateFormatter formatter = null;
            switch (schema)
            {
                case XMLPnPSchemaVersion.LATEST:
                    {
                        formatter = XMLPnPSchemaFormatter.LatestFormatter;
                        break;
                    }
                case XMLPnPSchemaVersion.V201503:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_03);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201505:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_05);
#pragma warning disable CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201508:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_08);
                        break;
                    }
                case XMLPnPSchemaVersion.V201512:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_12);
                        break;
                    }
                case XMLPnPSchemaVersion.V201605:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2016_05);
                        break;
                    }
            }
            return formatter;
        }

        private List<OfficeDevPnP.Core.Framework.Provisioning.Model.File> EnumerateFiles(string folder, string ctid, Hashtable properties)
        {
            var files = new List<OfficeDevPnP.Core.Framework.Provisioning.Model.File>();

            DirectoryInfo dirInfo = new DirectoryInfo(folder);

            foreach (var directory in dirInfo.GetDirectories().Where(d => (d.Attributes & FileAttributes.Hidden) == 0))
            {
                files.AddRange(EnumerateFiles(directory.FullName, ctid, properties));
            }

            var fileInfo = dirInfo.GetFiles(Match);
            foreach (var file in fileInfo.Where(f => (f.Attributes & FileAttributes.Hidden) == 0))
            {
                var unrootedPath = file.FullName.Substring(Folder.Length + 1);
                var targetFolder = Path.Combine(TargetFolder, unrootedPath.LastIndexOf("\\") > -1 ? unrootedPath.Substring(0, unrootedPath.LastIndexOf("\\")) : "");
                targetFolder = targetFolder.Replace('\\', '/');
                var modelFile = new OfficeDevPnP.Core.Framework.Provisioning.Model.File()
                {
                    Folder = targetFolder,
                    Overwrite = true,
                    Src = unrootedPath,
                };
                if (ctid != null)
                {
                    modelFile.Properties.Add("ContentTypeId", ctid);
                }
                if (properties != null && properties.Count > 0)
                {
                    foreach (var key in properties.Keys)
                    {
                        modelFile.Properties.Add(key.ToString(), properties[key].ToString());
                    }
                }
                modelFile.Security = null;
                files.Add(modelFile);
            }

            return files;
        }
    }
}