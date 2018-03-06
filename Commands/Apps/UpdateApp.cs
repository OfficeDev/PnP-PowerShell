#if !ONPREMISES
using OfficeDevPnP.Core.ALM;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Update, "PnPApp")]
    [CmdletHelp("Updates an available app from the app catalog",
        Category = CmdletHelpCategory.Apps, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Update-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will update an already installed app if a new version is available. Retrieve a list all available apps and the installed and available versions with Get-PnPApp", SortOrder = 1)]
    public class UpdateApp : PnPCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id or an actual app metadata instance")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new AppManager(ClientContext);
            manager.Upgrade(Identity.Id);
        }
    }
}
#endif