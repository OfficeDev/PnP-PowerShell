using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPHealthScore")]
    [CmdletHelp("Retrieves the healthscore", 
        "Retrieves the current health score value of the server which is a value between 0 and 10. Lower is better.", 
        Category = CmdletHelpCategory.Base,
        OutputType=typeof(int),
        OutputTypeDescription = "Returns a int value representing the current health score value of the server.")]
    [CmdletExample(
        Code = "PS:> Get-PnPHealthScore", 
        Remarks = @"This will retrieve the current health score of the server.",        
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPHealthScore -Url https://contoso.sharepoint.com",
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
