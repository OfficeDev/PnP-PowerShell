#if !SP2013 && !SP2016
using OfficeDevPnP.Core.ALM;
using OfficeDevPnP.Core.Enums;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;
using System.Threading;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Install, "PnPApp")]
    [CmdletHelp("Installs an available app from the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Install-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = @"This will install an app that is available in the tenant scoped app catalog, specified by the id, to the current site.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Install-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Site",
        Remarks = @"This will install an app that is available in the site collection scoped app catalog, specified by the id, to the current site.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe | Install-PnPApp",
        Remarks = @"This will install the given app from the tenant scoped app catalog into the site.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Site | Install-PnPApp",
        Remarks = @"This will install the given app from the site collection scoped app catalog into the site.",
        SortOrder = 4)]
    public class InstallApp : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id or an actual app metadata instance")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        [Parameter(Mandatory = false, HelpMessage = "If specified the execution will pause until the app has been installed in the site.")]
        public SwitchParameter Wait;

        protected override void ExecuteCmdlet()
        {
            var manager = new AppManager(ClientContext);

            var app = Identity.GetAppMetadata(ClientContext, Scope);
            if (app != null)
            {
                manager.Install(app, Scope);
                if(Wait.IsPresent)
                {
                    var installableApp = manager.GetAvailable(app.Id, Scope);
                    while (installableApp.InstalledVersion == null)
                    {
                        Thread.Sleep(1000); // wait a second
                        installableApp = manager.GetAvailable(app.Id, Scope);
                    }
                }
            }
            else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}
#endif