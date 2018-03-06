#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Remove, "PnPApp")]
    [CmdletHelp("Removes an app from the app catalog", SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Apps)]
    [CmdletExample(Code = @"PS:> Remove-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will remove the specified app from the app catalog", SortOrder = 1)]
    public class RemoveApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the Addin Instance")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new OfficeDevPnP.Core.ALM.AppManager(ClientContext);

            manager.Remove(Identity.Id);
        }
    }
}
#endif