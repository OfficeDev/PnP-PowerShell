#if !ONPREMISES

using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Webhooks
{
    [Cmdlet(VerbsCommon.Add, "PnPWebhookSubscription")]
    [CmdletHelp("Adds a new Webhook subscription",
        Category = CmdletHelpCategory.Webhooks,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(WebhookSubscription))]
    [CmdletExample(
        Code = "PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook",
        Remarks = "Add a Webhook subscription for the specified notification Url on the list MyList",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate ""2017-09-01""",
        Remarks = "Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate ""2017-09-01"" -ClientState ""Hello State!""",
        Remarks = "Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017 with a specific client state",
        SortOrder = 3)]
    public class AddWebhookSubscription : PnPWebCmdlet
    {
        public const int DefaultValidityInMonths = 6;
        public const int ValidityDeltaInDays = -72; // Note: Some expiration dates too close to the limit are rejected


        [Parameter(Mandatory = false, HelpMessage = "The list object or name where the Webhook subscription will be added to")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The URL of the Webhook endpoint that will be notified of the change")]
        public string NotificationUrl;

        [Parameter(Mandatory = false, HelpMessage = "The date at which the Webhook subscription will expire. (Default: 6 months from today)")]
        public DateTime ExpirationDate = DateTime.Today.ToUniversalTime().AddMonths(DefaultValidityInMonths).AddHours(ValidityDeltaInDays);

        [Parameter(Mandatory = false, HelpMessage = "A client state information that will be passed through notifications")]
        public string ClientState = string.Empty;

        protected override void ExecuteCmdlet()
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

                    // Write the subscription result object
                    WriteObject(list.AddWebhookSubscription(NotificationUrl, ExpirationDate, ClientState));
                }  
            }
            else
            {
                throw new PSNotImplementedException("This Cmdlet only supports List Webhooks currently");
            }
        }

    }
}

#endif