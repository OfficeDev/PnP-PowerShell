using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Get, "PnPWorkflowSubscription")]
    [CmdletAlias("Get-SPOWorkflowSubscription")]
    [CmdletHelp("Returns a workflow subscriptions from a list",
        Category = CmdletHelpCategory.Workflows,
        OutputType=typeof(WorkflowSubscription),
        OutputTypeLink= "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowsubscription.aspx"
        )]

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
