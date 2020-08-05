#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Enums;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Publish, "PnPApp")]
    [CmdletHelp("Publishes/Deploys/Trusts an available app in the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Publish-PnPApp -Identity 2646ccc3-6a2b-46ef-9273-81411cbbb60f", 
        Remarks = @"This will deploy/trust an app into the app catalog. Notice that the app needs to be available in the tenant scoped app catalog", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Publish-PnPApp -Identity 2646ccc3-6a2b-46ef-9273-81411cbbb60f -Scope Site",
        Remarks = @"This will deploy/trust an app into the app catalog. Notice that the app needs to be available in the site collection scoped app catalog", SortOrder = 1)]
    public class PublishApp : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the app")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false)]
        public SwitchParameter SkipFeatureDeployment;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            var app = Identity.GetAppMetadata(ClientContext, Scope);
            if (app != null)
            {
                manager.Deploy(app, SkipFeatureDeployment, Scope);
            } else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}
#endif
