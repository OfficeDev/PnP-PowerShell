using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Remove, "PnPWorkflowDefinition")]
    [CmdletHelp("Removes a workflow definition",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPWorkflowDefinition -Identity $wfDef", 
        Remarks = "Removes the workflow, retrieved by Get-PnPWorkflowDefinition, from the site.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowDefinition -Name MyWorkflow | Remove-PnPWorkflowDefinition", 
        Remarks = "Get the workflow MyWorkFlow and remove from the site.",
        SortOrder = 2)]
    public class RemoveWorkflowDefinition : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The definition to remove", Position = 0)]
        public WorkflowDefinitionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity.Definition != null)
            {
                Identity.Definition.Delete();
            }
            else if (Identity.Id != Guid.Empty)
            {
                var definition = SelectedWeb.GetWorkflowDefinition(Identity.Id);
                if (definition != null)
                    definition.Delete();
            }
            else if (!string.IsNullOrEmpty(Identity.Name))
            {
                var definition = SelectedWeb.GetWorkflowDefinition(Identity.Name);
                if (definition != null)
                    definition.Delete();
            }
        }
    }

}
