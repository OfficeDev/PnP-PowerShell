using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using File = System.IO.File;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsData.Import, "PnPTermGroupFromXml", SupportsShouldProcess = true)]
    [CmdletAlias("Import-SPOTermGroupFromXml")]
    [CmdletHelp("Imports a taxonomy TermGroup from either the input or from an XML file.",
        Category = CmdletHelpCategory.Taxonomy)]
    [CmdletExample(
        Code = @"PS:> Import-PnPTermGroupFromXml -Xml $xml",
        Remarks = "Imports the XML based termgroup information located in the $xml variable",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Import-PnPTermGroupFromXml -Path input.xml",
        Remarks = "Imports the XML file specified by the path.",
        SortOrder = 2)]
    public class ImportTermGroupFromXml : SPOCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The XML to process", Position = 0, ValueFromPipeline = true, ParameterSetName = "XML")]
        public string Xml;

        [Parameter(Mandatory = false, HelpMessage = "The XML File to import the data from", ParameterSetName = "File")]
        public string Path;


        protected override void ExecuteCmdlet()
        {
            var template = new ProvisioningTemplate();
            template.Security = null;
            template.Features = null;
            template.CustomActions = null;
            template.ComposedLook = null;

            template.Id = "TAXONOMYPROVISIONING";

            var outputStream = XMLPnPSchemaFormatter.LatestFormatter.ToFormattedTemplate(template);

            var reader = new StreamReader(outputStream);

            var fullXml = reader.ReadToEnd();

            var document = XDocument.Parse(fullXml);

            XElement termGroupsElement;
            if (MyInvocation.BoundParameters.ContainsKey("Xml"))
            {
                termGroupsElement = XElement.Parse(Xml);
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                termGroupsElement = XElement.Parse(File.ReadAllText(Path));
            }

            //XNamespace pnp = XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_25_12;
            var templateElement = document.Root.Descendants(document.Root.GetNamespaceOfPrefix("pnp") + "ProvisioningTemplate").FirstOrDefault();

            templateElement?.Add(termGroupsElement);

            var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            var completeTemplate = XMLPnPSchemaFormatter.LatestFormatter.ToProvisioningTemplate(stream);

            ProvisioningTemplateApplyingInformation templateAI = new ProvisioningTemplateApplyingInformation();
            templateAI.HandlersToProcess = Handlers.TermGroups;
            ClientContext.Web.ApplyProvisioningTemplate(completeTemplate, templateAI);

        }

    }
}
