using System;
using System.Linq;
using System.Management.Automation;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Resume, "PnPWorkflowInstance")]
    [CmdletHelp("Resume a workflow",
        "Resumes a previously stopped workflow instance",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Resume-PnPWorkflowInstance ab77c32e-8b61-4fb4-bb41-be12193e9852",
        Remarks = "Resumes the workflow instance, this can be a instance ID (Guid) or the instance itself.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Resume-PnPWorkflowInstance -Identity ""ab77c32e-8b61-4fb4-bb41-be12193e9852""",
        Remarks = "Resumes the workflow instance, this can be a instance ID (Guid) or the instance itself.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> $wfInstances | Resume-PnPWorkflowInstance",
        Remarks = "Resumes the workflow instance(s), either instance IDs or the instance objects",
        SortOrder = 3)]
    public class ResumeWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The instance to resume", Position = 0, ValueFromPipeline = true)]
        public WorkflowInstancePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var workflowInstanceService = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService();

            var instance = Identity.Instance
                ?? workflowInstanceService.GetInstance(Identity.Id);

            workflowInstanceService.ResumeWorkflow(instance);

            ClientContext.ExecuteQueryRetry();
        }
    }
}
