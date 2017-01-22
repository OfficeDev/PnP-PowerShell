using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Remove, "PnPWorkflowSubscription")]
    [CmdletAlias("Remove-SPOWorkflowSubscription")]
    [CmdletHelp("Removes a workflow subscription",
        Category = CmdletHelpCategory.Workflows)]

    public class RemoveWorkflowSubscription : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The subscription to remove", Position = 0)]
        public WorkflowSubscriptionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity.Subscription != null)
            {
                Identity.Subscription.Delete();
            }
            else if (Identity.Id != Guid.Empty)
            {
                var subscription = SelectedWeb.GetWorkflowSubscription(Identity.Id);
                if (subscription != null)
                    subscription.Delete();
            }
            else if (!string.IsNullOrEmpty(Identity.Name))
            {
                var subscription = SelectedWeb.GetWorkflowSubscription(Identity.Name);
                if (subscription != null)
                    subscription.Delete();
            }
        }
    }

}
