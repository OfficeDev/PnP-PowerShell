using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using File = System.IO.File;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using System.Collections;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.Get, "PnPProvisioningTemplate", SupportsShouldProcess = true)]
    [CmdletAlias("Get-SPOProvisioningTemplate")]
    [CmdletHelp("Generates a provisioning template from a web",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp",
       Remarks = "Extracts a provisioning template in Office Open XML from the current web.",
       SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.xml",
       Remarks = "Extracts a provisioning template in XML format from the current web.",
       SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -Schema V201503",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web and saves it in the V201503 version of the schema.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -IncludeAllTermGroups",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web and includes all term groups, term sets and terms from the Managed Metadata Service Taxonomy.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -IncludeSiteCollectionTermGroup",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web and includes the term group currently (if set) assigned to the site collection.",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistComposedLookFiles",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web and saves the files that make up the composed look to the same folder as where the template is saved.",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -Handlers Lists, SiteSecurity",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web, but only processes lists and site security when generating the template.",
        SortOrder = 7)]
    [CmdletExample(
        Code = @"
PS:> $handler1 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> $handler2 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> Get-PnPProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler1,$handler2",
        Remarks = @"This will create two new ExtensibilityHandler objects that are run during extraction of the template",
        SortOrder = 8)]
#if !SP2013
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistMultiLanguageResources",
        Introduction = "Only supported on SP2016 and SP Online",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web, and for supported artifacts it will create a resource file for each supported language (based upon the language settings of the current web). The generated resource files will be named after the value specified in the Out parameter. For instance if the Out parameter is specified as -Out 'template.xml' the generated resource file will be called 'template.en-US.resx'.",
        SortOrder = 9)]
    [CmdletExample(
        Code = @"PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistMultiLanguageResources -ResourceFilePrefix MyResources",
        Introduction = "Only supported on SP2016 and SP Online",
        Remarks = "Extracts a provisioning template in Office Open XML from the current web, and for supported artifacts it will create a resource file for each supported language (based upon the language settings of the current web). The generated resource files will be named 'MyResources.en-US.resx' etc.",
        SortOrder = 10)]
#endif
    [CmdletExample(
        Code = @"PS:> $template = Get-PnPProvisioningTemplate -OutputInstance",
        Remarks = "Extracts an instance of a provisioning template object from the current web. This syntax cannot be used together with the -Out parameter, but it can be used together with any other supported parameters.",
        SortOrder = 11)]
    [CmdletRelatedLink(
        Text = "Encoding",
        Url = "https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx")]
    public class GetProvisioningTemplate : SPOWebCmdlet
    {
        private ProgressRecord mainProgressRecord = new ProgressRecord(0, "Processing", "Status");
        private ProgressRecord subProgressRecord = new ProgressRecord(1, "Activity", "Status");

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Filename to write to, optionally including full path")]
        public string Out;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "The schema of the output to use, defaults to the latest schema")]
        public XMLPnPSchemaVersion Schema = XMLPnPSchemaVersion.LATEST;

        [Parameter(Mandatory = false, HelpMessage = "If specified, all term groups will be included. Overrides IncludeSiteCollectionTermGroup.")]
        public SwitchParameter IncludeAllTermGroups;

        [Parameter(Mandatory = false, HelpMessage = "If specified, all the site collection term groups will be included. Overridden by IncludeAllTermGroups.")]
        public SwitchParameter IncludeSiteCollectionTermGroup;

        [Parameter(Mandatory = false, HelpMessage = "If specified all site groups will be included.")]
        public SwitchParameter IncludeSiteGroups;

        [Parameter(Mandatory = false, HelpMessage = "If specified all the managers and contributors of term groups will be included.")]
        public SwitchParameter IncludeTermGroupsSecurity;

        [Parameter(Mandatory = false, HelpMessage = "If specified the template will contain the current search configuration of the site.")]
        public SwitchParameter IncludeSearchConfiguration;

        [Parameter(Mandatory = false, HelpMessage = "If specified the files used for masterpages, sitelogo, alternate CSS and the files that make up the composed look will be saved.")]
        public SwitchParameter PersistBrandingFiles;

        [Parameter(Mandatory = false, HelpMessage = "If specified the files making up the composed look (background image, font file and color file) will be saved.")]
        [Obsolete("Use PersistBrandingFiles instead.")]
        public SwitchParameter PersistComposedLookFiles;

        [Parameter(Mandatory = false, HelpMessage = "If specified the files used for the publishing feature will be saved.")]
        public SwitchParameter PersistPublishingFiles;

        [Parameter(Mandatory = false, HelpMessage = "If specified, out of the box / native publishing files will be saved.")]
        public SwitchParameter IncludeNativePublishingFiles;

        [Parameter(Mandatory = false, HelpMessage = "During extraction the version of the server will be checked for certain actions. If you specify this switch, this check will be skipped.")]
        public SwitchParameter SkipVersionCheck;

#if !SP2013
        [Parameter(Mandatory = false, HelpMessage = "If specified, resource values for applicable artifacts will be persisted to a resource file")]
        public SwitchParameter PersistMultiLanguageResources;

        [Parameter(Mandatory = false, HelpMessage = "If specified, resource files will be saved with the specified prefix instead of using the template name specified. If no template name is specified the files will be called PnP-Resources.<language>.resx. See examples for more info.")]
        public string ResourceFilePrefix;
#endif
        [Parameter(Mandatory = false, HelpMessage = "Allows you to only process a specific type of artifact in the site. Notice that this might result in a non-working template, as some of the handlers require other artifacts in place if they are not part of what your extracting.")]
        public Handlers Handlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to run all handlers, excluding the ones specified.")]
        public Handlers ExcludeHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ExtensbilityHandlers to execute while extracting a template.")]
        public ExtensibilityHandler[] ExtensibilityHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while extracting a template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Exports the template without the use of a base template, causing all OOTB artifacts to be included. Using this switch is generally not required/recommended.")]
        [Obsolete("Use of this method is generally not required/recommended")]
        public SwitchParameter NoBaseTemplate;

        [Parameter(Mandatory = false, HelpMessage = "The encoding type of the XML file, Unicode is default")]
        public System.Text.Encoding Encoding = System.Text.Encoding.Unicode;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify the DisplayName of the template file that will be extracted.")]
        public string TemplateDisplayName;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify the ImagePreviewUrl of the template file that will be extracted.")]
        public string TemplateImagePreviewUrl;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify custom Properties for the template file that will be extracted.")]
        public Hashtable TemplateProperties;

        [Parameter(Mandatory = false, HelpMessage = "Returns the template as an in-memory object, which is an instance of the ProvisioningTemplate type of the PnP Core Component. It cannot be used together with the -Out parameter.")]
        public SwitchParameter OutputInstance;

        protected override void ExecuteCmdlet()
        {
#if !SP2013
            if (PersistMultiLanguageResources == false && ResourceFilePrefix != null)
            {
                WriteWarning("In order to export resource files, also specify the PersistMultiLanguageResources switch");
            }
#endif
            if (!string.IsNullOrEmpty(Out))
            {
                if (!Path.IsPathRooted(Out))
                {
                    Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
                }
                if (File.Exists(Out))
                {
                    if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, Out), Resources.Confirm))
                    {
                        ExtractTemplate(Schema, new FileInfo(Out).DirectoryName, new FileInfo(Out).Name);
                    }
                }
                else
                {
                    ExtractTemplate(Schema, new FileInfo(Out).DirectoryName, new FileInfo(Out).Name);
                }
            }
            else
            {
                ExtractTemplate(Schema, SessionState.Path.CurrentFileSystemLocation.Path, null);
            }
        }

        private void ExtractTemplate(XMLPnPSchemaVersion schema, string path, string packageName)
        {
            SelectedWeb.EnsureProperty(w => w.Url);

            var creationInformation = new ProvisioningTemplateCreationInformation(SelectedWeb);

            if (MyInvocation.BoundParameters.ContainsKey("Handlers"))
            {
                creationInformation.HandlersToProcess = Handlers;
            }
            if (MyInvocation.BoundParameters.ContainsKey("ExcludeHandlers"))
            {
                foreach (var handler in (Handlers[])Enum.GetValues(typeof(Handlers)))
                {
                    if (!ExcludeHandlers.Has(handler) && handler != Handlers.All)
                    {
                        Handlers = Handlers | handler;
                    }
                }
                creationInformation.HandlersToProcess = Handlers;
            }

            var extension = "";
            if (packageName != null)
            {
                if (packageName.IndexOf(".", StringComparison.Ordinal) > -1)
                {
                    extension = packageName.Substring(packageName.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                }
                else
                {
                    packageName += ".pnp";
                    extension = ".pnp";
                }
            }

            var fileSystemConnector = new FileSystemConnector(path, "");
            if (extension == ".pnp")
            {
                creationInformation.FileConnector = new OpenXMLConnector(packageName, fileSystemConnector);
            }
            else
            {
                creationInformation.FileConnector = fileSystemConnector;
            }
#pragma warning disable 618
            creationInformation.PersistBrandingFiles = PersistBrandingFiles || PersistComposedLookFiles;
#pragma warning restore 618
            creationInformation.PersistPublishingFiles = PersistPublishingFiles;
            creationInformation.IncludeNativePublishingFiles = IncludeNativePublishingFiles;
            creationInformation.IncludeSiteGroups = IncludeSiteGroups;
            creationInformation.IncludeTermGroupsSecurity = IncludeTermGroupsSecurity;
            creationInformation.IncludeSearchConfiguration = IncludeSearchConfiguration;
            creationInformation.SkipVersionCheck = SkipVersionCheck;
#if !SP2013
            creationInformation.PersistMultiLanguageResources = PersistMultiLanguageResources;
            if (!string.IsNullOrEmpty(ResourceFilePrefix))
            {
                creationInformation.ResourceFilePrefix = ResourceFilePrefix;
            }
            else
            {
                if (Out != null)
                {
                    FileInfo fileInfo = new FileInfo(Out);
                    var prefix = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal));
                    creationInformation.ResourceFilePrefix = prefix;
                }

            }
#endif
            if (ExtensibilityHandlers != null)
            {
                creationInformation.ExtensibilityHandlers = ExtensibilityHandlers.ToList();
            }

#pragma warning disable CS0618 // Type or member is obsolete
            if (NoBaseTemplate)
            {
                creationInformation.BaseTemplate = null;
            }
            else
            {
                creationInformation.BaseTemplate = SelectedWeb.GetBaseTemplate();
            }
#pragma warning restore CS0618 // Type or member is obsolete

            creationInformation.ProgressDelegate = (message, step, total) =>
                {
                    var percentage = Convert.ToInt32((100 / Convert.ToDouble(total)) * Convert.ToDouble(step));

                    WriteProgress(new ProgressRecord(0, $"Extracting Template from {SelectedWeb.Url}", message) { PercentComplete = percentage });
                };
            creationInformation.MessagesDelegate = (message, type) =>
            {
                switch (type)
                {
                    case ProvisioningMessageType.Warning:
                        {
                            WriteWarning(message);
                            break;
                        }
                    case ProvisioningMessageType.Progress:
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
                                    subProgressRecord.Activity = messageSplitted[0];
                                    subProgressRecord.StatusDescription = messageSplitted[1];
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
                            break;
                        }
                    case ProvisioningMessageType.Completed:
                        {
                            WriteProgress(new ProgressRecord(1, message, " ") { RecordType = ProgressRecordType.Completed });
                            break;
                        }
                }
            };

            if (IncludeAllTermGroups)
            {
                creationInformation.IncludeAllTermGroups = true;
            }
            else
            {
                if (IncludeSiteCollectionTermGroup)
                {
                    creationInformation.IncludeSiteCollectionTermGroup = true;
                }
            }

            var template = SelectedWeb.GetProvisioningTemplate(creationInformation);

            // Set metadata for template, if any
            SetTemplateMetadata(template, TemplateDisplayName, TemplateImagePreviewUrl, TemplateProperties);

            if (!OutputInstance)
            {
                ITemplateFormatter formatter = null;
                switch (schema)
                {
                    case XMLPnPSchemaVersion.LATEST:
                        {
                            formatter = XMLPnPSchemaFormatter.LatestFormatter;
                            break;
                        }
                    case XMLPnPSchemaVersion.V201503:
                        {
#pragma warning disable CS0618 // Type or member is obsolete
                            formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_03);
#pragma warning restore CS0618 // Type or member is obsolete
                            break;
                        }
                    case XMLPnPSchemaVersion.V201505:
                        {
#pragma warning disable CS0618 // Type or member is obsolete
                            formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_05);
#pragma warning restore CS0618 // Type or member is obsolete
                            break;
                        }
                    case XMLPnPSchemaVersion.V201508:
                        {
#pragma warning disable CS0618 // Type or member is obsolete
                            formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_08);
#pragma warning restore CS0618 // Type or member is obsolete
                            break;
                        }
                    case XMLPnPSchemaVersion.V201512:
                        {
                            formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_12);
                            break;
                        }
                }

                if (extension == ".pnp")
                {
                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(

                          creationInformation.FileConnector as OpenXMLConnector);
                    var templateFileName = packageName.Substring(0, packageName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                    provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
                }
                else
                {
                    if (Out != null)
                    {
                        XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(path, "");
                        provider.SaveAs(template, Path.Combine(path, packageName), formatter, TemplateProviderExtensions);
                    }
                    else
                    {
                        var outputStream = formatter.ToFormattedTemplate(template);
                        var reader = new StreamReader(outputStream);

                        WriteObject(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                WriteObject(template);
            }
        }

        public static void SetTemplateMetadata(ProvisioningTemplate template, string templateDisplayName, string templateImagePreviewUrl, Hashtable templateProperties)
        {
            if (!String.IsNullOrEmpty(templateDisplayName))
            {
                template.DisplayName = templateDisplayName;
            }

            if (!String.IsNullOrEmpty(templateImagePreviewUrl))
            {
                template.ImagePreviewUrl = templateImagePreviewUrl;
            }

            if (templateProperties != null && templateProperties.Count > 0)
            {
                var properties = templateProperties
                    .Cast<DictionaryEntry>()
                    .ToDictionary(i => (String)i.Key, i => (String)i.Value);

                foreach (var key in properties.Keys)
                {
                    if (!String.IsNullOrEmpty(key))
                    {
                        template.Properties[key] = properties[key];
                    }
                }
            }
        }
    }
}
