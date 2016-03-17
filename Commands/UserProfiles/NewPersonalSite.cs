using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base;

#if !CLIENTSDKV15
namespace OfficeDevPnP.PowerShell.Commands.UserProfiles
{

    [Cmdlet(VerbsCommon.New, "SPOPersonalSite")]
    [CmdletHelp(@"Office365 only: Creates a personal / OneDrive For Business site",
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> $users = ('katiej@contoso.onmicrosoft.com','garth@contoso.onmicrosoft.com')
                 PS:> New-SPOPersonalSite -Email $users",
        Remarks = "Creates a personal / OneDrive For Business site for the 2 users in the variable $users",
        SortOrder = 1)]

    public class NewPersonalSite : SPOAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The UserPrincipalName (UPN) of the user", Position = 0)]
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