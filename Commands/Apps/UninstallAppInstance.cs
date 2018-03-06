using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Apps
{
#if !ONPREMISES
    [Obsolete("Use Uninstall-PnPApp instead")]
#endif
    [Cmdlet(VerbsLifecycle.Uninstall, "PnPAppInstance", SupportsShouldProcess = true)]
    [CmdletHelp("Removes an app from a site",
        "Removes an add-in/app that has been installed to a site.",
        Category = CmdletHelpCategory.Apps)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity $appinstance", Remarks = "Uninstalls the app instance which was retrieved with the command Get-PnPAppInstance", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = "Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe'", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -force", Remarks = "Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe' and do not ask for confirmation", SortOrder = 3)]
    public class UninstallAppInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Appinstance or Id of the addin to remove.")]
        public AppPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            AppInstance instance;

            instance = Identity.GetAppInstance(SelectedWeb);

            if (instance != null)
            {
                if (!instance.IsObjectPropertyInstantiated("Title"))
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
