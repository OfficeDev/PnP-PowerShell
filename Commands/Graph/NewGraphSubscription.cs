#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.New, "PnPGraphSubscription")]
    [CmdletHelp("Creates a new Microsof Graph Subscription which allows your webhook API to be called when a change occurs in Microsoft Graph",
        DetailedDescription = "Creates a new Microsof Graph Subscription. The required Azure Active Directory application permission depends on the resource creating the subscription for, see https://docs.microsoft.com/graph/api/subscription-post-subscriptions#permissions. For a sample ASP.NET WebApi webhook implementation to receive the notifications from Microsoft Graph, see https://github.com/microsoftgraph/msgraph-training-changenotifications/blob/b8d21ca7aa5feeece336287c9a781e71b7ba01c6/demos/01-create-application/Controllers/NotificationsController.cs#L51.",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/subscription-post-subscriptions",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> New-PnPGraphSubscription -ChangeType Create -NotificationUrl https://mywebapiservice/notifications -Resource \"me/mailFolders('Inbox')/messages\" -ExpirationDateTime (Get-Date).AddDays(1) -ClientState [Guid]::NewGuid().ToString()",
       Remarks = "Creates a new Microsoft Graph subscription listening for incoming mail during the next 24 hours in the inbox of the user under which the connection has been made and will signal the URL provided through NotificationUrl when a message comes in",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> New-PnPGraphSubscription -ChangeType Updates -NotificationUrl https://mywebapiservice/notifications -Resource \"Users\" -ExpirationDateTime (Get-Date).AddHours(1) -ClientState [Guid]::NewGuid().ToString()",
       Remarks = "Creates a new Microsoft Graph subscription listening for changes to user objects during the next hour and will signal the URL provided through NotificationUrl when a change has been made",
       SortOrder = 2)]
    // Deliberately omitting the CmdletMicrosoftGraphApiPermission attribute as permissions vary largely by the subscription type being used
    public class NewGraphSubscription : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The event(s) the subscription should trigger on")]
        public OfficeDevPnP.Core.Enums.GraphSubscriptionChangeType ChangeType;

        [Parameter(Mandatory = true, HelpMessage = "The URL that should be called when an event matching this subscription occurs")]
        public String NotificationUrl;

        [Parameter(Mandatory = true, HelpMessage = "The resource to monitor for changes. See https://docs.microsoft.com/graph/api/subscription-post-subscriptions#resources-examples for the list with supported options.")]
        public String Resource;

        [Parameter(Mandatory = false, HelpMessage = "The datetime defining how long this subscription should stay alive before which it needs to get extended to stay alive. See https://docs.microsoft.com/graph/api/resources/subscription#maximum-length-of-subscription-per-resource-type for the supported maximum lifetime of the subscriber endpoints.")]
        public DateTime ExpirationDateTime;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the value of the clientState property sent by the service in each notification. The maximum length is 128 characters. The client can check that the notification came from the service by comparing the value of the clientState property sent with the subscription with the value of the clientState property received with each notification.")]
        public String ClientState;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the latest version of Transport Layer Security (TLS) that the notification endpoint, specified by NotificationUrl, supports. If not provided, TLS 1.2 will be assumed.")]
        public OfficeDevPnP.Core.Enums.GraphSubscriptionTlsVersion LatestSupportedTlsVersion = OfficeDevPnP.Core.Enums.GraphSubscriptionTlsVersion.v1_2;

        protected override void ExecuteCmdlet()
        {
            var subscription = SubscriptionsUtility.CreateSubscription(
                changeType: ChangeType,
                notificationUrl: NotificationUrl,
                resource: Resource,
                expirationDateTime: ExpirationDateTime,
                clientState: ClientState,
                accessToken: AccessToken,
                latestSupportedTlsVersion: ParameterSpecified(nameof(LatestSupportedTlsVersion)) ? LatestSupportedTlsVersion : default);

            WriteObject(subscription);
        }
    }
}
#endif