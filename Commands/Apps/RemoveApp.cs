#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Enums;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Remove, "PnPApp")]
    [CmdletHelp("Removes an app from the app catalog", SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        Category = CmdletHelpCategory.Apps)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = @"This will remove the specified app from the tenant scoped app catalog", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Site",
        Remarks = @"This will remove the specified app from the site collection scoped app catalog", SortOrder = 2)]
    public class RemoveApp : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            var app = Identity.GetAppMetadata(ClientContext, Scope);

            if (app != null)
            {
                manager.Remove(app, Scope);
            }
            else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}
#endif