#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Get, "PnPUserOneDriveQuota")]
    [CmdletHelp(@"Retrieves the current quota set on the OneDrive for Business site for a specific user", 
        DetailedDescription = "This command allows you to request the quota set on the OneDrive for Business site of a specific user. You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.", 
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUserOneDriveQuota -Account 'user@domain.com'", 
        Remarks = "Returns the quota set on the OneDrive for Business site for the specified user",
        SortOrder = 1)]
    public class GetUserOneDriveQuota : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com", Position = 0)]
        public string Account;

        protected override void ExecuteCmdlet()
        {
            var peopleManager = new PeopleManager(ClientContext);
            var oneDriveQuota = peopleManager.GetUserOneDriveQuotaMax(Account);
            ClientContext.ExecuteQueryRetry();
            WriteObject(oneDriveQuota);
        }
    }
}
#endif