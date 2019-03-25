using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPHealthScore")]
    [CmdletHelp("Retrieves the healthscore of the site given in his Url parameter", 
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
      [CmdletExample(
        Code = "$username ="<Global Admin UPN>" #ex = jortega@contoso.com
$appPassword= "xxxxxxxxxxxxxxxx" #ex: "gbntxrfnxxxxxxxx"
$adminUrl = "https://<Tenant>-admin.sharepoint.com" #found when you open Administration/Admin Panels/Sharepoint/that root path
 #dont modify anything below here
$cred = New-Object pscredential -ArgumentList $username, (  ConvertTo-SecureString -String $appPassword -AsPlainText -Force)
#connects to SPO
Connect-SPOService -Credential $cred -Url $adminUrl #to get the my addresses
Connect-PnPOnline -Credentials $cred -Url $adminUrl #to get the health
 Get-SPOSite -IncludePersonalSite $true -Limit all -Filter "Url -like '-my.sharepoint.com/personal/" |select Url | %{ 
     New-object psobject -Property @{"Page"= $_.Url;"HealthScore"=(Get-PnPHealthScore -Url $_.URL)}
}",
        Remarks = @"This is an advanced example, where you want to get all hte HealthScore of all your Onedrive for Business sites, tenant wide. The Global admin user has double authentication enabled, so we will need to create a [Application Password](https://support.office.com/en-gb/article/create-an-app-password-for-office-365-3e7c860f-bda4-4441-a618-b53953ee1183)

This example will use double connection to [Sharepoint Online](https://docs.microsoft.com/en-us/powershell/sharepoint/sharepoint-online/connect-sharepoint-online?view=sharepoint-ps#to-connect-with-a-user-name-and-password) and [Sharepoint PNP Online](https://docs.microsoft.com/en-us/powershell/module/sharepoint-pnp/connect-pnponline?view=sharepoint-ps) modules.
This will get all the health of all the Oneddrive for business site and show them as an object into the screen.
This Cmdlet works "per URL" so you'd need to have a list of sites before running the script to get the health of the sites. This is just an interesting example for a passionate documenter.",
        SortOrder = 3)]
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
