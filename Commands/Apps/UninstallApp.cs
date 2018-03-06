#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Uninstall, "PnPApp")]
    [CmdletHelp("Uninstalls an available add-in from the site",
        Category = CmdletHelpCategory.Apps)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will uninstall the specified app from the current site.", SortOrder = 2)]
    public class UninstallApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            manager.Uninstall(Identity.Id);
        }
    }
}
#endif