#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Reset, "PnPUserOneDriveQuotaToDefault")]
    [CmdletHelp(@"Resets the current quota set on the OneDrive for Business site for a specific user to the tenant default", 
        DetailedDescription = "This command allows you to reset the quota set on the OneDrive for Business site of a specific user to the default as set on the tenant. You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.", 
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> Reset-PnPUserOneDriveQuotaToDefault -Account 'user@domain.com'", 
        Remarks = "Resets the quota set on the OneDrive for Business site for the specified user to the tenant default",
        SortOrder = 1)]
    public class ResetUserOneDriveQuotaMax : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com", Position = 0)]
        public string Account;

        protected override void ExecuteCmdlet()
        {
            var peopleManager = new PeopleManager(ClientContext);
            var result = peopleManager.ResetUserOneDriveQuotaToDefault(Account);
            ClientContext.ExecuteQueryRetry();
            WriteObject(result);
        }
    }
}
#endif