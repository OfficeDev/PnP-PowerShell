using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPHealthScore")]
    [CmdletHelp("Retrieves the healthscore of the site given in his Url parameter or from the current connection if the Url parameter is not provided",
                "Retrieves the current X-SharePointHealthScore value of the server, or CPU, on which your SharePoint instance runs. X-SharePointHealthScore is a value between 0 and 10, where 0 indicates the server is idle and 10 indicates the server is very busy. For more information visit https://docs.microsoft.com/office365/enterprise/diagnosing-performance-issues-with-sharepoint-online and https://docs.microsoft.com/openspecs/sharepoint_protocols/ms-wsshp/c60ddeb6-4113-4a73-9e97-26b5c3907d33.",
        Category = CmdletHelpCategory.Base,
        OutputType=typeof(int),
        SupportedPlatform = CmdletSupportedPlatform.OnPremises,
        OutputTypeDescription = "Returns a int value representing the current health score value of the server.")]
    [CmdletExample(
        Code = "PS:> Get-PnPHealthScore", 
        Remarks = @"This will retrieve the current health score of the server.",        
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPHealthScore -Url https://contoso.sharepoint.com",
        Remarks = @"This will retrieve the current health score for the url https://contoso.sharepoint.com.",
        SortOrder = 2)]
#if !ONPREMISES
    [Obsolete("Get-PnPHealthScore does not return valid data when using SharePoint Online")]
#endif
    public class GetHealthScore : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The url of the WebApplication to retrieve the health score from", ValueFromPipeline = true)]
        public string Url { get; set; }

        protected override void ProcessRecord()
        {
            string url;
            if (Url != null)
            {
                url = Url;
            }
            else
            {
                if (PnPConnection.CurrentConnection != null)
                {
                    url = PnPConnection.CurrentConnection.Url;
                }
                else
                {
                    throw new Exception(Properties.Resources.NoContextPresent);
                }
            }
            WriteObject(Utility.GetHealthScore(url));
        }
    }
}
