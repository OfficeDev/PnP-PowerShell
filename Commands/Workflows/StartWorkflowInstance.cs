using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsLifecycle.Start, "PnPWorkflowInstance")]
    [CmdletHelp("Starts a SharePoint 2010/2013 workflow instance on a list item",
        DetailedDescription = "Allows starting a SharePoint 2010/2013 workflow on a list item in a list",
        OutputType = typeof(Guid),
        OutputTypeDescription = "Returns the GUID of the new workflow instance",
        Category = CmdletHelpCategory.Workflows,
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> $wfSubscriptions | Start-PnPWorkflowInstance",
        Remarks = "Starts a workflow instance of the specified subscription(s)",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPListItem -List MyList | Start-PnPWorkflowInstance -Subscription MyWorkflow",
        Remarks = "Starts a MyWorkflow instance on each item in the MyList list",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Subscription 'WorkflowName' -ListItem $item",
        Remarks = "Starts a workflow instance on the specified list item",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance -Subscription $subscription -ListItem 2",
        Remarks = "Starts a workflow instance on the specified list item",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance ""MyWorkflow"" -Initiator 5",
        Remarks = "Starts a workflow instance as a specified user ID",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance ""MyWorkflow"" -Initiator ""Jenny.Smith@contoso.com""",
        Remarks = "Starts a workflow instance as a specified user",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Start-PnPWorkflowInstance ""MyWorkflow"" -InitiationParameters @{ MyParameter = ""Hello""; SecondParameter = 100 }",
        Remarks = "Starts a workflow instance specifiying initiation parameters",
        SortOrder = 7)]
    public class StartWorkflowInstance : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The workflow subscription to start", Position = 0, ValueFromPipeline = true)]
        public WorkflowSubscriptionPipeBind Subscription;

        [Parameter(Mandatory = false, HelpMessage = "The list item or list item id to start the workflow on", Position = 1)]
        public ListItemPipeBind ListItem;

        [Parameter(Mandatory = false, HelpMessage = "The list item to start the workflow on", ValueFromPipeline = true)]
        public ListItem ListItemObject;

        [Parameter(Mandatory = false, HelpMessage = "The user who initiated the workflow")]
        public UserPipeBind Initiator;

        [Parameter(Mandatory = false, HelpMessage = "Initiation properties are external variables whose values are set when the workflow is initiated.")]
        public Hashtable InitiationParameters;

        protected override void ExecuteCmdlet()
        {
            var subscription = Subscription.GetWorkflowSubscription(SelectedWeb)
                ?? throw new PSArgumentException($"No workflow subscription found for '{Subscription}'", nameof(Subscription));

            // not much info available about theses
            // https://docs.microsoft.com/en-us/sharepoint/dev/general-development/workflow-initiation-and-configuration-properties#initiation-properties
            var inputParameters = InitiationParameters?.Cast<DictionaryEntry>()?.ToDictionary(e => e.Key.ToString(), e => e.Value)
                ?? new Dictionary<string, object>();

            if (Initiator is object)
            {
                var initiatorUser = Initiator.User is object ? Initiator.User
                                  : Initiator.Login is object ? SelectedWeb.EnsureUser(Initiator.Login) // this is really any string the user enters
                                                              : SelectedWeb.SiteUsers.GetById(Initiator.Id);

                var initiatorLoginName = initiatorUser.EnsureProperty(u => u.LoginName);

                // yes it's called InitiatorUserId but it actually wants a login name (claims format)
                // maybe 2010 workflows are different?
                inputParameters.Add("Microsoft.SharePoint.ActivationProperties.InitiatorUserId", initiatorLoginName);
            }

            var instanceService = new WorkflowServicesManager(ClientContext, SelectedWeb)
                .GetWorkflowInstanceService();

            ClientResult<Guid> instanceResult;

            if (ListItem is object)
            {
                int listItemID = ListItem?.Item?.EnsureProperty(li => li.Id) ?? (int)ListItem?.Id;
                instanceResult = instanceService.StartWorkflowOnListItem(subscription, listItemID, inputParameters);
            }
            else if (ListItemObject is object)
            {
                int listItemID = ListItemObject.EnsureProperty(li => li.Id);
                instanceResult = instanceService.StartWorkflowOnListItem(subscription, listItemID, inputParameters);
            }
            else
            {
                instanceResult = instanceService.StartWorkflow(subscription, inputParameters);
            }

            ClientContext.ExecuteQueryRetry();

            WriteObject(instanceResult.Value, true);
        }
    }
}
