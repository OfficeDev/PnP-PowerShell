#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.IO;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommunications.Read, "PnPTenantTemplate")]
    [CmdletHelp("Loads/Reads a PnP tenant template from the file system and returns an in-memory instance of this template.",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Read-PnPTenantTemplate -Path template.pnp",
       Remarks = "Reads a PnP tenant template file from the file system and returns an in-memory instance",
       SortOrder = 1)]
    public class ReadTenantTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            WriteObject(LoadProvisioningHierarchyFromFile(Path, TemplateProviderExtensions, (e) =>
            {
                WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
            }));
        }

        internal static ProvisioningHierarchy LoadProvisioningHierarchyFromFile(string templatePath, ITemplateProviderExtension[] templateProviderExtensions, Action<Exception> exceptionHandler)
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
                if(ex.InnerException is AggregateException)
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
#endif