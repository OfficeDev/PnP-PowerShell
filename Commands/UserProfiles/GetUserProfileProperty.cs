using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Get, "PnPUserProfileProperty")]
#if !ONPREMISES
    [CmdletHelp(@"You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet. ", 
        DetailedDescription = "Requires a connection to a SharePoint Tenant Admin site.", 
        Category = CmdletHelpCategory.UserProfiles,
        OutputType = typeof(PersonProperties),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-csom/jj164752(v=office.15)",
        SupportedPlatform = CmdletSupportedPlatform.All)]
#endif
    [CmdletExample(
        Code = @"PS:> Get-PnPUserProfileProperty -Account 'user@domain.com'", 
        Remarks = "Returns the profile properties for the specified user",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUserProfileProperty -Account 'user@domain.com','user2@domain.com'", 
        Remarks = "Returns the profile properties for the specified users",
        SortOrder = 1)]
    public class GetUserProfileProperty : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com", Position = 0)]
        public string[] Account;

        protected override void ExecuteCmdlet()
        {
            var peopleManager = new PeopleManager(ClientContext);

            foreach (var acc in Account)
            {
                var currentAccount = acc;
#if !ONPREMISES
                var result = Tenant.EncodeClaim(currentAccount);
                ClientContext.ExecuteQueryRetry();
                currentAccount = result.Value;
#endif

                var properties = peopleManager.GetPropertiesFor(currentAccount);
                ClientContext.Load(properties);
                ClientContext.ExecuteQueryRetry();
                WriteObject(properties);
            }
        }
    }
}
