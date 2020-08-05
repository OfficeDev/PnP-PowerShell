#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model;
using PnP.PowerShell.Commands.Utilities.REST;

namespace PnP.PowerShell.Commands.ManagementApi
{
    [Cmdlet(VerbsCommon.Get, "PnPOffice365HistoricalServiceStatus")]
    [CmdletHelp(
        "Gets the historical service status of the Office 365 Services of the last 7 days from the Office 365 Management API",
        Category = CmdletHelpCategory.ManagementApi,
        OutputTypeLink = "https://docs.microsoft.com/office/office-365-management-api/office-365-service-communications-api-reference#get-historical-status",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365HistoricalServiceStatus",
       Remarks = "Retrieves the historical service status of all Office 365 services",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365HistoricalServiceStatus -Workload SharePoint",
       Remarks = "Retrieves the historical service status of SharePoint Online",
       SortOrder = 2)]
    [CmdletOfficeManagementApiPermission(OfficeManagementApiPermission.ServiceHealth_Read)]
    public class GetOffice365HistoricalServiceStatus : PnPOfficeManagementApiCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Allows retrieval of the historical service status of only one particular service. If not provided, the historical service status of all services will be returned.")]
        public Enums.Office365Workload? Workload;

        protected override void ExecuteCmdlet()
        {
            var collection = GraphHelper.GetAsync<GraphCollection<ManagementApiServiceStatus>>(HttpClient, $"{ApiRootUrl}ServiceComms/HistoricalStatus{(ParameterSpecified(nameof(Workload)) ? $"?$filter=Workload eq '{Workload.Value}'" : "")}", AccessToken, false).GetAwaiter().GetResult();

            if (collection != null)
            {
                WriteObject(collection.Items, true);
            }
            //var response = GraphHttpClient.MakeGetRequestForString($"{ApiRootUrl}ServiceComms/HistoricalStatus{(ParameterSpecified(nameof(Workload)) ? $"?$filter=Workload eq '{Workload.Value}'" : "")}", AccessToken);
            //var serviceStatusesJson = JObject.Parse(response);
            //var serviceStatuses = serviceStatusesJson["value"].ToObject<IEnumerable<ManagementApiServiceStatus>>();

            //WriteObject(serviceStatuses, true);
        }
    }
}
#endif