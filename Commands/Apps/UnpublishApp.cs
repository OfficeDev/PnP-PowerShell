#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Unpublish, "PnPApp")]
    [CmdletHelp("Unpublishes/retracts an available add-in from the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Unpublish-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will retract, but not remove, the specified app from the app catalog", SortOrder = 2)]
    public class UnpublishApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            manager.Retract(Identity.Id);
        }
    }
}
#endif