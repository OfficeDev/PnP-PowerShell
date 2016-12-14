#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.UserProfiles
{

    [Cmdlet(VerbsCommon.New, "PnPPersonalSite")]
    [CmdletAlias("New-SPOPersonalSite")]
    [CmdletHelp(@"Office365 only: Creates a personal / OneDrive For Business site",
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> $users = ('katiej@contoso.onmicrosoft.com','garth@contoso.onmicrosoft.com')
                 PS:> New-PnPPersonalSite -Email $users",
        Remarks = "Creates a personal / OneDrive For Business site for the 2 users in the variable $users",
        SortOrder = 1)]

    public class NewPersonalSite : SPOAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The UserPrincipalName (UPN) of the users", Position = 0)]
        public string[] Email;

        protected override void ExecuteCmdlet()
        {
            ProfileLoader profileLoader = ProfileLoader.GetProfileLoader(ClientContext);
            profileLoader.CreatePersonalSiteEnqueueBulk(Email);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif