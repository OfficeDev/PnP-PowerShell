#if !ONPREMISES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model;
using PnP.PowerShell.Commands.Utilities.REST;

namespace PnP.PowerShell.Commands.ManagementApi
{
    [Cmdlet(VerbsCommon.Get, "PnPUnifiedAuditLog")]
    [CmdletHelp(
        "Gets unified audit logs from the Office 365 Management API. Requires the Azure Active Directory application permission 'ActivityFeed.Read'.",
        Category = CmdletHelpCategory.ManagementApi,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedAuditLog -ContentType SharePoint -StartTime (Get-Date).AddDays(-1) -EndTime (Get-Date).AddDays(-2)",
       Remarks = "Retrieves the audit logs of SharePoint happening between the current time yesterday and the current time the day before yesterday",
       SortOrder = 1)]
    [CmdletOfficeManagementApiPermission(OfficeManagementApiPermission.ActivityFeed_Read)]
    public class GetUnifiedAuditLog : PnPOfficeManagementApiCmdlet
    {
        private const string ParameterSet_LogsByDate = "Logs by date";

        [Parameter(Mandatory = false, HelpMessage = "Content type of logs to be retrieved, should be one of the following: AzureActiveDirectory, Exchange, SharePoint, General, DLP.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_LogsByDate, HelpMessage = "Content type of logs to be retreived, should be one of the following: AzureActiveDirectory, Exchange, SharePoint, General, DLP.")]
        public Enums.AuditContentType ContentType = Enums.AuditContentType.SharePoint;

        [Parameter(
            Mandatory = false,
            ParameterSetName = ParameterSet_LogsByDate,
            HelpMessage = "Start time of logs to be retrieved. Start time and end time must both be specified (or both omitted) and must be less than or equal to 24 hours apart, with the start time prior to end time and start time no more than 7 days in the past.")]
        public DateTime StartTime = DateTime.MinValue;

        [Parameter(
            Mandatory = false,
            ParameterSetName = ParameterSet_LogsByDate,
            HelpMessage = "End time of logs to be retrieved. Start time and end time must both be specified (or both omitted) and must be less than or equal to 24 hours apart.")]
        public DateTime EndTime = DateTime.MaxValue;

        /// <summary>
        /// Returns the Content Type to query in a string format which is recognized by the Office 365 Management API
        /// </summary>
        protected string ContentTypeString
        {
            get
            {
                switch (ContentType)
                {
                    case Enums.AuditContentType.AzureActiveDirectory: return "Audit.AzureActiveDirectory";
                    case Enums.AuditContentType.SharePoint: return "Audit.SharePoint";
                    case Enums.AuditContentType.Exchange: return "Audit.Exchange";
                    case Enums.AuditContentType.DLP: return "DLP.All";
                    case Enums.AuditContentType.General: default: return "Audit.General";
                }
            }
        }

        /// <summary>
        /// Base URL to the Office 365 Management API being used in this cmdlet
        /// </summary>
        protected string ApiUrl => $"{ApiRootUrl}activity/feed";

        private IEnumerable<ManagementApiSubscription> GetSubscriptions()
        {
            var url = $"{ApiUrl}/subscriptions/list";
            return GraphHelper.GetAsync<IEnumerable<ManagementApiSubscription>>(HttpClient, url, AccessToken).GetAwaiter().GetResult();

            //var res = GraphHttpClient.MakeGetRequestForString(url.ToString(), AccessToken);
            //var subscriptions = JsonConvert.DeserializeObject<IEnumerable<ManagementApiSubscription>>(res);
            //return subscriptions;
        }

        private void EnsureSubscription(string contentType)
        {
            var subscriptions = GetSubscriptions();
            var subscription = subscriptions.FirstOrDefault(s => s.ContentType == contentType);
            if (subscription == null)
            {
                subscription = GraphHelper.PostAsync<ManagementApiSubscription>(HttpClient, $"{ApiUrl}/subscriptions/start?contentType={contentType}&PublisherIdentifier={Token.TenantId}", AccessToken).GetAwaiter().GetResult();
                if (!subscription.Status.Equals("enabled", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Cannot enable subscription for {contentType}");
                }
                //    var url = $"{ApiUrl}/subscriptions/start?contentType={contentType}&PublisherIdentifier={Token.TenantId}";
                //var res = GraphHttpClient.MakePostRequestForString(url.ToString(), accessToken: AccessToken);
                //var response = JsonConvert.DeserializeObject<ManagementApiSubscription>(res);
                //if (response.Status != "enabled")
                //{
                //    throw new Exception($"Cannot enable subscription for {contentType}");
                //}
            }
        }

        protected override void ExecuteCmdlet()
        {
            EnsureSubscription(ContentTypeString);

            var url = $"{ApiUrl}/subscriptions/content?contentType={ContentTypeString}&PublisherIdentifier=${Token.TenantId}";
            if (StartTime != DateTime.MinValue)
            {
                url += $"&startTime={StartTime:yyyy-MM-ddThh:mm:ss}";
            }
            if (EndTime != DateTime.MaxValue)
            {
                url += $"&endTime={EndTime:yyyy-MM-ddThh:mm:ss}";
            }
            
            var subscriptionContents = GraphHelper.GetAsync<IEnumerable<ManagementApiSubscriptionContent>>(HttpClient, url, AccessToken).GetAwaiter().GetResult();
            if(subscriptionContents != null)
            {
                foreach(var content in subscriptionContents)
                {
                    var logs = GraphHelper.GetAsync<IEnumerable<ManagementApiUnifiedLogRecord>>(HttpClient, content.ContentUri, AccessToken,false).GetAwaiter().GetResult();
                    WriteObject(logs, true);
                }
            }

            /*
            var res = GraphHttpClient.MakeGetRequestForString(url.ToString(), AccessToken);
            if (!string.IsNullOrEmpty(res))
            {
                var contents = JsonConvert.DeserializeObject<IEnumerable<ManagementApiSubscriptionContent>>(res);
                foreach (var content in contents)
                {
                    res = GraphHttpClient.MakeGetRequestForString(content.ContentUri, AccessToken);
                    var logs = JsonConvert.DeserializeObject<IEnumerable<ManagementApiUnifiedLogRecord>>(res);
                    WriteObject(logs, true);
                }
            }
            */
        }
    }
}
#endif