#if !SP2013 && !SP2016
using OfficeDevPnP.Core.ALM;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Add, "PnPApp")]
    [CmdletHelp("Add/uploads an available app to the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        OutputType = typeof(AppMetadata))]
    [CmdletExample(
        Code = @"PS:> Add-PnPApp -Path ./myapp.sppkg",
        Remarks = @"This will upload the specified app package to the app catalog", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPApp -Path ./myapp.sppkg -Publish",
        Remarks = @"This will upload the specified app package to the app catalog and deploy/trust it at the same time.", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPApp -Path ./myapp.sppkg -Scope Site -Publish",
        Remarks = @"This will upload the specified app package to the site collection app catalog and deploy/trust it at the same time.", SortOrder = 2)]
    public class AddApp : PnPCmdlet
    {
        private const string ParameterSet_ADD = "Add only";
        private const string ParameterSet_PUBLISH = "Add and Publish";

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_ADD, ValueFromPipeline = true, HelpMessage = "Specifies the Id or an actual app metadata instance")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_PUBLISH, ValueFromPipeline = true, HelpMessage = "Specifies the Id or an actual app metadata instance")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_PUBLISH, HelpMessage = "This will deploy/trust an app into the app catalog")]
        public SwitchParameter Publish;

        [Parameter(Mandatory = false, ValueFromPipeline = false, ParameterSetName = ParameterSet_PUBLISH)]
        public SwitchParameter SkipFeatureDeployment;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the existing app package if it already exists")]
        public SwitchParameter Overwrite;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the timeout in seconds. Defaults to 200.")]
        public int Timeout = 200;

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            var fileInfo = new System.IO.FileInfo(Path);

            var bytes = System.IO.File.ReadAllBytes(Path);

            var manager = new AppManager(ClientContext, GetAdditionalHeaders());

            var result = manager.Add(bytes, fileInfo.Name, Overwrite, Scope, timeoutSeconds: Timeout);

            try
            {

                if (Publish)
                {
                    if (manager.Deploy(result, SkipFeatureDeployment, Scope))
                    {
                        result = manager.GetAvailable(result.Id, Scope);
                    }

                }
                WriteObject(result);
            }
            catch
            {
                // Exception occurred rolling back
                manager.Remove(result, Scope);
                throw;
            }
        }
    }
}
#endif
