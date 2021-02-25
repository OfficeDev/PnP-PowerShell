using System;
using System.Management.Automation;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Get, "PnPWorkflowInstance", DefaultParameterSetName = ParameterSet_BYSITE)]
    [CmdletHelp("Gets SharePoint 2010/2013 workflow instances",
        DetailedDescription = "Gets all SharePoint 2010/2013 workflow instances",
        Category = CmdletHelpCategory.Workflows,
        OutputType = typeof(WorkflowInstance),
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowInstance",
        Remarks = @"Retrieves workflow instances for site workflows",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $wfSubscriptions | Get-PnPWorkflowInstance",
        Remarks = @"Retrieves workflow instance(s) for specified subscription(s).",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowInstance ""ab77c32e-8b61-4fb4-bb41-be12193e9852""",
        Remarks = @"Retrieves workflow instance by workflow instance ID.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowInstance $listItem",
        Remarks = @"Retrieves workflow instances on the provided list item",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowInstance -List ""My Library"" -ListItem 2",
        Remarks = @"Retrieves workflow instances on the provided item with 2 in list ""My Library""",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> $listItems | Get-PnPWorkflowInstance",
        Remarks = @"Retrieves workflow instances on the provided items",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowInstance -WorkflowSubscription ""ab77c32e-8b61-4fb4-bb41-be12193e9852""",
        Remarks = @"Retrieves workflow instances by workflow subscription ID",
        SortOrder = 7)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowSubscription | Get-PnPWorkflowInstance",
        Remarks = @"Retrieves workflow instances from all subscriptions",
        SortOrder = 8)]

    public class GetWorkflowInstance : PnPWebCmdlet
    {
        private const string ParameterSet_BYSITE = "By Site";
        private const string ParameterSet_BYGUID = "By GUID";
        private const string ParameterSet_BYLISTITEM = "By ListItem";
        private const string ParameterSet_BYLISTITEMOBJECT = "By ListItem object";
        private const string ParameterSet_BYSUBSCRIPTION = "By WorkflowSubscription";
        private const string ParameterSet_BYSUBSCRIPTIONOBJECT = "By WorkflowSubscription object";

        protected override void ExecuteCmdlet()
        {
            switch (ParameterSetName)
            {
                case ParameterSet_BYSITE:
                    ExecuteCmdletBySite();
                    break;

                case ParameterSet_BYLISTITEM:
                case ParameterSet_BYLISTITEMOBJECT:
                    ExecuteCmdletByListItem();
                    break;
                case ParameterSet_BYSUBSCRIPTION:
                case ParameterSet_BYSUBSCRIPTIONOBJECT:
                    ExecuteCmdletBySubscription();
                    break;

                case ParameterSet_BYGUID:
                    ExecuteCmdletByIdentity();
                    break;
                default:
                    throw new NotImplementedException($"{nameof(ParameterSetName)}: {ParameterSetName}");
            }
        }

        private void ExecuteCmdletBySite()
        {
            var instances = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService()
                .EnumerateInstancesForSite();

            ClientContext.Load(instances);
            ClientContext.ExecuteQueryRetry();

            WriteObject(instances, true);
        }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYLISTITEM, HelpMessage = "The List for which workflow instances should be retrieved", Position = 0)]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYLISTITEM, HelpMessage = "The List Item for which workflow instances should be retrieved", Position = 1)]
        public ListItemPipeBind ListItem;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYLISTITEMOBJECT, HelpMessage = "The ListItem for which workflow instances should be retrieved", Position = 0)]
        public ListItem ListItemObject;

        private void ExecuteCmdletByListItem()
        {
            var list = ListItemObject?.ParentList
                ?? List.GetList(SelectedWeb)
                ?? throw new PSArgumentException($"No list found with id, title or url '{List}'", nameof(List));

            var listId = list.EnsureProperty(x => x.Id);

            var listItem = ListItemObject
                ?? ListItem.GetListItem(list)
                ?? throw new PSArgumentException($"No list item found with id, or title '{ListItem}'", nameof(ListItem));

            var listItemId = listItem.EnsureProperty(x => x.Id);

            var workflows = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService()
                .EnumerateInstancesForListItem(listId, listItemId);

            ClientContext.Load(workflows);
            ClientContext.ExecuteQueryRetry();
            WriteObject(workflows, true);
        }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYSUBSCRIPTION, HelpMessage = "The workflow subscription for which workflow instances should be retrieved", Position = 0)]
        public WorkflowSubscriptionPipeBind WorkflowSubscription;
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYSUBSCRIPTIONOBJECT, HelpMessage = "The workflow subscription for which workflow instances should be retrieved", Position = 0, ValueFromPipeline = true)]
        public WorkflowSubscription WorkflowSubscriptionObject;

        private void ExecuteCmdletBySubscription()
        {
            var workflowSubscription = WorkflowSubscriptionObject
                ?? WorkflowSubscription.GetWorkflowSubscription(SelectedWeb)
                ?? throw new PSArgumentException($"No workflow subscription found for '{WorkflowSubscription}'", nameof(WorkflowSubscription));

            var workflows = workflowSubscription.GetInstances();
            WriteObject(workflows, true);
        }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_BYGUID, HelpMessage = "The guid of the workflow instance to retrieved.", Position = 0, ValueFromPipeline = true, ValueFromRemainingArguments = true)]
        public WorkflowInstancePipeBind Identity;

        private void ExecuteCmdletByIdentity()
        {
            var workflowInstanceId = Identity.Instance?.EnsureProperty(i => i.Id)
                ?? Identity.Id;

            var instance = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService()
                .GetInstance(workflowInstanceId);

            ClientContext.Load(instance);
            ClientContext.ExecuteQueryRetry();

            WriteObject(instance, true);
        }
    }
}
