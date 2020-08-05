#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model;
using PnP.PowerShell.Commands.Utilities.REST;

namespace PnP.PowerShell.Commands.ManagementApi
{
    [Cmdlet(VerbsCommon.Get, "PnPOffice365CurrentServiceStatus")]
    [CmdletHelp(
        "Gets current service status of the Office 365 Services from the Office 365 Management API",
        Category = CmdletHelpCategory.ManagementApi,
        OutputTypeLink = "https://docs.microsoft.com/office/office-365-management-api/office-365-service-communications-api-reference#get-current-status",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365CurrentServiceStatus",
       Remarks = "Retrieves the current service status of all Office 365 services",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365CurrentServiceStatus -Workload SharePoint",
       Remarks = "Retrieves the current service status of SharePoint Online",
       SortOrder = 2)]
    [CmdletOfficeManagementApiPermission(OfficeManagementApiPermission.ServiceHealth_Read)]
    public class GetOffice365CurrentServiceStatus : PnPOfficeManagementApiCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Allows retrieval of the current service status of only one particular service. If not provided, the current service status of all services will be returned.")]
        public Enums.Office365Workload? Workload;

        protected override void ExecuteCmdlet()
        {
            var collection = GraphHelper.GetAsync<GraphCollection<ManagementApiServiceStatus>>(HttpClient, $"{ApiRootUrl}ServiceComms/CurrentStatus{(ParameterSpecified(nameof(Workload)) ? $"?$filter=Workload eq '{Workload.Value}'" : "")}", AccessToken, false).GetAwaiter().GetResult();

            if (collection != null)
            {
                WriteObject(collection.Items, true);
            }
        }
    }
}
#endif