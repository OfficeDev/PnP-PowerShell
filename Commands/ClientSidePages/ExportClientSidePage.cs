#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Properties;
using System.IO;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsData.Export, "PnPClientSidePage", SupportsShouldProcess = true)]
    [CmdletHelp("Exports a Client Side Page to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Export-PnPClientSidePage -Identity Home.aspx ",
       Remarks = "Exports the page 'Home.aspx' to a new PnP Provisioning Template",
       SortOrder = 1)]
    public class ExportClientSidePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If specified referenced files will be exported to the current folder.")]
        public SwitchParameter PersistBrandingFiles;

        [Parameter(Mandatory = false, HelpMessage = "If specified the template will be saved to the file specified with this parameter.")]
        public string Out;

        [Parameter(Mandatory = false, HelpMessage = "Specify to override the question to overwrite a file if it already exists.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            if (!string.IsNullOrEmpty(Out))
            {
                if (!Path.IsPathRooted(Out))
                {
                    Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
                }
                if (System.IO.File.Exists(Out))
                {
                    if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, Out), Resources.Confirm))
                    {
                        ExtractTemplate(new FileInfo(Out).DirectoryName, new FileInfo(Out).Name);
                    }
                }
                else
                {
                    ExtractTemplate(new FileInfo(Out).DirectoryName, new FileInfo(Out).Name);
                }
            }
            else
            {
                ExtractTemplate(null, null);
            }
        }


        private void ExtractTemplate(string dirName, string fileName)
        {
            var outputTemplate = new ProvisioningTemplate();

            var helper = new OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.Utilities.ClientSidePageContentsHelper();

            var ci = new OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.ProvisioningTemplateCreationInformation(SelectedWeb);
            ci.PersistBrandingFiles = PersistBrandingFiles;
            if (!string.IsNullOrEmpty(dirName))
            {
                var fileSystemConnector = new FileSystemConnector(dirName, "");
                ci.FileConnector = fileSystemConnector;
            }
            helper.ExtractClientSidePage(SelectedWeb, outputTemplate, ci, new OfficeDevPnP.Core.Diagnostics.PnPMonitoredScope(), null, Identity.Name, false);

            if (!string.IsNullOrEmpty(fileName))
            {
                System.IO.File.WriteAllText(Path.Combine(dirName, fileName), outputTemplate.ToXML());
            }
            else
            {
                WriteObject(outputTemplate.ToXML());
            }
        }
    }

}
#endif