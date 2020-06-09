using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using PnPFileLevel = OfficeDevPnP.Core.Framework.Provisioning.Model.FileLevel;
using SPFile = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPFileToProvisioningTemplate")]
    [CmdletHelp("Adds a file to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder",
        Remarks = "Adds a file to a PnP Site Template",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.xml -Source $sourceFilePath -Folder $targetFolder",
        Remarks = "Adds a file reference to a PnP Site XML Template",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source ""./myfile.png"" -Folder ""folderinsite"" -FileLevel Published -FileOverwrite:$false",
        Remarks = "Adds a file to a PnP Site Template, specifies the level as Published and defines to not overwrite the file if it exists in the site.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container",
        Remarks = "Adds a file to a PnP Site Template with a custom container for the file",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceUrl ""Shared%20Documents/ProjectStatus.docs""",
        Remarks = "Adds a file to a PnP Provisioning Template retrieved from the currently connected site. The url can be server relative or web relative. If specifying a server relative url has to start with the current site url.",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceUrl ""SitePages/Home.aspx"" -ExtractWebParts",
        Remarks = "Adds a file to a PnP Provisioning Template retrieved from the currently connected site. If the file is a classic page, also extract its webparts. The url can be server relative or web relative. If specifying a server relative url has to start with the current site url.",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolderUrl ""Shared Documents""",
        Remarks = "Adds the content of a remote folder to a PnP Provisioning Template retrieved from the currently connected site. The url can be server relative or web relative. If specifying a server relative url has to start with the current site url.",
        SortOrder = 7)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder ""c:\\data\\reports"" -Folder ""Shared Documents""",
        Remarks = "Adds the content of a local folder to a PnP Provisioning Template retrieved from the currently connected site.",
        SortOrder = 8)]
    public class AddFileToProvisioningTemplate : PnPWebCmdlet
    {
        private const string parameterSet_LOCALFILE = "Local File";
        private const string parameterSet_REMOTEFILE = "Remote File";
        private const string parameterSet_LOCALFOLDER = "Local Folder";
        private const string parameterSet_REMOTEFOLDER = "Remote Folder";
        private const string webpartNSV2 = "http://schemas.microsoft.com/WebPart/v2";
        private const string webpartNSV3 = "http://schemas.microsoft.com/WebPart/v3";

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML site template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = parameterSet_LOCALFILE, HelpMessage = "The file to add to the in-memory template, optionally including full path.")]
        public string Source;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = parameterSet_REMOTEFILE, HelpMessage = "The folder where to search for files, to be added to the in-memory template, specifying its url in the current connected Web.")]
        public string SourceUrl;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = parameterSet_LOCALFOLDER, HelpMessage = "The file to add to the in-memory template, optionally including full path.")]
        public string SourceFolder;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = parameterSet_REMOTEFOLDER, HelpMessage = "The local folder where to search for files to be added to the in-memory template.")]
        public string SourceFolderUrl;

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = parameterSet_LOCALFILE, HelpMessage = "The target Folder for the file to add to the in-memory template.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = parameterSet_LOCALFOLDER, HelpMessage = "The target Folder for the files to add to the in-memory template.")]
        public string Folder;

        [Parameter(Mandatory = false, Position = 3, HelpMessage = "The target Container for the file to add to the in-memory template, optional argument.")]
        public string Container;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "The level of the files to add. Defaults to Published")]
        public PnPFileLevel FileLevel = PnPFileLevel.Published;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "Set to overwrite in site, Defaults to true")]
        public SwitchParameter FileOverwrite = true;

        [Parameter(Mandatory = false, Position = 6, ParameterSetName = parameterSet_REMOTEFILE, HelpMessage = "Include webparts if the file is a page")]
        [Parameter(Mandatory = false, Position = 6, ParameterSetName = parameterSet_REMOTEFOLDER, HelpMessage = "Include webparts if the files are pages")]
        public SwitchParameter ExtractWebParts = true;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            // Load the template
            var template = ReadProvisioningTemplate
                .LoadProvisioningTemplateFromFile(Path,
                TemplateProviderExtensions, (e) =>
                {
                    WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
                });

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }

            if (ExtractWebParts && (this.ParameterSetName == parameterSet_REMOTEFILE || this.ParameterSetName == parameterSet_REMOTEFOLDER))
            {
                ClientContext.Load(SelectedWeb, web => web.Url, web => web.Id, web => web.ServerRelativeUrl);
                ClientContext.Load(((ClientContext)SelectedWeb.Context).Site, site => site.Id, site => site.ServerRelativeUrl, site => site.Url);
                ClientContext.Load(SelectedWeb.Lists, lists => lists.Include(l => l.Title, l => l.RootFolder.ServerRelativeUrl, l => l.Id));

                ClientContext.ExecuteQuery();
            }
            switch (this.ParameterSetName)
            {
                // Add a file from the connected Web
                case parameterSet_REMOTEFILE:
                    {
                        var serverRelativeUrl = UrlToServerRelativeUrl(SourceUrl);

                        var file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
                        AddSPFileToTemplate(template, file);
                        break;
                    }

                case parameterSet_REMOTEFOLDER:
                    {
                        var serverRelativeUrl = UrlToServerRelativeUrl(SourceFolderUrl);

                        var folder = SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);
                        var files = folder.Files;
                        SelectedWeb.Context.Load(files);
                        SelectedWeb.Context.ExecuteQueryRetry();
                        foreach (var file in files)
                        {
                            AddSPFileToTemplate(template, file);
                        }
                        break;
                    }

                case parameterSet_LOCALFILE:
                    {
                        if (!System.IO.Path.IsPathRooted(Source))
                        {
                            Source = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Source);
                        }

                        Folder = Folder.Replace('\\', '/');
                        // Load the file and add it to the .PNP file
                        AddLocalFile(template, Source, Folder, Container);

                        break;
                    }

                case parameterSet_LOCALFOLDER:
                    {
                        if (!System.IO.Path.IsPathRooted(SourceFolder))
                        {
                            SourceFolder = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, SourceFolder);
                        }

                        var files = System.IO.Directory.GetFiles(SourceFolder);
                        var container = Container ?? System.IO.Path.GetFileName(Folder); // Default to name of the targeted folder
                        Folder = Folder.Replace('\\', '/');
                        foreach (var file in files)
                        {
                            AddLocalFile(template, file, Folder, container);
                        }

                        break;
                    }
            }
        }

        private void AddLocalFile(ProvisioningTemplate template, string source, string folder, string container)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var fs = System.IO.File.OpenRead(source))
            {
                var fileName = source.IndexOf(System.IO.Path.DirectorySeparatorChar) > 0
                    ? source.Substring(source.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1)
                    : source;
                AddFileToTemplate(template, fs, folder, fileName, container ?? string.Empty);
            }
        }

        private string UrlToServerRelativeUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var sourceFolderUri = new Uri(url, UriKind.RelativeOrAbsolute);
            var serverRelativeUrl =
                sourceFolderUri.IsAbsoluteUri ? sourceFolderUri.AbsolutePath :
                url.StartsWith("/", StringComparison.Ordinal) ? url :
                SelectedWeb.ServerRelativeUrl.TrimEnd('/') + "/" + url;
            return serverRelativeUrl;
        }

        private void AddSPFileToTemplate(ProvisioningTemplate template, SPFile file)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (file == null) throw new ArgumentNullException(nameof(file));

            file.EnsureProperties(f => f.Name, f => f.ServerRelativeUrl);
            var serverRelativeUrl = file.ServerRelativeUrl;
            var fileName = file.Name;
            var folderRelativeUrl = serverRelativeUrl.Substring(0, serverRelativeUrl.Length - fileName.Length - 1);
            var folderWebRelativeUrl = HttpUtility.UrlKeyValueDecode(folderRelativeUrl.Substring(SelectedWeb.ServerRelativeUrl.TrimEnd('/').Length + 1));

            try
            {
#if SP2013 || SP2016
                var fi = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
                var fi = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif

                IEnumerable<WebPart> webParts = null;
                if (ExtractWebParts)
                {
                    webParts = ExtractSPFileWebParts(file).ToArray();
                }

                var fileStream = fi.OpenBinaryStream();
                ClientContext.ExecuteQueryRetry();
                using (var ms = fileStream.Value)
                {
                    AddFileToTemplate(template, ms, folderWebRelativeUrl, fileName, folderWebRelativeUrl, webParts);
                }
            }
            catch (WebException exc)
            {
                WriteWarning($"Can't add file from url {serverRelativeUrl} : {exc}");
            }
        }

        private IEnumerable<WebPart> ExtractSPFileWebParts(SPFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            if (string.Compare(System.IO.Path.GetExtension(file.Name), ".aspx", true) == 0)
            {
                foreach (var spwp in SelectedWeb.GetWebParts(file.ServerRelativeUrl))
                {
                    spwp.EnsureProperties(wp => wp.WebPart, wp => wp.ZoneId);
                    var webPartDefinition = XElement.Parse(SelectedWeb.GetWebPartXml(spwp.Id, file.ServerRelativeUrl), LoadOptions.PreserveWhitespace);
                    var tokenizedDefinition = Tokenize(webPartDefinition);
                    yield return new WebPart
                    {
                        Contents = tokenizedDefinition,
                        Order = (uint)spwp.WebPart.ZoneIndex,
                        Title = spwp.WebPart.Title,
                        Zone = spwp.ZoneId
                    };
                }
            }
        }

        private static XmlNamespaceManager g_nsMgr = InitNamespaceManager();

        private static XmlNamespaceManager InitNamespaceManager()
        {
            var result = new XmlNamespaceManager(new NameTable());
            result.AddNamespace("v3", webpartNSV3);
            result.AddNamespace("v2", webpartNSV2);
            return result;
        }

        private string Tokenize(XElement webPartDefinition)
        {
            var propNodes = webPartDefinition.Name.Namespace == webpartNSV2 ?
                webPartDefinition.Elements() :
                webPartDefinition.XPathSelectElements("v3:webPart/v3:data/v3:properties/v3:property", g_nsMgr);

            foreach (var propNode in propNodes)
            {
                if (propNode.FirstNode is XCData cdataValue)
                {
                    propNode.ReplaceNodes(new XCData(Tokenize(cdataValue.Value)));
                }
                else if (propNode.Value.Length > 0)
                {
                    propNode.Value = Tokenize(propNode.Value);
                }
            }

            return webPartDefinition.ToString();
        }

        private string Tokenize(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            foreach (var list in SelectedWeb.Lists)
            {
                var webRelativeUrl = list.GetWebRelativeUrl();
                if (!webRelativeUrl.StartsWith("_catalogs", StringComparison.Ordinal))
                {
                    input = input
                        .ReplaceCaseInsensitive(list.Id.ToString("D"), "{listid:" + list.Title + "}");
                    //.ReplaceCaseInsensitive(webRelativeUrl, "{listurl:" + list.Title + "}");
                }
            }
            return input.ReplaceCaseInsensitive(SelectedWeb.Url, "{site}")
                .ReplaceCaseInsensitive(SelectedWeb.ServerRelativeUrl, "{site}")
                .ReplaceCaseInsensitive(SelectedWeb.Id.ToString(), "{siteid}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.ServerRelativeUrl, "{sitecollection}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.Id.ToString(), "{sitecollectionid}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.Url, "{sitecollection}");
        }

        private void AddFileToTemplate(
            ProvisioningTemplate template,
            Stream fs,
            string folder,
            string fileName,
            string container,
            IEnumerable<WebPart> webParts = null
                                      )
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (fs == null) throw new ArgumentNullException(nameof(fs));

            var source = !string.IsNullOrEmpty(container) ? (container + "/" + fileName) : fileName;

            template.Connector.SaveFileStream(fileName, container, fs);

            if (template.Connector is ICommitableFileConnector)
            {
                ((ICommitableFileConnector)template.Connector).Commit();
            }

            var existing = template.Files.FirstOrDefault(f =>
                f.Src == $"{container}/{fileName}"
                && f.Folder == folder);

            if (existing != null)
                template.Files.Remove(existing);

            var newFile = new OfficeDevPnP.Core.Framework.Provisioning.Model.File
            {
                Src = source,
                Folder = folder,
                Level = FileLevel,
                Overwrite = FileOverwrite,
            };

            if (webParts != null) newFile.WebParts.AddRange(webParts);

            template.Files.Add(newFile);

            // Determine the output file name and path
            var outFileName = System.IO.Path.GetFileName(Path);
            var outPath = new FileInfo(Path).DirectoryName;

            var fileSystemConnector = new FileSystemConnector(outPath, "");
            var formatter = XMLPnPSchemaFormatter.LatestFormatter;
            var extension = new FileInfo(Path).Extension.ToLowerInvariant();
            if (extension == ".pnp")
            {
                var provider = new XMLOpenXMLTemplateProvider(template.Connector as OpenXMLConnector);
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(Path, "");
                provider.SaveAs(template, Path, formatter, TemplateProviderExtensions);
            }
        }
    }
}