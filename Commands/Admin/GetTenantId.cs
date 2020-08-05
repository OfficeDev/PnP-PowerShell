#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System;
using System.Management.Automation;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantId")]
    [CmdletHelp(@"Returns the Tenant ID",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantId",
        Remarks = @"Returns the current Tenant Id. A valid connection with Connect-PnPOnline is required.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantId -TenantUrl https://contoso.sharepoint.com",
        Remarks = @"Returns the Tenant ID for the specified tenant. Can be executed without a connecting first with Connect-PnPOnline", SortOrder = 1)]
    public class GetTenantId : BasePSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string TenantUrl;

        protected override void ProcessRecord()
        {
            try
            {
                if (string.IsNullOrEmpty(TenantUrl) && PnPConnection.CurrentConnection != null)
                {
                    WriteObject(TenantExtensions.GetTenantIdByUrl(PnPConnection.CurrentConnection.Url));
                }
                else if (!string.IsNullOrEmpty(TenantUrl))
                {
                    WriteObject(TenantExtensions.GetTenantIdByUrl(TenantUrl));
                }
                else
                {
                    throw new InvalidOperationException("Either a connection needs to be made by Connect-PnPOnline or TenantUrl needs to be specified");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
#if !PNPPSCORE
                    if (ex.InnerException is HttpException)
#else
                    if (ex.InnerException is HttpRequestException)
#endif
                    {
                        var message = ex.InnerException.Message;

                        using (var jdoc = JsonDocument.Parse(message))
                        {
                            var errorDescription = jdoc.RootElement.GetProperty("error_description").GetString();
                            WriteObject(errorDescription);
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
#endif