using OfficeDevPnP.Core.Utilities;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace OfficeDevPnP.PowerShell.Commands.Base
{
    [Cmdlet("Get", "SPOHealthScore")]
    [CmdletHelp("Retrieves the current health score value of the server", 
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = "PS:> Get-SPOHealthScore", 
        Remarks = @"This will retrieve the current health score of the server.",        
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-SPOHealthScore -Url https://contoso.sharepoint.com",
        Remarks = @"This will retrieve the current health score for the url https://contoso.sharepoint.com.",
        SortOrder = 2)]
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
                if (SPOnlineConnection.CurrentConnection != null)
                {
                    url = SPOnlineConnection.CurrentConnection.Url;
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
