#if !ONPREMISES

using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Webhooks
{
    [Cmdlet(VerbsCommon.Remove, "PnPWebhookSubscription")]
    [CmdletHelp("Removes a Webhook subscription from the resource",
        Category = CmdletHelpCategory.Webhooks,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(WebhookSubscription))]
    [CmdletExample(
        Code = "PS:> Remove-PnPWebhookSubscription -List MyList -Identity ea1533a8-ff03-415b-a7b6-517ee50db8b6",
        Remarks = "Removes the Webhook subscription with the specified id from the list MyList",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> Remove-PnPWebhookSubscription -Identity $subscriptions[0] -List MyList",
        Remarks = "Removes the first Webhook subscription from the list MyList",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> $subscriptions[0] | Remove-PnPWebhookSubscription -List MyList",
        Remarks = "Removes the first Webhook subscription from the list MyList",
        SortOrder = 3)]
    public class RemoveWebhookSubscription : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The identity of the Webhook subscription to remove")]
        public WebhookSubscriptionPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The list object or name which the Webhook subscription will be removed from")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                // NOTE: Currently only supports List Webhooks
                if (MyInvocation.BoundParameters.ContainsKey("List"))
                {
                    // Get the list from the currently selected web
                    List list = List.GetList(SelectedWeb);
                    if (list != null)
                    {
                        // Ensure we have list Id (and Title for the confirm message)
                        list.EnsureProperties(l => l.Id, l => l.Title);

                        // Check the Force switch of ask confirm
                        if (Force
                            || ShouldContinue(string.Format(Properties.Resources.RemoveWebhookSubscription0From1_2,
                                Identity.Id, Properties.Resources.List, List.Title), Properties.Resources.Confirm))
                        {
                            // Remove the Webhook subscription for the specified Id
                            list.RemoveWebhookSubscription(Identity.Subscription);
                        }

                    }
                }
                else
                {
                    throw new PSNotImplementedException("This Cmdlet only supports List Webhooks currently");
                }
            }
        }

    }
}

#endif