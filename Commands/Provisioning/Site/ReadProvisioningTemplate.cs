using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace PnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommunications.Read, "PnPProvisioningTemplate")]
    [Alias("Load-PnPProvisioningTemplate")]
    [CmdletHelp("Loads/Reads a PnP file from the file system or a string",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Read-PnPProvisioningTemplate -Path template.pnp",
       Remarks = "Loads a PnP file from the file system",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Read-PnPProvisioningTemplate -Path template.pnp -TemplateProviderExtensions $extensions",
       Remarks = "Loads a PnP file from the file system using some custom template provider extensions while loading the file.",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Read-PnPProvisioningTemplate -Xml $xml",
       Remarks = "Reads a PnP Provisioning template from a string containing the XML of a provisioning template",
       SortOrder = 3)]
    public class ReadProvisioningTemplate : PSCmdlet
    {
        const string ParameterSet_PATH = "By Path";
        const string ParameterSet_XML = "By XML";

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_PATH, HelpMessage = "Filename to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSet_XML, HelpMessage = "Variable to read from, containing the valid XML of a provisioning template.")]
        public string Xml;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if (MyInvocation.InvocationName.ToLower() == "load-pnpprovisioningtemplate")
            {
                WriteWarning("Load-PnPProvisioningTemplate has been deprecated in favor of Read-PnPProvisioningTemplate which supports the same parameters.");
            }
            switch (ParameterSetName)
            {
                case ParameterSet_PATH:
                    {

                        if (!System.IO.Path.IsPathRooted(Path))
                        {
                            Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                        }
                        WriteObject(LoadProvisioningTemplateFromFile(Path, TemplateProviderExtensions, (e) =>
                        {
                            WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
                        }));
                        break;
                    }
                case ParameterSet_XML:
                    {
                        WriteObject(LoadProvisioningTemplateFromString(Xml, TemplateProviderExtensions, (e) =>
                        {
                            WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
                        }));
                        break;
                    }
            }
        }

        internal static ProvisioningTemplate LoadProvisioningTemplateFromString(string xml, ITemplateProviderExtension[] templateProviderExtensions, Action<Exception> exceptionHandler)
        {
            var formatter = new XMLPnPSchemaFormatter();

            XMLTemplateProvider provider = new XMLStreamTemplateProvider();

            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    return provider.GetTemplate(stream, templateProviderExtensions);
                }
            }
            catch (ApplicationException ex)
            {
                if (ex.InnerException is AggregateException)
                {
                    if (exceptionHandler != null)
                    {
                        foreach (var exception in ((AggregateException)ex.InnerException).InnerExceptions)
                        {
                            exceptionHandler(exception);
                        }
                    }
                }
            }
            return null;
        }

        internal static ProvisioningTemplate LoadProvisioningTemplateFromFile(string templatePath, ITemplateProviderExtension[] templateProviderExtensions, Action<Exception> exceptionHandler)
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
                if (!String.IsNullOrEmpty(openXmlConnector.Info?.Properties?.TemplateFileName))
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
            try
            {
                ProvisioningTemplate provisioningTemplate = provider.GetTemplate(templateFileName, templateProviderExtensions);
                provisioningTemplate.Connector = provider.Connector;
                return provisioningTemplate;
            }
            catch (ApplicationException ex)
            {
                if (ex.InnerException is AggregateException)
                {
                    if (exceptionHandler != null)
                    {
                        foreach (var exception in ((AggregateException)ex.InnerException).InnerExceptions)
                        {
                            exceptionHandler(exception);
                        }
                    }
                }
            }
            return null;
        }
    }
}
