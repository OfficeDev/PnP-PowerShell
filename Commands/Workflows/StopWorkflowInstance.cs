using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client.Workflow;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Stop, "PnPWorkflowInstance")]
    [CmdletHelp("Stops a workflow instance",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Stop-PnPWorkflowInstance -identity $wfInstance", 
        Remarks = "Stops the workflow Instance",
        SortOrder = 1)]
    public class StopWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The instance to stop", Position = 0)]
        public WorkflowInstancePipeBind Identity;
    
        //Cancel vs Terminate: https://support.office.com/en-us/article/Cancel-a-workflow-in-progress-096b7d2d-9b8d-48f1-a002-e98bb86bdc7f
        [Parameter(Mandatory = false, HelpMessage = "Forcefully terminate the workflow instead of cancelling. Works on errored and non-responsive workflows. Deletes all created tasks. Does not notify participants.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {

            WorkflowServicesManager workflowServicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
            InteropService interopService = workflowServicesManager.GetWorkflowInteropService();
            WorkflowInstanceService instanceService = workflowServicesManager.GetWorkflowInstanceService();

            if (Identity.Instance != null)
            {
                WriteVerbose("Instance object set");
                WriteVerbose("Cancelling workflow with ID: " + Identity.Instance.Id);
                if(Force)
                {
                    instanceService.TerminateWorkflow(Identity.Instance);
                }
                else
                {
                    Identity.Instance.CancelWorkFlow();
                }
                ClientContext.ExecuteQueryRetry();
            }
            else if (Identity.Id != Guid.Empty)
            {
                WriteVerbose("Instance object not set. Looking up site workflows by GUID: " + Identity.Id);
                var allinstances = SelectedWeb.GetWorkflowInstances();
                foreach (var instance in allinstances.Where(instance => instance.Id == Identity.Id))
                {
                    WriteVerbose("Cancelling workflow with ID: " + Identity.Instance.Id);
                    if (Force)
                    {
                        instanceService.TerminateWorkflow(instance);
                    }
                    else
                    {
                        instance.CancelWorkFlow();
                    }
                    ClientContext.ExecuteQueryRetry();
                    break;
                }
            }
        }
    }
}
