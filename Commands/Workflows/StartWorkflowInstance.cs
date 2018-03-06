using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client.Workflow;
using Microsoft.SharePoint.Client.WorkflowServices;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Start, "PnPWorkflowInstance")]
    [CmdletHelp("Starts a workflow instance on a list item",
        Category = CmdletHelpCategory.Workflows)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Name 'WorkflowName' -ListItem $item ", 
        Remarks = "Starts a workflow instance on the specified list item",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Name 'WorkflowName' -ListItem 2 ",
        Remarks = "Starts a workflow instance on the specified list item",
        SortOrder = 2)]
    public class StartWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The workflow subscription to start", Position = 0)]
        public WorkflowSubscriptionPipeBind Subscription;

        [Parameter(Mandatory = true, HelpMessage = "The list item to start the workflow against", Position = 1)]
        public ListItemPipeBind ListItem;

        protected override void ExecuteCmdlet()
        {
            int ListItemID;
            if (ListItem != null)
            {
                if (ListItem.Id != uint.MinValue)
                {
                    ListItemID = (int)ListItem.Id;
                }
                else if (ListItem.Item != null)
                {
                    ListItemID = ListItem.Item.Id;
                }
                else
                {
                    throw new PSArgumentException("No valid list item specified.");
                }
            }
            else
            {
                throw new PSArgumentException("List Item is required");
            }
            WorkflowServicesManager workflowServicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
            WorkflowInstanceService instanceService = workflowServicesManager.GetWorkflowInstanceService();

            if (Subscription.Subscription != null)
            {
                var inputParameters = new Dictionary<string, object>();
                instanceService.StartWorkflowOnListItem(Subscription.Subscription, ListItemID, inputParameters);
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
