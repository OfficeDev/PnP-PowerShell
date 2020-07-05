#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPGraphSubscription", DefaultParameterSetName = ParameterSet_LIST)]
    [CmdletHelp("Gets subscriptions from Microsoft Graph. Requires the Azure Active Directory application permission 'Subscription.Read.All'.",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/subscription-get",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPGraphSubscription",
       Remarks = "Retrieves all subscriptions from Microsoft Graph",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPGraphSubscription -Identity 328c7693-5524-44ac-a946-73e02d6b0f98",
       Remarks = "Retrieves the subscription from Microsoft Graph with the id 328c7693-5524-44ac-a946-73e02d6b0f98",
       SortOrder = 2)]
    // Deliberately omitting the CmdletMicrosoftGraphApiPermission attribute as permissions vary largely by the subscription type being used
    public class GetGraphSubscription : PnPGraphCmdlet
    {
        const string ParameterSet_BYID = "Return by specific ID";
        const string ParameterSet_LIST = "Return a list";

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_BYID, HelpMessage = "Returns the subscription with the provided subscription id")]
        public string Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                OfficeDevPnP.Core.Framework.Graph.Model.Subscription subscription = OfficeDevPnP.Core.Framework.Graph.SubscriptionsUtility.GetSubscription(AccessToken, System.Guid.Parse(Identity));
                WriteObject(subscription);
            }
            else
            {
                List<OfficeDevPnP.Core.Framework.Graph.Model.Subscription> subscriptions = OfficeDevPnP.Core.Framework.Graph.SubscriptionsUtility.ListSubscriptions(AccessToken);
                WriteObject(subscriptions, true);
            }
        }
    }
}
#endif