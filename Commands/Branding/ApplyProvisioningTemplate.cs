using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using OfficeDevPnP.Core.Utilities;
using System.Xml.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using System.Collections;

namespace OfficeDevPnP.PowerShell.Commands.Branding
{
    [Cmdlet("Apply", "SPOProvisioningTemplate")]
    [CmdletHelp("Applies a provisioning template to a web",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
     Code = @"
    PS:> Apply-SPOProvisioningTemplate -Path template.xml
",
     Remarks = @"Applies a provisioning template in XML format to the current web.
",
     SortOrder = 1)]
    [CmdletExample(
     Code = @"
    PS:> Apply-SPOProvisioningTemplate -Path template.xml -ResourceFolder c:\provisioning\resources
",
     Remarks = @"Applies a provisioning template in XML format to the current web. Any resources like files that are referenced in the template will be retrieved from the folder as specified with the ResourceFolder parameter.
",
     SortOrder = 2)]

    [CmdletExample(
     Code = @"
    PS:> Apply-SPOProvisioningTemplate -Path template.xml -Parameters @{""ListTitle""=""Projects"";""parameter2""=""a second value""}
",
     Remarks = @"Applies a provisioning template in XML format to the current web. It will populate the parameter in the template the values as specified and in the template you can refer to those values with the {parameter:<key>} token.

For instance with the example above, specifying {parameter:ListTitle} in your template will translate to 'Projects' when applying the template. These tokens can be used in most string values in a template.",
     SortOrder = 3)]
    public class ApplyProvisioningTemplate : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml file containing the provisioning template.")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the provisioning template is located will be used.")]
        public string ResourceFolder;

        [Parameter(Mandatory = false, HelpMessage = "Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)")]
        public SwitchParameter OverwriteSystemPropertyBagValues;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify parameters that can be referred to in the template by means of the {parameter:<Key>} token. See examples on how to use this parameter.")]
        public Hashtable Parameters;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.Url);

            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            if (!string.IsNullOrEmpty(ResourceFolder))
            {
                if (System.IO.Path.IsPathRooted(ResourceFolder))
                {
                    ResourceFolder = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, ResourceFolder);
                }
            }

            FileInfo fileInfo = new FileInfo(Path);

            XMLTemplateProvider provider =
                new XMLFileSystemTemplateProvider(fileInfo.DirectoryName, "");

            var provisioningTemplate = provider.GetTemplate(fileInfo.Name);

            if (provisioningTemplate != null)
            {
                FileSystemConnector fileSystemConnector = null;
                if (string.IsNullOrEmpty(ResourceFolder))
                {
                    fileSystemConnector = new FileSystemConnector(fileInfo.DirectoryName, "");
                }
                else
                {
                    fileSystemConnector = new FileSystemConnector(ResourceFolder, "");
                }
                provisioningTemplate.Connector = fileSystemConnector;

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

                applyingInformation.ProgressDelegate = (message, step, total) =>
                {
                    WriteProgress(new ProgressRecord(0, string.Format("Applying template to {0}", SelectedWeb.Url), message) { PercentComplete = (100 / total) * step });
                };

                applyingInformation.MessagesDelegate = (message, type) =>
                {
                    if (type == ProvisioningMessageType.Warning)
                    {
                        WriteWarning(message);
                    }
                };

                applyingInformation.OverwriteSystemPropertyBagValues = OverwriteSystemPropertyBagValues;
                SelectedWeb.ApplyProvisioningTemplate(provisioningTemplate, applyingInformation);
            }
        }
    }
}
