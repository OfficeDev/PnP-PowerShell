#if !ONPREMISES

using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Webhooks
{
    [Cmdlet(VerbsCommon.Set, "PnPWebhookSubscription")]
    [CmdletHelp("Updates a Webhook subscription",
        Category = CmdletHelpCategory.Webhooks,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(WebhookSubscription))]
    [CmdletExample(
        Code = "PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook",
        Remarks = "Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate ""2017-09-01""",
        Remarks = "Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url and a new expiration date",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> $updated = $subscriptions[0]
PS:> $updated.ExpirationDate = ""2017-10-01""
PS:> Set-PnPWebhookSubscription -List MyList -Subscription $updated",
        Remarks = @"Updates the Webhook subscription from the list MyList with a modified subscription object.
Note: The date will be converted to Universal Time",
        SortOrder = 3)]
    public class SetWebhookSubscription : PnPWebCmdlet
    {
        public const int DefaultValidityInMonths = 6;
        public const int ValidityDeltaInDays = -72; // Note: Some expiration dates too close to the limit are rejected
        
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The identity of the Webhook subscription to update")]
        public WebhookSubscriptionPipeBind Subscription;

        [Parameter(Mandatory = false, HelpMessage = "The list object or name from which the Webhook subscription will be modified")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the Webhook endpoint that will be notified of the change")]
        public string NotificationUrl;

        [Parameter(Mandatory = false, HelpMessage = "The date at which the Webhook subscription will expire. (Default: 6 months from today)")]
        public DateTime ExpirationDate = DateTime.Today.ToUniversalTime().AddMonths(DefaultValidityInMonths).AddHours(ValidityDeltaInDays);

        protected override void ExecuteCmdlet()
        {
            if (Subscription != null)
            {
                // NOTE: Currently only supports List Webhooks
                if (MyInvocation.BoundParameters.ContainsKey("List"))
                {
                    // Get the list from the currently selected web
                    List list = List.GetList(SelectedWeb);
                    if (list != null)
                    {
                        // Ensure we have list Id (TODO Should be changed in the Core extension method)
                        list.EnsureProperty(l => l.Id);

                        // If the notification Url is specified, override the property of the subscription object
                        if (MyInvocation.BoundParameters.ContainsKey(nameof(NotificationUrl)))
                        {
                            Subscription.Subscription.NotificationUrl = NotificationUrl;
                        }
                        // If the expiration date is specified, override the property of the subscription object
                        if (MyInvocation.BoundParameters.ContainsKey(nameof(ExpirationDate)))
                        {
                            Subscription.Subscription.ExpirationDateTime = ExpirationDate;
                        }

                        // Write the result object (A flag indicating success)
                        WriteObject(list.UpdateWebhookSubscription(Subscription.Subscription));
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