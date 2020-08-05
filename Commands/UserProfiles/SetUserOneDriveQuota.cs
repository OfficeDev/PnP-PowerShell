#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Set, "PnPUserOneDriveQuota")]
    [CmdletHelp(@"Sets the quota on the OneDrive for Business site for a specific user", 
        DetailedDescription = "This command allows you to set the quota on the OneDrive for Business site of a specific user. You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.", 
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserOneDriveQuota -Account 'user@domain.com' -Quota 5368709120 -QuotaWarning 4831838208", 
        Remarks = "Sets the quota on the OneDrive for Business site for the specified user to 5GB (5368709120 bytes) and sets a warning to be shown at 4.5 GB (4831838208)",
        SortOrder = 1)]
    public class SetUserOneDriveQuota : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com", Position = 0)]
        public string Account;

        [Parameter(Mandatory = true, HelpMessage = "The quota to set on the OneDrive for Business site of the user, in bytes", Position = 1)]
        public long Quota;

        [Parameter(Mandatory = true, HelpMessage = "The quota to set on the OneDrive for Business site of the user when to start showing warnings about the drive nearing being full, in bytes", Position = 2)]
        public long QuotaWarning;

        protected override void ExecuteCmdlet()
        {
            var peopleManager = new PeopleManager(ClientContext);
            var oneDriveQuota = peopleManager.SetUserOneDriveQuota(Account, Quota, QuotaWarning);
            ClientContext.ExecuteQueryRetry();
            WriteObject(oneDriveQuota);
        }
    }
}
#endif