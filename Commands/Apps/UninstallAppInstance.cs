using Microsoft.SharePoint.Client;
using System.Management.Automation;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsLifecycle.Uninstall, "PnPAppInstance", SupportsShouldProcess = true)]
    [CmdletAlias("Uninstall-SPOAppInstance")]
    [CmdletHelp("Removes an app from a site", Category = CmdletHelpCategory.Apps)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity $appinstance", Remarks = "Uninstalls the app instance which was retrieved with the command Get-PnPAppInstance", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = "Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe'", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -force", Remarks = "Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe' and do not ask for confirmation", SortOrder = 3)]
    public class UninstallAppInstance : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Appinstance or Id of the addin to remove.")]
        public AppPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            AppInstance instance;

            if (Identity.Instance != null)
            {
                instance = Identity.Instance;
            }
            else
            {
                instance = SelectedWeb.GetAppInstanceById(Identity.Id);
            }

            if(instance != null)
            {
                if(!instance.IsObjectPropertyInstantiated("Title"))
                {
                    ClientContext.Load(instance, i => i.Title);
                    ClientContext.ExecuteQueryRetry();
                }
                if (Force || ShouldContinue(string.Format(Properties.Resources.UninstallApp0, instance.Title), Properties.Resources.Confirm))
                {
                    instance.Uninstall();
                    ClientContext.ExecuteQueryRetry();
                }
            }

        }


    }
}
