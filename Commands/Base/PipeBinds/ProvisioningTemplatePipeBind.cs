using System;
using System.IO;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.Commands.Utilities;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ProvisioningTemplatePipeBind
    {
        private ProvisioningTemplate template;
        private string templatePath;

        public ProvisioningTemplatePipeBind(ProvisioningTemplate template)
        {
            this.template = template;
        }

        public ProvisioningTemplatePipeBind(string templatePath)
        {
            this.templatePath = templatePath;
        }

        public ProvisioningTemplate GetTemplate(string rootPath, Action<Exception> exceptionHandler)
        {
            if (this.template != null)
            {
                return this.template;
            }
            if(!string.IsNullOrEmpty(templatePath))
            {
                if (!System.IO.Path.IsPathRooted(templatePath))
                {
                    templatePath = System.IO.Path.Combine(rootPath, templatePath);
                }
                return LoadProvisioningTemplateFromFile(templatePath, (e) =>
                {
                    if(exceptionHandler != null)
                    {
                        exceptionHandler(e);
                    }
                });
            }
            return null;
        }

        internal static ProvisioningTemplate LoadProvisioningTemplateFromFile(string templatePath, Action<Exception> exceptionHandler)
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
                ProvisioningTemplate provisioningTemplate = provider.GetTemplate(templateFileName);
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
