#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Get, "PnPUserProfileProperty")]
    [CmdletAlias("Get-SPOUserProfileProperty")]
    [CmdletHelp(@"You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet. 
", DetailedDescription = "Requires a connection to a SharePoint Tenant Admin site.", 
        Category = CmdletHelpCategory.UserProfiles,
         OutputType = typeof(PersonProperties),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.userprofiles.personproperties.aspx")]
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
                var result = Tenant.EncodeClaim(acc);
                ClientContext.ExecuteQueryRetry();
                var properties = peopleManager.GetPropertiesFor(result.Value);
                ClientContext.Load(properties);
                ClientContext.ExecuteQueryRetry();
                WriteObject(properties);
            }
        }
    }
}
#endif