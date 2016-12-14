using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet("Remove", "PnPFileFromProvisioningTemplate")]
    [CmdletAlias("Remove-SPOFileFromProvisioningTemplate")]
    [CmdletHelp("Removes a file from an in-memory PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Remove-PnPFileFromProvisioningTemplate -Path template.pnp -FilePath filePath",
       Remarks = "Removes a file from an in-memory PnP Provisioning Template",
       SortOrder = 1)]
    public class RemoveFileFromProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to read the template from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "The relative File Path of the file to remove from the in-memory template")]
        public string FilePath;

        [Parameter(Mandatory = false, Position = 2, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while saving the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if (String.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentNullException("FilePath");
            }

            // Load the template
            ProvisioningTemplate template = LoadProvisioningTemplate
                .LoadProvisioningTemplateFromFile(Path,
                SessionState.Path.CurrentFileSystemLocation.Path,
                TemplateProviderExtensions);

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }

            var fileToRemove = template.Files.FirstOrDefault(f => f.Src == FilePath);
            if (fileToRemove != null)
            {
                template.Files.Remove(fileToRemove);
                template.Connector.DeleteFile(FilePath);

                if (template.Connector is ICommitableFileConnector)
                {
                    ((ICommitableFileConnector)template.Connector).Commit();
                }

                // Determine the output file name and path
                string outFileName = System.IO.Path.GetFileName(Path);
                string outPath = new System.IO.FileInfo(Path).DirectoryName;

                // Save the template back to the storage
                var fileSystemConnector = new FileSystemConnector(outPath, "");
                ITemplateFormatter formatter = XMLPnPSchemaFormatter.LatestFormatter;

                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(
                      Path, fileSystemConnector);
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
        }
    }
}
