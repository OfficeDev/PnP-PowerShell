using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.New, "PnPUser")]
    [CmdletHelp("Adds a user to the built-in Site User Info List and returns a user object",
        Category = CmdletHelpCategory.Principals,
        OutputType = typeof(User),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx")]
    [CmdletExample(
        Code = @"PS:> New-PnPUser -LoginName user@company.com",
        SortOrder = 1,
        Remarks = "Adds a new user with the login user@company.com to the current site")]
    public class NewUser : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The users login name (user@company.com)")]
        [Alias("LogonName")]
        public string LoginName = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var user = SelectedWeb.EnsureUser(LoginName);
            ClientContext.Load(user, u => u.Email, u => u.Id, u => u.IsSiteAdmin, u => u.Groups, u => u.PrincipalType, u => u.Title, u => u.IsHiddenInUI, u => u.UserId, u => u.LoginName);
            ClientContext.ExecuteQueryRetry();
            WriteObject(user);
        }
    }
}
