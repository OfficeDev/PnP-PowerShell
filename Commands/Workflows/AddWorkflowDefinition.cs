using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Add, "PnPWorkflowDefinition")]
    [CmdletAlias("Add-SPOWorkflowDefinition")]
    [CmdletHelp("Adds a workflow definition",
        Category = CmdletHelpCategory.Workflows,
        OutputType=typeof(Guid),
        OutputTypeDescription = "Returns the Id of the workflow definition")]
    [CmdletExample(
        Code = @"Add-PnPWorkflowDefinition -Definition $wfdef", 
        Remarks = "Adds an existing workflow definition, retrieved by Get-PnPWorkflowDefinition, to a site.",
        SortOrder = 1)]
    public class AddWorkflowDefinition : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The workflow definition to add.")]
        public WorkflowDefinition Definition;

        [Parameter(Mandatory = false, HelpMessage = "Overrides the default behaviour, which is to publish workflow definitions.")]
        public SwitchParameter DoNotPublish;
        protected override void ExecuteCmdlet()
        {
            WriteObject(SelectedWeb.AddWorkflowDefinition(Definition,!DoNotPublish));
        }
    }

}
