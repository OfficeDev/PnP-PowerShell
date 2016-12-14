using System.IO;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsData.Convert, "PnPProvisioningTemplate")]
    [CmdletAlias("Convert-SPOProvisioningTemplate")]
    [CmdletHelp("Converts a provisioning template to a other schema version",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
     Code = @"PS:> Convert-PnPProvisioningTemplate -Path template.xml",
     Remarks = @"Converts a provisioning template to the latest schema and outputs the result to current console.",
     SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Convert-PnPProvisioningTemplate -Path template.xml -Out newtemplate.xml",
     Remarks = @"Converts a provisioning template to the latest schema and outputs the result the newtemplate.xml file.",
     SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Convert-PnPProvisioningTemplate -Path template.xml -Out newtemplate.xml -ToSchema V201512",
     Remarks = @"Converts a provisioning template to the latest schema using the 201512 schema and outputs the result the newtemplate.xml file.",
     SortOrder = 3)]
     [CmdletRelatedLink(
        Text ="Encoding", 
        Url = "https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx")]
    public class ConvertProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml file containing the provisioning template")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Filename to write to, optionally including full path")]
        public string Out;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "The schema of the output to use, defaults to the latest schema")]
        public XMLPnPSchemaVersion ToSchema = XMLPnPSchemaVersion.LATEST;

        [Parameter(Mandatory = false, HelpMessage = "The encoding type of the XML file, Unicode is default")]
        public System.Text.Encoding Encoding = System.Text.Encoding.Unicode;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists")]
        public SwitchParameter Force;

        protected override void BeginProcessing()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            if (MyInvocation.BoundParameters.ContainsKey("Out"))
            {
                if (!System.IO.Path.IsPathRooted(Out))
                {
                    Out = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
                }
            }

            FileInfo fileInfo = new FileInfo(Path);

            XMLTemplateProvider provider =
            new XMLFileSystemTemplateProvider(fileInfo.DirectoryName, "");

            var provisioningTemplate = provider.GetTemplate(fileInfo.Name);

            if (provisioningTemplate != null)
            {
                ITemplateFormatter formatter = null;
                switch (ToSchema)
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
                    case XMLPnPSchemaVersion.V201605:
                        {
                            formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2016_05);
                            break;
                        }
                }

                if (!string.IsNullOrEmpty(Out))
                {
                    if (File.Exists(Out))
                    {
                        if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, Out), Resources.Confirm))
                        {
                            File.WriteAllText(Out, provisioningTemplate.ToXML(formatter), Encoding);
                        }
                    }
                    else
                    {
                        File.WriteAllText(Out, provisioningTemplate.ToXML(formatter), Encoding);
                    }
                }
                else
                {
                    WriteObject(provisioningTemplate.ToXML(formatter));
                }
            }
        }
    }
}
