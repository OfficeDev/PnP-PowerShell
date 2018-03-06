#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Publish, "PnPApp")]
    [CmdletHelp("Publishes/Deploys/Trusts an available app in the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Publish-PnPApp", Remarks = @"This will deploy/trust an app into the app catalog. Notice that the app needs to be available in the app catalog", SortOrder = 1)]
    
    public class PublishApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the app")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false)]
        public SwitchParameter SkipFeatureDeployment;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            manager.Deploy(Identity.Id,SkipFeatureDeployment);
        }
    }
}
#endif
