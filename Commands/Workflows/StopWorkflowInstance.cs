using System;
using System.Linq;
using System.Management.Automation;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Stop, "PnPWorkflowInstance")]
    [CmdletHelp("Stops a workflow instance",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Stop-PnPWorkflowInstance -Identity $wfInstance",
        Remarks = "Stops the workflow instance",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $wfInstances | Stop-PnPWorkflowInstance -Force",
        Remarks = "Terminates the workflow instance(s)",
        SortOrder = 2)]
    public class StopWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The instance to stop", Position = 0, ValueFromPipeline = true)]
        public WorkflowInstancePipeBind Identity;

        //Cancel vs Terminate: https://support.office.com/en-us/article/Cancel-a-workflow-in-progress-096b7d2d-9b8d-48f1-a002-e98bb86bdc7f
        [Parameter(Mandatory = false, HelpMessage = "Forcefully terminate the workflow instead of cancelling. Works on errored and non-responsive workflows. Does not notify participants.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var instanceService = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService();

            var instance = Identity.Instance
                ?? instanceService.GetInstance(Identity.Id);

            var instanceId = Identity.Instance?.Id ?? Identity.Id;
            if (Force)
            {
                WriteVerbose("Terminating workflow with ID: " + instanceId);
                instanceService.TerminateWorkflow(instance);
            }
            else
            {
                WriteVerbose("Cancelling workflow with ID: " + instanceId);
                instanceService.CancelWorkflow(instance);
            }

            ClientContext.ExecuteQueryRetryAsync();
        }
    }
}
