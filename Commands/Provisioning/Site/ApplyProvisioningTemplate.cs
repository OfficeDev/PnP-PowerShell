using System;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using System.Collections;
using System.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using System.Collections.Generic;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Base;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet("Apply", "PnPProvisioningTemplate")]
    [CmdletHelp("Applies a site template to a web",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path template.xml",
     Remarks = @"Applies a site template in XML format to the current web.",
     SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path template.xml -ResourceFolder c:\provisioning\resources",
     Remarks = @"Applies a site template in XML format to the current web. Any resources like files that are referenced in the template will be retrieved from the folder as specified with the ResourceFolder parameter.",
     SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path template.xml -Parameters @{""ListTitle""=""Projects"";""parameter2""=""a second value""}",
     Remarks = @"Applies a site template in XML format to the current web. It will populate the parameter in the template the values as specified and in the template you can refer to those values with the {parameter:<key>} token.

For instance with the example above, specifying {parameter:ListTitle} in your template will translate to 'Projects' when applying the template. These tokens can be used in most string values in a template.",
     SortOrder = 3)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path template.xml -Handlers Lists, SiteSecurity",
     Remarks = @"Applies a site template in XML format to the current web. It will only apply the lists and site security part of the template.",
     SortOrder = 4)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path template.pnp",
     Remarks = @"Applies a site template from a pnp package to the current web.",
     SortOrder = 5)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path https://tenant.sharepoint.com/sites/templatestorage/Documents/template.pnp",
     Remarks = @"Applies a site template from a pnp package stored in a library to the current web.",
     SortOrder = 6)]
    [CmdletExample(
        Code = @"
PS:> $handler1 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> $handler2 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler2
PS:> Apply-PnPProvisioningTemplate -Path NewTemplate.xml -ExtensibilityHandlers $handler1,$handler2",
        Remarks = @"This will create two new ExtensibilityHandler objects that are run while provisioning the template",
        SortOrder = 7)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path .\ -InputInstance $template",
     Remarks = @"Applies a site template from an in-memory instance of a ProvisioningTemplate type of the PnP Core Component, reading the supporting files, if any, from the current (.\) path. The syntax can be used together with any other supported parameters.",
     SortOrder = 8)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPProvisioningTemplate -Path .\template.xml -TemplateId ""MyTemplate""",
     Remarks = @"Applies the ProvisioningTemplate with the ID ""MyTemplate"" located in the template definition file template.xml.",
     SortOrder = 9)]

    public class ApplyProvisioningTemplate : PnPWebCmdlet
    {
        private ProgressRecord progressRecord = new ProgressRecord(0, "Activity", "Status");
        private ProgressRecord subProgressRecord = new ProgressRecord(1, "Activity", "Status");


        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml or pnp file containing the provisioning template.", ParameterSetName = "Path")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "ID of the template to use from the xml file containing the provisioning template. If not specified and multiple ProvisioningTemplate elements exist, the last one will be used.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string TemplateId;

        [Parameter(Mandatory = false, HelpMessage = "Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the provisioning template is located will be used.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string ResourceFolder;

        [Parameter(Mandatory = false, HelpMessage = "Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter OverwriteSystemPropertyBagValues;

        [Parameter(Mandatory = false, HelpMessage = "Ignore duplicate data row errors when the data row in the template already exists.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter IgnoreDuplicateDataRowErrors;

        [Parameter(Mandatory = false, HelpMessage = "If set content types will be provisioned if the target web is a subweb.")]
        public SwitchParameter ProvisionContentTypesToSubWebs;

        [Parameter(Mandatory = false, HelpMessage = "If set fields will be provisioned if the target web is a subweb.")]
        public SwitchParameter ProvisionFieldsToSubWebs;

        [Parameter(Mandatory = false, HelpMessage = "Override the RemoveExistingNodes attribute in the Navigation elements of the template. If you specify this value the navigation nodes will always be removed before adding the nodes in the template")]
        public SwitchParameter ClearNavigation;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify parameters that can be referred to in the template by means of the {parameter:<Key>} token. See examples on how to use this parameter.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Hashtable Parameters;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to only process a specific part of the template. Notice that this might fail, as some of the handlers require other artifacts in place if they are not part of what your applying. Visit https://docs.microsoft.com/dotnet/api/officedevpnp.core.framework.provisioning.model.handlers for possible values.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Handlers Handlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to run all handlers, excluding the ones specified.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Handlers ExcludeHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ExtensbilityHandlers to execute while applying a template", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ExtensibilityHandler[] ExtensibilityHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while applying a template.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to provide an in-memory instance of the ProvisioningTemplate type of the PnP Core Component. When using this parameter, the -Path parameter refers to the path of any supporting file for the template.", ParameterSetName = "Instance")]
        public ProvisioningTemplate InputInstance;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.Url);
            ProvisioningTemplate provisioningTemplate;

            FileConnectorBase fileConnector;
            if (ParameterSpecified(nameof(Path)))
            {
                bool templateFromFileSystem = !Path.ToLower().StartsWith("http");
                string templateFileName = System.IO.Path.GetFileName(Path);
                if (templateFromFileSystem)
                {
                    if (!System.IO.Path.IsPathRooted(Path))
                    {
                        Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                    }
                    if (!System.IO.File.Exists(Path))
                    {
                        throw new FileNotFoundException($"File not found");
                    }
                    if (!string.IsNullOrEmpty(ResourceFolder))
                    {
                        if (!System.IO.Path.IsPathRooted(ResourceFolder))
                        {
                            ResourceFolder = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path,
                                ResourceFolder);
                        }
                    }
                    var fileInfo = new FileInfo(Path);
                    fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");
                }
                else
                {
                    Uri fileUri = new Uri(Path);
                    var webUrl = Microsoft.SharePoint.Client.Web.WebUrlFromFolderUrlDirect(ClientContext, fileUri);
                    var templateContext = ClientContext.Clone(webUrl.ToString());

                    var library = Path.ToLower().Replace(templateContext.Url.ToLower(), "").TrimStart('/');
                    var idx = library.IndexOf("/", StringComparison.Ordinal);
                    library = library.Substring(0, idx);

                    // This syntax creates a SharePoint connector regardless we have the -InputInstance argument or not
                    fileConnector = new SharePointConnector(templateContext, templateContext.Url, library);
                }

                // If we don't have the -InputInstance parameter, we load the template from the source connector

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
                    if (templateFromFileSystem)
                    {
                        provider = new XMLFileSystemTemplateProvider(fileConnector.Parameters[FileConnectorBase.CONNECTIONSTRING] + "", "");
                    }
                    else
                    {
                        throw new NotSupportedException("Only .pnp package files are supported from a SharePoint library");
                    }
                }

                if (ParameterSpecified(nameof(TemplateId)))
                {
                    provisioningTemplate = provider.GetTemplate(templateFileName, TemplateId, null, TemplateProviderExtensions);
                }
                else
                {
                    provisioningTemplate = provider.GetTemplate(templateFileName, TemplateProviderExtensions);
                }

                if (provisioningTemplate == null)
                {
                    // If we don't have the template, raise an error and exit
                    WriteError(new ErrorRecord(new Exception("The -Path parameter targets an invalid repository or template object."), "WRONG_PATH", ErrorCategory.SyntaxError, null));
                    return;
                }

                if (isOpenOfficeFile)
                {
                    provisioningTemplate.Connector = provider.Connector;
                }
                else
                {
                    if (ResourceFolder != null)
                    {
                        var fileSystemConnector = new FileSystemConnector(ResourceFolder, "");
                        provisioningTemplate.Connector = fileSystemConnector;
                    }
                    else
                    {
                        provisioningTemplate.Connector = provider.Connector;
                    }
                }
            }

            else
            {
                provisioningTemplate = InputInstance;

                if (ResourceFolder != null)
                {
                    var fileSystemConnector = new FileSystemConnector(ResourceFolder, "");
                    provisioningTemplate.Connector = fileSystemConnector;
                }
                else
                {
                    if (Path != null)
                    {
                        if (!System.IO.Path.IsPathRooted(Path))
                        {
                            Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                        }
                    }
                    else
                    {
                        Path = SessionState.Path.CurrentFileSystemLocation.Path;
                    }
                    var fileInfo = new FileInfo(Path);
                    fileConnector = new FileSystemConnector(System.IO.Path.IsPathRooted(fileInfo.FullName) ? fileInfo.FullName : fileInfo.DirectoryName, "");
                    provisioningTemplate.Connector = fileConnector;
                }
            }

            if (Parameters != null)
            {
                foreach (var parameter in Parameters.Keys)
                {
                    if (provisioningTemplate.Parameters.ContainsKey(parameter.ToString()))
                    {
                        provisioningTemplate.Parameters[parameter.ToString()] = Parameters[parameter].ToString();
                    }
                    else
                    {
                        provisioningTemplate.Parameters.Add(parameter.ToString(), Parameters[parameter].ToString());
                    }
                }
            }

            var applyingInformation = new ProvisioningTemplateApplyingInformation();

            if (ParameterSpecified(nameof(Handlers)))
            {
                applyingInformation.HandlersToProcess = Handlers;
            }
            if (ParameterSpecified(nameof(ExcludeHandlers)))
            {
                foreach (var handler in (Handlers[])Enum.GetValues(typeof(Handlers)))
                {
                    if (!ExcludeHandlers.Has(handler) && handler != Handlers.All)
                    {
                        Handlers = Handlers | handler;
                    }
                }
                applyingInformation.HandlersToProcess = Handlers;
            }

            if (ExtensibilityHandlers != null)
            {
                applyingInformation.ExtensibilityHandlers = ExtensibilityHandlers.ToList();
            }

            applyingInformation.ProgressDelegate = (message, step, total) =>
            {
                if (message != null)
                {
                    var percentage = Convert.ToInt32((100 / Convert.ToDouble(total)) * Convert.ToDouble(step));
                    progressRecord.Activity = $"Applying template to {SelectedWeb.Url}";
                    progressRecord.StatusDescription = message;
                    progressRecord.PercentComplete = percentage;
                    progressRecord.RecordType = ProgressRecordType.Processing;
                    WriteProgress(progressRecord);
                }
            };

            var warningsShown = new List<string>();

            applyingInformation.MessagesDelegate = (message, type) =>
            {
                switch (type)
                {
                    case ProvisioningMessageType.Warning:
                        {
                            if (!warningsShown.Contains(message))
                            {
                                WriteWarning(message);
                                warningsShown.Add(message);
                            }
                            break;
                        }
                    case ProvisioningMessageType.Progress:
                        {
                            if (message != null)
                            {
                                var activity = message;
                                if (message.IndexOf("|") > -1)
                                {
                                    var messageSplitted = message.Split('|');
                                    if (messageSplitted.Length == 4)
                                    {
                                        var current = double.Parse(messageSplitted[2]);
                                        var total = double.Parse(messageSplitted[3]);
                                        subProgressRecord.RecordType = ProgressRecordType.Processing;
                                        subProgressRecord.Activity = string.IsNullOrEmpty(messageSplitted[0]) ? "-" : messageSplitted[0];
                                        subProgressRecord.StatusDescription = string.IsNullOrEmpty(messageSplitted[1]) ? "-" : messageSplitted[1];
                                        subProgressRecord.PercentComplete = Convert.ToInt32((100 / total) * current);
                                        WriteProgress(subProgressRecord);
                                    }
                                    else
                                    {
                                        subProgressRecord.Activity = "Processing";
                                        subProgressRecord.RecordType = ProgressRecordType.Processing;
                                        subProgressRecord.StatusDescription = activity;
                                        subProgressRecord.PercentComplete = 0;
                                        WriteProgress(subProgressRecord);
                                    }
                                }
                                else
                                {
                                    subProgressRecord.Activity = "Processing";
                                    subProgressRecord.RecordType = ProgressRecordType.Processing;
                                    subProgressRecord.StatusDescription = activity;
                                    subProgressRecord.PercentComplete = 0;
                                    WriteProgress(subProgressRecord);
                                }
                            }
                            break;
                        }
                    case ProvisioningMessageType.Completed:
                        {

                            WriteProgress(new ProgressRecord(1, message, " ") { RecordType = ProgressRecordType.Completed });
                            break;
                        }
                }
            };

            applyingInformation.OverwriteSystemPropertyBagValues = OverwriteSystemPropertyBagValues;
            applyingInformation.IgnoreDuplicateDataRowErrors = IgnoreDuplicateDataRowErrors;
            applyingInformation.ClearNavigation = ClearNavigation;
            applyingInformation.ProvisionContentTypesToSubWebs = ProvisionContentTypesToSubWebs;
            applyingInformation.ProvisionFieldsToSubWebs = ProvisionFieldsToSubWebs;

#if !ONPREMISES
            using (var provisioningContext = new PnPProvisioningContext(async (resource, scope) =>
            {
                // Get Azure AD Token
                if (PnPConnection.CurrentConnection != null)
                {
                    var graphAccessToken = PnPConnection.CurrentConnection.TryGetAccessToken(Enums.TokenAudience.MicrosoftGraph);
                    if (graphAccessToken != null)
                    {
                        // Authenticated using -Graph or using another way to retrieve the accesstoken with Connect-PnPOnline
                        return await Task.FromResult(graphAccessToken);
                        
                    }
                }

                if (PnPConnection.CurrentConnection.PSCredential != null)
                {
                    // Using normal credentials
                    return await Task.FromResult(TokenHandler.AcquireToken(resource, null));
                }
                else
                {
                    // No token...
                    return await Task.FromResult<string>(null);
                }
            }))
            {
#endif
                SelectedWeb.ApplyProvisioningTemplate(provisioningTemplate, applyingInformation);
#if !ONPREMISES
            }
#endif

            WriteProgress(new ProgressRecord(0, $"Applying template to {SelectedWeb.Url}", " ") { RecordType = ProgressRecordType.Completed });
        }
    }
}
