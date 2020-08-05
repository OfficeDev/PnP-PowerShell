using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client.WorkflowServices;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Start, "PnPWorkflowInstance")]
    [CmdletHelp("Starts a SharePoint 2010/2013 workflow instance on a list item",
        DetailedDescription = "Allows starting a SharePoint 2010/2013 workflow on a list item in a list",
        Category = CmdletHelpCategory.Workflows,
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Subscription 'WorkflowName' -ListItem $item",
        Remarks = "Starts a workflow instance on the specified list item",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Subscription $subscription -ListItem 2",
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
                    throw new PSArgumentException("No valid list item specified.", nameof(ListItem));
                }
            }
            else
            {
                throw new PSArgumentException("List Item is required", nameof(ListItem));
            }

            var subscription = Subscription.GetWorkflowSubscription(SelectedWeb)
                ?? throw new PSArgumentException($"No workflow subscription found for '{Subscription}'", nameof(Subscription));

            var inputParameters = new Dictionary<string, object>();

            WorkflowServicesManager workflowServicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
            WorkflowInstanceService instanceService = workflowServicesManager.GetWorkflowInstanceService();

            instanceService.StartWorkflowOnListItem(subscription, ListItemID, inputParameters);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
