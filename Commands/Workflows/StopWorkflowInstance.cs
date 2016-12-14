using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Stop, "PnPWorkflowInstance")]
    [CmdletAlias("Stop-SPOWorkflowInstance")]
    [CmdletHelp("Stops a workflow instance",
        Category = CmdletHelpCategory.Workflows)]

    public class StopWorkflowInstance : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The instance to stop", Position = 0)]
        public WorkflowInstancePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity.Instance != null)
            {
                Identity.Instance.CancelWorkFlow();
            }
            else if (Identity.Id != Guid.Empty)
            {
                var allinstances = SelectedWeb.GetWorkflowInstances();
                foreach (var instance in allinstances.Where(instance => instance.Id == Identity.Id))
                {
                    instance.CancelWorkFlow();
                    break;
                }
            }
        }
    }


}
