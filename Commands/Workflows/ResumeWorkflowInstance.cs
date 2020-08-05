using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Resume, "PnPWorkflowInstance")]
    [CmdletHelp("Resume a workflow",
        "Resumes a previously stopped workflow instance",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Resume-PnPWorkflowInstance -identity $wfInstance", 
        Remarks = "Resumes the workflow instance, this can be the Guid of the instance or the instance itself.",
        SortOrder = 1)]
    public class ResumeWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The instance to resume", Position = 0)]
        public WorkflowInstancePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity.Instance != null)
            {
                Identity.Instance.ResumeWorkflow();
            }
            else if (Identity.Id != Guid.Empty)
            {
                var allinstances = SelectedWeb.GetWorkflowInstances();
                foreach (var instance in allinstances.Where(instance => instance.Id == Identity.Id))
                {
                    instance.ResumeWorkflow();
                    break;
                }
            }
        }
    }


}
