#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model;
using PnP.PowerShell.Commands.Utilities.REST;

namespace PnP.PowerShell.Commands.ManagementApi
{
    [Cmdlet(VerbsCommon.Get, "PnPOffice365ServiceMessage")]
    [CmdletHelp(
        "Gets the service messages regarding services in Office 365 from the Office 365 Management API",
        Category = CmdletHelpCategory.ManagementApi,
        OutputTypeLink = "https://docs.microsoft.com/office/office-365-management-api/office-365-service-communications-api-reference#get-messages",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365ServiceMessage",
       Remarks = "Retrieves the service messages regarding services in Office 365",
       SortOrder = 1)]
    [CmdletOfficeManagementApiPermission(OfficeManagementApiPermission.ServiceHealth_Read)]
    public class GetOffice365ServiceMessage : PnPOfficeManagementApiCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Allows retrieval of the service messages for only one particular service. If not provided, the service messages of all services will be returned.")]
        public Enums.Office365Workload? Workload;

        protected override void ExecuteCmdlet()
        {
            var collection = GraphHelper.GetAsync<GraphCollection<ManagementApiServiceMessage>>(HttpClient, $"{ApiRootUrl}ServiceComms/Messages{(ParameterSpecified(nameof(Workload)) ? $"?$filter=Workload eq '{Workload.Value}'" : "")}", AccessToken, false).GetAwaiter().GetResult();

            if (collection != null)
            {
                WriteObject(collection.Items, true);
            }

            //var response = GraphHttpClient.MakeGetRequestForString($"{ApiRootUrl}ServiceComms/Messages{(ParameterSpecified(nameof(Workload)) ? $"?$filter=Workload eq '{Workload.Value}'" : "")}", AccessToken);
            //var servicesJson = JObject.Parse(response);
            //var services = servicesJson["value"].ToObject<IEnumerable<ManagementApiServiceMessage>>();

            //WriteObject(services, true);
        }
    }
}
#endif