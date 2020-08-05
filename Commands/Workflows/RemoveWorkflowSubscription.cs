using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Remove, "PnPWorkflowSubscription")]
    [CmdletHelp("Removes a SharePoint 2010/2013 workflow subscription",
        DetailedDescription = "Removes a previously registered SharePoint 2010/2013 workflow subscription",
        Category = CmdletHelpCategory.Workflows,
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPWorkflowSubscription -Identity $wfSub", 
        Remarks = "Removes the workflowsubscription, retrieved by Get-PnPWorkflowSubscription.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWorkflowSubscription -Name MyWorkflow | Remove-PnPWorkflowSubscription", 
        Remarks = "Get the workflowSubscription MyWorkFlow and remove it.",
        SortOrder = 2)]
    public class RemoveWorkflowSubscription : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The subscription to remove", Position = 0, ValueFromPipeline = true)]
        public WorkflowSubscriptionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var identity = Identity.GetWorkflowSubscription(SelectedWeb)
                ?? throw new PSArgumentException($"No workflow subscription found for '{Identity}'", nameof(Identity));
            identity.Delete();
        }
    }

}
