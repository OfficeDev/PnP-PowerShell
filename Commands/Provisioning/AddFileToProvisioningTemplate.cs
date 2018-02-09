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
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using PnPFileLevel = OfficeDevPnP.Core.Framework.Provisioning.Model.FileLevel;
using SPFile = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet("Add", "PnPFileToProvisioningTemplate")]
    [CmdletHelp("Adds a file to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder",
       Remarks = "Adds a file to a PnP Provisioning Template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.xml -Source $sourceFilePath -Folder $targetFolder",
       Remarks = "Adds a file reference to a PnP Provisioning XML Template",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source ""./myfile.png"" -Folder ""folderinsite"" -FileLevel Published -FileOverwrite:$false",
       Remarks = "Adds a file to a PnP Provisioning Template, specifies the level as Published and defines to not overwrite the file if it exists in the site.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container",
       Remarks = "Adds a file to a PnP Provisioning Template with a custom container for the file",
       SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceUrl $urlOfFile",
        Remarks = "Adds a file to a PnP Provisioning Template retrieved from the currently connected web",
        SortOrder = 4)]
    public class AddFileToProvisioningTemplate : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML provisioning template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "LocalSourceFile", HelpMessage = "The file to add to the in-memory template, optionally including full path.")]
        public string Source;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "RemoteSourceFile", HelpMessage = "The file to add to the in-memory template, specifying its url in the current connected Web.")]
        public string SourceUrl;

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = "LocalSourceFile", HelpMessage = "The target Folder for the file to add to the in-memory template.")]
        public string Folder;

        [Parameter(Mandatory = false, Position = 3, HelpMessage = "The target Container for the file to add to the in-memory template, optional argument.")]
        public string Container;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "The level of the files to add. Defaults to Published")]
        public PnPFileLevel FileLevel = PnPFileLevel.Published;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "Set to overwrite in site, Defaults to true")]
        public SwitchParameter FileOverwrite = true;

        [Parameter(Mandatory = false, Position = 6, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        [Parameter(Mandatory = false, Position = 7, ParameterSetName = "RemoteSourceFile", HelpMessage = "Specifies if webparts has to be exported when exporting a remote file")]
        public SwitchParameter IncludeWebParts;

        protected override void ProcessRecord()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            // Load the template
            var template = LoadProvisioningTemplate.LoadProvisioningTemplateFromFile(
                Path,
                TemplateProviderExtensions
                );

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }
            if (this.ParameterSetName == "RemoteSourceFile")
            {
                SelectedWeb.EnsureProperties(w => w.Id, w => w.Url, w => w.ServerRelativeUrl);
                ((ClientContext)SelectedWeb.Context).Site.EnsureProperties(s => s.Id, s => s.Url, s => s.ServerRelativeUrl);

                var sourceUri = new Uri(SourceUrl, UriKind.RelativeOrAbsolute);
                var serverRelativeUrl =
                    sourceUri.IsAbsoluteUri ? sourceUri.AbsolutePath :
                    SourceUrl.StartsWith("/") ? SourceUrl :
                    SelectedWeb.ServerRelativeUrl.TrimEnd('/') + "/" + SourceUrl;

                var file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
                ClientContext.Load(file, f => f.ServerRelativeUrl);
                var fileName = file.EnsureProperty(f => f.Name);
                var folderRelativeUrl = serverRelativeUrl.Substring(0, serverRelativeUrl.Length - fileName.Length - 1);
                var folderWebRelativeUrl = HttpUtility.UrlKeyValueDecode(folderRelativeUrl.Substring(SelectedWeb.ServerRelativeUrl.TrimEnd('/').Length + 1));
                if (ClientContext.HasPendingRequest) ClientContext.ExecuteQuery();
                try
                {
                    using (var fi = SPFile.OpenBinaryDirect(ClientContext, serverRelativeUrl))
                    using (var ms = new MemoryStream())
                    {
                        // We are using a temporary memory stream because the file connector is seeking in the stream
                        // and the stream provided by OpenBinaryDirect does not allow it
                        fi.Stream.CopyTo(ms);
                        ms.Position = 0;
                        var fileProperties = GetProperties(file, ms.ToArray());

                        var webParts = IncludeWebParts ? ExtractWebParts(file) : null;
                        AddFileToTemplate(
                            template,
                            ms,
                            folderWebRelativeUrl,
                            fileName,
                            folderWebRelativeUrl,
                            fileProperties,
                            webParts
                            );
                    }
                }
                catch (WebException exc)
                {
                    WriteWarning($"Can't add file from url {serverRelativeUrl} : {exc}");
                }
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(Source))
                {
                    Source = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Source);
                }

                // Load the file and add it to the .PNP file
                using (var fs = System.IO.File.OpenRead(Source))
                {
                    Folder = Folder.Replace("\\", "/");

                    var fileName = Source.IndexOf("\\") > 0 ? Source.Substring(Source.LastIndexOf("\\") + 1) : Source;
                    var container = !string.IsNullOrEmpty(Container) ? Container : string.Empty;
                    AddFileToTemplate(template, fs, Folder, fileName, container);
                }
            }
        }

        private IEnumerable<WebPart> ExtractWebParts(SPFile file)
        {
            if (string.Compare(System.IO.Path.GetExtension(file.Name), ".aspx", true) == 0)
            {
                foreach (var spwp in SelectedWeb.GetWebParts(file.ServerRelativeUrl))
                {
                    spwp.EnsureProperties(wp => wp.WebPart
#if !SP2016 // Missing ZoneId property in SP2016 version of the CSOM Library
                , wp => wp.ZoneId
#endif
                );
                    yield return new WebPart
                    {
                        Contents = Tokenize(SelectedWeb.GetWebPartXml(spwp.Id, file.ServerRelativeUrl)),
                        Order = (uint)spwp.WebPart.ZoneIndex,
                        Title = spwp.WebPart.Title,
#if !SP2016 // Missing ZoneId property in SP2016 version of the CSOM Library
                        Zone = spwp.ZoneId
#endif
                    };
                }
            }
            else
            {
                WriteWarning($"File {file.ServerRelativeUrl} is not a webpart page");
            }
        }

        private string Tokenize(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            //foreach (var list in lists)
            //{
            //    input = input.ReplaceCaseInsensitive(web.Url.TrimEnd('/') + "/" + list.GetWebRelativeUrl(), "{listurl:" + Regex.Escape(list.Title) + "}");
            //    input = input.ReplaceCaseInsensitive(list.RootFolder.ServerRelativeUrl, "{listurl:" + Regex.Escape(list.Title)+ "}");
            //}
            return input.ReplaceCaseInsensitive(SelectedWeb.Url, "{site}")
                .ReplaceCaseInsensitive(SelectedWeb.ServerRelativeUrl, "{site}")
                .ReplaceCaseInsensitive(SelectedWeb.Id.ToString(), "{siteid}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.ServerRelativeUrl, "{sitecollection}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.Id.ToString(), "{sitecollectionid}")
                .ReplaceCaseInsensitive(((ClientContext)SelectedWeb.Context).Site.Url, "{sitecollection}");
        }

        private Dictionary<string, string> GetProperties(SPFile file, byte[] rawContent)
        {
            if (string.Compare(System.IO.Path.GetExtension(file.Name), ".aspx", true) == 0)
            {
                var content = Encoding.UTF8.GetString(rawContent);

                var propsMatch = Regex.Match(content, @"<SharePoint:CTFieldRefs.*?>(.*)</SharePoint:CTFieldRefs>", RegexOptions.Singleline);
                if (propsMatch.Success)
                {
                    var xml = $"<wrapper xmlns:mso='mso' xmlns:msdt='msdt'>{propsMatch.Groups[1].Value}</wrapper>";
                    var propsXml = XElement.Parse(xml);
                    var xmlNsMgr = new XmlNamespaceManager(new NameTable());
                    xmlNsMgr.AddNamespace("mso", "mso");
                    xmlNsMgr.AddNamespace("msdt", "msdt");
                    var propNodes = propsXml.XPathSelectElement("//mso:CustomDocumentProperties", xmlNsMgr);
                    return propNodes.Elements().ToDictionary(
                        n => n.Name.LocalName,
                        n => Tokenize(n.Value)
                        );
                }
            }
            return null;
        }

        private void AddFileToTemplate(
            ProvisioningTemplate template,
            Stream fs,
            string folder,
            string fileName,
            string container,
            IDictionary<string, string> properties = null,
            IEnumerable<WebPart> webParts = null
            )
        {
            var source = !string.IsNullOrEmpty(container) ? (container + "/" + fileName) : fileName;

            template.Connector.SaveFileStream(fileName, container, fs);

            if (template.Connector is ICommitableFileConnector)
            {
                ((ICommitableFileConnector)template.Connector).Commit();
            }

            var existing = template.Files.FirstOrDefault(f =>
                f.Src == $"{container}/{fileName}"
                && f.Folder == folder);
            if(existing != null)
            template.Files.Remove(existing);

            var newFile = new OfficeDevPnP.Core.Framework.Provisioning.Model.File
            {
                Src = source,
                Folder = folder,
                Level = FileLevel,
                Overwrite = FileOverwrite
            };
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    newFile.Properties.Add(prop.Key, prop.Value);
                }
            }
            if (webParts != null)
            {
                newFile.WebParts.AddRange(webParts);
            }
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