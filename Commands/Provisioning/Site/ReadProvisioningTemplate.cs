using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.IO;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{

    [Cmdlet(VerbsCommunications.Read, "PnPProvisioningTemplate")]
    [Alias("Load-PnPProvisioningTemplate")]
    [CmdletHelp("Loads/Reads a PnP file from the file system",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Read-PnPProvisioningTemplate -Path template.pnp",
       Remarks = "Loads a PnP file from the file system",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Read-PnPProvisioningTemplate -Path template.pnp -TemplateProviderExtensions $extensions",
       Remarks = "Loads a PnP file from the file system using some custom template provider extenions while loading the file.",
       SortOrder = 2)]
    public class ReadProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if(MyInvocation.InvocationName.ToLower() == "load-pnpprovisioningtemplate")
            {
                WriteWarning("Load-PnPProvisioningTemplate has been deprecated in favor of Read-PnPProvisioningTemplate which supports the same parameters.");
            }
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            WriteObject(LoadProvisioningTemplateFromFile(Path, TemplateProviderExtensions));
        }

        internal static ProvisioningTemplate LoadProvisioningTemplateFromFile(string templatePath, ITemplateProviderExtension[] templateProviderExtensions)
        {
            // Prepare the File Connector
            string templateFileName = System.IO.Path.GetFileName(templatePath);

            // Prepare the template path
            var fileInfo = new FileInfo(templatePath);
            FileConnectorBase fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");

            // Load the provisioning template file
            Stream stream = fileConnector.GetFileStream(templateFileName);
            var isOpenOfficeFile = FileUtilities.IsOpenOfficeFile(stream);

            XMLTemplateProvider provider;
            if (isOpenOfficeFile)
            {
                var openXmlConnector = new OpenXMLConnector(templateFileName, fileConnector);
                provider = new XMLOpenXMLTemplateProvider(openXmlConnector);
                if (openXmlConnector.Info?.Properties?.TemplateFileName != null)
                {
                    templateFileName = openXmlConnector.Info.Properties.TemplateFileName;
                }
                else
                {
                    templateFileName = templateFileName.Substring(0, templateFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";
                }
            }
            else
            {
                provider = new XMLFileSystemTemplateProvider(fileConnector.Parameters[FileConnectorBase.CONNECTIONSTRING] + "", "");
            }

            ProvisioningTemplate provisioningTemplate = provider.GetTemplate(templateFileName, templateProviderExtensions);
            provisioningTemplate.Connector = provider.Connector;

            // Return the result
            return provisioningTemplate;
        }
    }
}
