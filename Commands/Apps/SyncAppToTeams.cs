#if !ONPREMISES
using OfficeDevPnP.Core.Enums;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Sync, "PnPAppToTeams")]
    [CmdletHelp("Synchronize an app from the tenant app catalog to the Microsoft Teams app catalog", SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Apps)]
    [CmdletExample(
        Code = @"PS:> Sync-PnPAppToTeams -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = @"This will synchronize the given app with the Microsoft Teams app catalog", SortOrder = 1)]
    public class SyncAppToTeams : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            var app = Identity.GetAppMetadata(ClientContext, AppCatalogScope.Tenant);

            if (app != null)
            {
                manager.SyncToTeams(app);
            }
            else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}
#endif