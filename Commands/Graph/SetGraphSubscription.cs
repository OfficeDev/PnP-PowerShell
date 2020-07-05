#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPGraphSubscription")]
    [CmdletHelp("Updates an existing Microsoft Graph subscription. Required Azure Active Directory application permission depends on the resource the subscription exists on, see https://docs.microsoft.com/graph/api/subscription-delete#permissions.",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/subscription-update",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPGraphSubscription -Identity bc204397-1128-4911-9d70-1d8bceee39da -ExpirationDate \"2020-11-22T18:23:45.9356913Z\"",
       Remarks = "Updates the Microsoft Graph subscription with the id 'bc204397-1128-4911-9d70-1d8bceee39da' to expire at the mentioned date",
       SortOrder = 1)]
    // Deliberately omitting the CmdletMicrosoftGraphApiPermission attribute as permissions vary largely by the subscription type being used
    public class SetGraphSubscription : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The unique id or an instance of a Microsoft Graph Subscription")]
        public GraphSubscriptionPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "Date and time to set the expiration to. Take notice of the maximum allowed lifetime of the subscription endponts as documented at https://docs.microsoft.com/graph/api/resources/subscription#maximum-length-of-subscription-per-resource-type")]
        public DateTime ExpirationDate;

        protected override void ExecuteCmdlet()
        {
            var subscription = SubscriptionsUtility.UpdateSubscription(Identity.SubscriptionId, ExpirationDate, AccessToken);
            WriteObject(subscription);
        }
    }
}
#endif