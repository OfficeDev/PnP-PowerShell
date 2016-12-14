using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet("Add", "PnPFileToProvisioningTemplate")]
    [CmdletAlias("Add-SPOFileToProvisioningTemplate")]
    [CmdletHelp("Adds a file to an in-memory PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder",
       Remarks = "Adds a file to an in-memory PnP Provisioning Template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container",
       Remarks = "Adds a file to an in-memory PnP Provisioning Template with a custom container for the file",
       SortOrder = 2)]
    public class AddFileToProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML provisioning template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "The file to add to the in-memory template, optionally including full path.")]
        public string Source;

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "The target Folder for the file to add to the in-memory template.")]
        public string Folder;

        [Parameter(Mandatory = false, Position = 3, HelpMessage = "The target Container for the file to add to the in-memory template, optional argument.")]
        public string Container;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
           // Load the template
            var template = LoadProvisioningTemplate
                .LoadProvisioningTemplateFromFile(Path,
                SessionState.Path.CurrentFileSystemLocation.Path,
                TemplateProviderExtensions);

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }

            // Load the file and add it to the .PNP file
            using (var fs = new FileStream(Source, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Folder = Folder.Replace("\\", "/");

                var fileName = Source.IndexOf("\\") > 0 ? Source.Substring(Source.LastIndexOf("\\") + 1) : Source;
                var container = !string.IsNullOrEmpty(Container) ? Container : string.Empty;
                var source = !string.IsNullOrEmpty(container) ? (container + "/" + fileName) : fileName;

                template.Connector.SaveFileStream(fileName, container, fs);

                if (template.Connector is ICommitableFileConnector)
                {
                    ((ICommitableFileConnector)template.Connector).Commit();
                }

                template.Files.Add(new OfficeDevPnP.Core.Framework.Provisioning.Model.File {
                    Src = source,
                    Folder = Folder,
                    Overwrite = true,
                });

                // Determine the output file name and path
                var outFileName = System.IO.Path.GetFileName(Path);
                var outPath = new System.IO.FileInfo(Path).DirectoryName;

                // Save the template back to the storage
                var fileSystemConnector = new FileSystemConnector(outPath, "");
                var formatter = XMLPnPSchemaFormatter.LatestFormatter;

                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(
                      Path, fileSystemConnector);
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
        }
    }
}
