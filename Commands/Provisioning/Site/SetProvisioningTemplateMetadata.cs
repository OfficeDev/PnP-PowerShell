using System;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using System.Collections;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using PnP.PowerShell.Commands.Utilities;

namespace PnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPProvisioningTemplateMetadata")]
    [CmdletHelp("Sets metadata of a provisioning template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateDisplayName ""DisplayNameValue""",
     Remarks = @"Sets the DisplayName property of a site template in XML format.",
     SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateDisplayName ""DisplayNameValue""",
     Remarks = @"Sets the DisplayName property of a site template in Office Open XML format.",
     SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateImagePreviewUrl ""Full URL of the Image Preview""",
     Remarks = @"Sets the Url to the preview image of a site template in XML format.",
     SortOrder = 3)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateImagePreviewUrl ""Full URL of the Image Preview""",
     Remarks = @"Sets the to the preview image of a site template in Office Open XML format.",
     SortOrder = 4)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateProperties @{""Property1"" = ""Test Value 1""; ""Property2""=""Test Value 2""}",
     Remarks = @"Sets the property 'Property1' to the value 'Test Value 1' of a site template in XML format.",
     SortOrder = 5)]
    [CmdletExample(
     Code = @"PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateProperties @{""Property1"" = ""Test Value 1""; ""Property2""=""Test Value 2""}",
     Remarks = @"Sets the property 'Property1' to the value 'Test Value 1' of a site template in Office Open XML format.",
     SortOrder = 6)]


    public class SetProvisioningTemplateMetadata : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml or pnp file containing the site template.")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify the DisplayName of the template file that will be updated.")]
        public string TemplateDisplayName;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify the ImagePreviewUrl of the template file that will be updated.")]
        public string TemplateImagePreviewUrl;

        [Parameter(Mandatory = false, HelpMessage = "It can be used to specify custom Properties for the template file that will be updated.")]
        public Hashtable TemplateProperties;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while extracting a template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.Url);
            bool templateFromFileSystem = !Path.ToLower().StartsWith("http");
            FileConnectorBase fileConnector;
            string templateFileName = System.IO.Path.GetFileName(Path);
            if (templateFromFileSystem)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                FileInfo fileInfo = new FileInfo(Path);
                fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");
            }
            else
            {                
                Uri fileUri = new Uri(Path);
                var webUrl = Microsoft.SharePoint.Client.Web.WebUrlFromFolderUrlDirect(ClientContext, fileUri);
                var templateContext = ClientContext.Clone(webUrl.ToString());

                string library = Path.ToLower().Replace(templateContext.Url.ToLower(), "").TrimStart('/');
                int idx = library.IndexOf("/");
                library = library.Substring(0, idx);
                fileConnector = new SharePointConnector(templateContext, templateContext.Url, library);
            }
            XMLTemplateProvider provider;
            ProvisioningTemplate provisioningTemplate;
            Stream stream = fileConnector.GetFileStream(templateFileName);
            var isOpenOfficeFile = FileUtilities.IsOpenOfficeFile(stream);
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
            provisioningTemplate = provider.GetTemplate(templateFileName, TemplateProviderExtensions);

            if (provisioningTemplate == null) return;

            GetProvisioningTemplate.SetTemplateMetadata(provisioningTemplate, TemplateDisplayName, TemplateImagePreviewUrl, TemplateProperties);

            provider.SaveAs(provisioningTemplate, templateFileName, TemplateProviderExtensions);
        }
    }
}
