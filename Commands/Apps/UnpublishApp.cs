#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Unpublish, "PnPApp")]
    [CmdletHelp("Unpublishes/retracts an available add-in from the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Unpublish-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = @"This will retract, but not remove, the specified app from the tenant app catalog",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Unpublish-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Site",
        Remarks = @"This will retract, but not remove, the specified app from the site collection app catalog",
        SortOrder = 2)]
    public class UnpublishApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            var additionalHeaders = GetAdditionalHeaders();
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext, additionalHeaders);

            var app = Identity.GetAppMetadata(ClientContext, Scope, additionalHeaders);
            if (app != null)
            {
                manager.Retract(app, Scope);
            }
            else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}
#endif