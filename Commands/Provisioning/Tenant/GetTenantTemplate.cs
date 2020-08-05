#if !ONPREMISES
using System;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using PnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using File = System.IO.File;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using PnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.Core.Framework.Provisioning.Model.Configuration;
using PnP.PowerShell.Commands.Base;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantTemplate", SupportsShouldProcess = true)]
    [CmdletHelp("Generates a provisioning tenant template from a site. If the site is a hubsite any connected site will be included.",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Get-PnPTenantTemplate -Out tenanttemplate.xml",
       Remarks = "Extracts a tenant template",
       SortOrder = 1)]
    public class GetTenantTemplate : PnPAdminCmdlet
    {
        const string PARAMETERSET_ASFILE = "Extract a template to a file";
        const string PARAMETERSET_ASOBJECT = "Extract a template as an object";

        private ProgressRecord mainProgressRecord = new ProgressRecord(0, "Processing", "Status");
        private ProgressRecord subProgressRecord = new ProgressRecord(1, "Activity", "Status");

        [Parameter(Mandatory = false, ParameterSetName = PARAMETERSET_ASFILE)]
        [Parameter(Mandatory = false, ParameterSetName = PARAMETERSET_ASOBJECT)]
        public string SiteUrl;

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to write to, optionally including full path", ParameterSetName = PARAMETERSET_ASFILE)]
        public string Out;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.", ParameterSetName = PARAMETERSET_ASFILE)]
        public SwitchParameter Force;

        [Parameter(Mandatory = true, HelpMessage = "Returns the template as an in-memory object, which is an instance of the ProvisioningHierarchy type of the PnP Core Component. It cannot be used together with the -Out parameter.", ParameterSetName = PARAMETERSET_ASOBJECT)]
        public SwitchParameter AsInstance;

        [Parameter(Mandatory = false, HelpMessage = "Specify a JSON configuration file to configure the extraction progress.", ParameterSetName = PARAMETERSET_ASFILE)]
        [Parameter(Mandatory = false, HelpMessage = "Specify a JSON configuration file to configure the extraction progress.", ParameterSetName = PARAMETERSET_ASOBJECT)]
        public ExtractConfigurationPipeBind Configuration;

        protected override void ExecuteCmdlet()
        {
            
            ExtractConfiguration extractConfiguration = null;
            if (ParameterSpecified(nameof(Configuration)))
            {
                extractConfiguration = Configuration.GetConfiguration(SessionState.Path.CurrentFileSystemLocation.Path);
                if(!string.IsNullOrEmpty(SiteUrl))
                {
                    if(extractConfiguration.Tenant.Sequence == null)
                    {
                        extractConfiguration.Tenant.Sequence = new OfficeDevPnP.Core.Framework.Provisioning.Model.Configuration.Tenant.Sequence.ExtractSequenceConfiguration();
                    }
                    extractConfiguration.Tenant.Sequence.SiteUrls.Add(SiteUrl);
                }
            }

            if (ParameterSetName == PARAMETERSET_ASFILE)
            {
                ProvisioningHierarchy tenantTemplate = null;

                if (!Path.IsPathRooted(Out))
                {
                    Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
                }
                if(Out.ToLower().EndsWith(".pnp"))
                {
                    WriteWarning("This cmdlet does not save a tenant template as a PnP file.");
                }
                var fileInfo = new FileInfo(Out);
                var fileSystemConnector = new FileSystemConnector(fileInfo.DirectoryName, "");

                extractConfiguration.FileConnector = fileSystemConnector;

                var proceed = false;
                if (File.Exists(Out))
                {
                    if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, Out), Resources.Confirm))
                    {
                        proceed = true;
                    }
                }
                else
                {
                    proceed = true;
                }

                if (proceed)
                {
                    tenantTemplate = ExtractTemplate(extractConfiguration);

                    XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(fileInfo.DirectoryName, "");
                    provider.SaveAs(tenantTemplate, Out);
                }
            }
            else
            {
                WriteObject(ExtractTemplate(extractConfiguration));
            }
        }

        private ProvisioningHierarchy ExtractTemplate(ExtractConfiguration configuration)
        {
            configuration.ProgressDelegate = (message, step, total) =>
            {
                var percentage = Convert.ToInt32((100 / Convert.ToDouble(total)) * Convert.ToDouble(step));

                WriteProgress(new ProgressRecord(0, $"Extracting Tenant Template", message) { PercentComplete = percentage });
                WriteProgress(new ProgressRecord(1, " ", " ") { RecordType = ProgressRecordType.Completed });
            };
            configuration.MessagesDelegate = (message, type) =>
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
            using (var provisioningContext = new PnPProvisioningContext((resource, scope) =>
            {
                // Get Azure AD Token
                if (PnPConnection.CurrentConnection != null)
                {
                    var graphAccessToken = PnPConnection.CurrentConnection.TryGetAccessToken(Enums.TokenAudience.MicrosoftGraph);
                    if (graphAccessToken != null)
                    {
                        // Authenticated using -Graph or using another way to retrieve the accesstoken with Connect-PnPOnline
                        return Task.FromResult(graphAccessToken);
                    }
                }

                if (PnPConnection.CurrentConnection.PSCredential != null)
                {
                    // Using normal credentials
                    return Task.FromResult(TokenHandler.AcquireToken(resource, null));
                }
                else
                {
                    // No token...
                    return null;
                }
            }))
            {
                return Tenant.GetTenantTemplate(configuration);
            }
        }
    }

}
#endif