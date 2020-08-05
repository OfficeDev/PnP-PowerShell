using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Get, "PnPWorkflowSubscription")]
    [CmdletHelp("Return a workflow subscription",
        "Returns a workflow subscriptions from a list",
        Category = CmdletHelpCategory.Workflows,
        OutputType = typeof(WorkflowSubscription),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowsubscription.aspx"
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowSubscription -Name MyWorkflow", 
        Remarks = @"Gets an Workflow subscription with the name ""MyWorkflow"".",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowSubscription -Name MyWorkflow -list $list", 
        Remarks = @"Gets an Workflow subscription with the name ""MyWorkflow"" from the list $list.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPList -identity ""MyList"" | Get-PnPWorkflowSubscription -Name MyWorkflow", 
        Remarks = @"Gets an Workflow subscription with the name ""MyWorkflow"" from the list ""MyList"".",
        SortOrder = 3)]
    public class GetWorkflowSubscription : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The name of the workflow", Position = 0)]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "A list to search the association for", Position = 1)]
        public ListPipeBind List;
        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                if (list == null)
                    throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

                if (string.IsNullOrEmpty(Name))
                {
                    var servicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
                    var subscriptionService = servicesManager.GetWorkflowSubscriptionService();
                    var subscriptions = subscriptionService.EnumerateSubscriptionsByList(list.Id);

                    ClientContext.Load(subscriptions);

                    ClientContext.ExecuteQueryRetry();
                    WriteObject(subscriptions, true);
                }
                else
                {
                    WriteObject(list.GetWorkflowSubscription(Name));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                {
                    var servicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
                    var subscriptionService = servicesManager.GetWorkflowSubscriptionService();
                    var subscriptions = subscriptionService.EnumerateSubscriptions();

                    ClientContext.Load(subscriptions);

                    ClientContext.ExecuteQueryRetry();
                    WriteObject(subscriptions, true);
                }
                else
                {
                    WriteObject(SelectedWeb.GetWorkflowSubscription(Name));
                }
            }
        }
    }

}
