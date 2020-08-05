using System;
using System.IO;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.Commands.Utilities;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ProvisioningHierarchyPipeBind
    {
        private ProvisioningHierarchy template;
        private string templatePath;

        public ProvisioningHierarchyPipeBind(ProvisioningHierarchy template)
        {
            this.template = template;
        }

        public ProvisioningHierarchyPipeBind(string templatePath)
        {
            this.templatePath = templatePath;
        }

        public ProvisioningHierarchy GetTemplate(string rootPath, Action<Exception> exceptionHandler)
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
                return LoadProvisioningHierarchyFromFile(templatePath, (e) =>
                {
                    if(exceptionHandler != null)
                    {
                        exceptionHandler(e);
                    }
                });
            }
            return null;
        }

        private static ProvisioningHierarchy LoadProvisioningHierarchyFromFile(string templatePath, Action<Exception> exceptionHandler)
        {
            // Prepare the File Connector
            string templateFileName = System.IO.Path.GetFileName(templatePath);

            // Prepare the template path
            var fileInfo = new FileInfo(templatePath);
            FileConnectorBase fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");

            // Load the provisioning template file
            var isOpenOfficeFile = false;
            using (var stream = fileConnector.GetFileStream(templateFileName))
            {
                isOpenOfficeFile = FileUtilities.IsOpenOfficeFile(stream);
            }

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

                var hierarchy = (provider as XMLOpenXMLTemplateProvider).GetHierarchy();
                if (hierarchy != null)
                {
                    hierarchy.Connector = provider.Connector;
                    return hierarchy;
                }
            }
            else
            {
                provider = new XMLFileSystemTemplateProvider(fileConnector.Parameters[FileConnectorBase.CONNECTIONSTRING] + "", "");
            }

            try
            {
                ProvisioningHierarchy provisioningHierarchy = provider.GetHierarchy(templateFileName);
                provisioningHierarchy.Connector = provider.Connector;
                return provisioningHierarchy;
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
