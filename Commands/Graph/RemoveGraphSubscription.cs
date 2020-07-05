#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPGraphSubscription")]
    [CmdletHelp("Removes an existing Microsoft Graph subscription. Required Azure Active Directory application permission depends on the resource the subscription exists on, see https://docs.microsoft.com/graph/api/subscription-delete#permissions.",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/subscription-delete",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPGraphSubscription -Identity bc204397-1128-4911-9d70-1d8bceee39da",
       Remarks = "Removes the Microsoft Graph subscription with the id 'bc204397-1128-4911-9d70-1d8bceee39da'",
       SortOrder = 1)]
    // Deliberately omitting the CmdletMicrosoftGraphApiPermission attribute as permissions vary largely by the subscription type being used
    public class RemoveGraphSubscription : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The unique id or an instance of a Microsoft Graph Subscription")]
        public GraphSubscriptionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                SubscriptionsUtility.DeleteSubscription(Identity.SubscriptionId, AccessToken);
            }
        }
    }
}
#endif