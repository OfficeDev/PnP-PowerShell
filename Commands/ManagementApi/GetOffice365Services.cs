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
    [Cmdlet(VerbsCommon.Get, "PnPOffice365Services")]
    [CmdletHelp(
        "Gets the services available in Office 365 from the Office 365 Management API",
        Category = CmdletHelpCategory.ManagementApi,
        OutputTypeLink = "https://docs.microsoft.com/office/office-365-management-api/office-365-service-communications-api-reference#get-services",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPOffice365Services",
       Remarks = "Retrieves the current services available in Office 365",
       SortOrder = 1)]
    [CmdletOfficeManagementApiPermission(OfficeManagementApiPermission.ServiceHealth_Read)]
    public class GetOffice365Services : PnPOfficeManagementApiCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var collection = GraphHelper.GetAsync<GraphCollection<ManagementApiService>>(HttpClient, $"{ApiRootUrl}ServiceComms/Services", AccessToken, false).GetAwaiter().GetResult();

            if(collection != null)
            {
                WriteObject(collection.Items, true);
            }
        }
    }
}
#endif