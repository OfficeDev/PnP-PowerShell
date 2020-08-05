using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Add, "PnPWorkflowSubscription")]
    [CmdletHelp("Adds a workflow subscription to a list",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf -list $list", 
        Remarks = "Adds an Workflow with the name 'SendMessageWf' to the list $list.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $list | Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf", 
        Remarks = @"Adds an Workflow with the name ""SendMessageWf"" to the list $list.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPList -Identity ""MyCustomList"" | Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf", 
        Remarks = @"Adds an Workflow with the name ""SendMessageWf"" to the list ""MyCustomList"".",
        SortOrder = 3)]
    public class AddWorkflowSubscription : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the subscription")]
        public string Name;

        [Parameter(Mandatory = true, HelpMessage = "The name of the workflow definition")]
        public string DefinitionName;

        [Parameter(Mandatory = true, HelpMessage = "The list to add the workflow to")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Switch if the workflow should be started manually, default value is 'true'")]
        public SwitchParameter StartManually = true;

        [Parameter(Mandatory = false, HelpMessage = "Should the workflow run when an new item is created")]
        public SwitchParameter StartOnCreated;
        
        [Parameter(Mandatory = false, HelpMessage = "Should the workflow run when an item is changed")]
        public SwitchParameter StartOnChanged;

        [Parameter(Mandatory = true, HelpMessage = "The name of the History list")]
        public string HistoryListName;

        [Parameter(Mandatory = true, HelpMessage = "The name of the task list")]
        public string TaskListName;

        [Parameter(Mandatory = false)]
        public Dictionary<string,string> AssociationValues;
        
        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);

            list.AddWorkflowSubscription(DefinitionName,Name,StartManually,StartOnCreated,StartOnChanged,HistoryListName,TaskListName, AssociationValues);
        }
    }

}
