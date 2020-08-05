#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsData.Save, "PnPTenantTemplate")]
    [CmdletHelp("Saves a PnP provisioning hierarchy to the file system",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Save-PnPTenantTemplate -Template template.xml -Out .\tenanttemplate.pnp",
       Remarks = "Saves a PnP tenant template to the file system",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> $template = Read-PnPTenantTemplate -Path template.xml
PS:> Save-PnPTenantTemplate -Template $template -Out .\template.pnp",
       Remarks = "Saves a PnP tenant template to the file system as a PnP file. The schema used will the latest released schema when creating the PnP file regardless of the original schema",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> $template = Read-PnPTenantTemplate -Path template.xml
PS:> Save-PnPTenantTemplate -Template $template -Out .\template.pnp -Schema V202002",
       Remarks = "Saves a PnP tenant template to the file system as a PnP file and converts the template in the PnP file to the specified schema.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Read-PnPTenantTemplate -Path template.xml | Save-PnPTenantTemplate -Out .\template.pnp",
       Remarks = "Saves a PnP tenant template to the file system as a PnP file.",
       SortOrder = 4)]
    public class SaveTenantTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Allows you to provide an in-memory instance of a Tenant Template or a filename of a template file in XML format. When using this parameter, the -Out parameter refers to the path for saving the template and storing any supporting file for the template.")]
        public ProvisioningHierarchyPipeBind Template;

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to write to, optionally including full path.")]
        public string Out;

        [Parameter(Mandatory = false, HelpMessage = "The optional schema to use when creating the PnP file. Always defaults to the latest schema.")]
        public XMLPnPSchemaVersion Schema = XMLPnPSchemaVersion.LATEST;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var templateObject = Template.GetTemplate(SessionState.Path.CurrentFileSystemLocation.Path, (e) =>
             {
                 WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
             });

            // Determine the output file name and path
            string outFileName = Path.GetFileName(Out);

            if (!Path.IsPathRooted(Out))
            {
                Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
            }

            bool proceed = false;

            if (System.IO.File.Exists(Out))
            {
                if (Force || ShouldContinue(string.Format(Properties.Resources.File0ExistsOverwrite, Out),
                    Properties.Resources.Confirm))
                {
                    System.IO.File.Delete(Out);
                    proceed = true;
                }
            }
            else
            {
                proceed = true;
            }

            string outPath = new FileInfo(Out).DirectoryName;

            // Determine if it is an .XML or a .PNP file
            var extension = "";
            if (proceed && outFileName != null)
            {
                if (outFileName.IndexOf(".", StringComparison.Ordinal) > -1)
                {
                    extension = outFileName.Substring(outFileName.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                }
                else
                {
                    extension = ".pnp";
                }
            }

            var fileSystemConnector = new FileSystemConnector(outPath, "");

            var formatter = ProvisioningHelper.GetFormatter(Schema);

            if (extension == ".pnp")
            {
#if !PNPPSCORE
                IsolatedStorage.InitializeIsolatedStorage();
#endif
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";
                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(
                      Out, fileSystemConnector, templateFileName: templateFileName);
                WriteObject("Processing template");
                provider.SaveAs(templateObject, templateFileName, formatter);
                ProcessFiles(templateObject, Out, fileSystemConnector, provider.Connector, (message) =>
                {
                    WriteObject(message);
                });
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(outPath, "");
                provider.SaveAs(templateObject, Out, formatter);
            }
        }

        internal static void ProcessFiles(ProvisioningHierarchy tenantTemplate, string templateFileName, FileConnectorBase fileSystemConnector, FileConnectorBase connector, Action<string> progress)
        {
            var templateFile = ReadTenantTemplate.LoadProvisioningHierarchyFromFile(templateFileName, null, null);
            if (tenantTemplate.Tenant?.AppCatalog != null)
            {
                foreach (var app in tenantTemplate.Tenant.AppCatalog.Packages)
                {
                    progress($"Processing {app.Src}");
                    AddFile(app.Src, templateFile, fileSystemConnector, connector);
                }
            }
            if (tenantTemplate.Tenant?.SiteScripts != null)
            {
                foreach (var siteScript in tenantTemplate.Tenant.SiteScripts)
                {
                    progress($"Processing {siteScript.JsonFilePath}");
                    AddFile(siteScript.JsonFilePath, templateFile, fileSystemConnector, connector);
                }
            }
            if (tenantTemplate.Localizations != null && tenantTemplate.Localizations.Any())
            {
                foreach (var location in tenantTemplate.Localizations)
                {
                    progress($"Processing {location.ResourceFile}");
                    AddFile(location.ResourceFile, templateFile, fileSystemConnector, connector);
                }
            }
            foreach (var template in tenantTemplate.Templates)
            {
                if (template.WebSettings != null && !String.IsNullOrEmpty(template.WebSettings.SiteLogo))
                {
                    // is it a file?
                    var isFile = false;
                    try
                    {
                        using (var fileStream = fileSystemConnector.GetFileStream(template.WebSettings.SiteLogo))
                        {
                            isFile = fileStream != null;
                        }
                    }
                    catch { }
                    if (isFile)
                    {
                        progress($"Processing {template.WebSettings.SiteLogo}");
                        AddFile(template.WebSettings.SiteLogo, templateFile, fileSystemConnector, connector);
                    }
                }
                if (template.Files.Any())
                {
                    foreach (var file in template.Files)
                    {
                        progress($"Processing {file.Src}");
                        AddFile(file.Src, templateFile, fileSystemConnector, connector);
                    }
                }
                if (template.Lists.Any())
                {
                    foreach (var list in template.Lists)
                    {
                        if (list.DataRows.Any())
                        {
                            foreach (var dataRow in list.DataRows)
                            {
                                if (dataRow.Attachments.Any())
                                {
                                    progress("List attachments");
                                    foreach (var attachment in dataRow.Attachments)
                                    {
                                        AddFile(attachment.Src, templateFile, fileSystemConnector, connector);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (templateFile.Connector is ICommitableFileConnector)
            {
                ((ICommitableFileConnector)templateFile.Connector).Commit();
            }
        }

        private static void AddFile(string sourceName, ProvisioningHierarchy hierarchy, FileConnectorBase fileSystemConnector, FileConnectorBase connector)
        {
            using (var fs = fileSystemConnector.GetFileStream(sourceName))
            {
                var fileName = sourceName.IndexOf("\\") > 0 ? sourceName.Substring(sourceName.LastIndexOf("\\") + 1) : sourceName;
                var folderName = sourceName.IndexOf("\\") > 0 ? sourceName.Substring(0, sourceName.LastIndexOf("\\")) : "";
                hierarchy.Connector.SaveFileStream(fileName, folderName, fs);
            }
        }
    }
}
#endif