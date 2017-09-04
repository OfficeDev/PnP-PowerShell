using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Remove, "PnPUser")]
    [CmdletHelp("Removes a specific user from the site collection User Information List",
        Category = CmdletHelpCategory.Principals,
        DetailedDescription = "This command will allow the removal of a specific user from the User Information List",
        OutputType = typeof(User),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx")]
    [CmdletExample(
        Code = @"PS:> Remove-PnPUser -Identity 23",
        Remarks = "Remove the user with Id 23 from the User Information List of the current site collection",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com",
        Remarks = "Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser | ? Email -eq ""user@tenant.onmicrosoft.com"" | Remove-PnPUser",
        Remarks = "Remove the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com -Confirm:$false",
        Remarks = "Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection without asking to confirm the removal first",
        SortOrder = 4)]
    public class RemoveUser : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "User ID or login name")]
        public UserPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Confirm parameter will allow the confirmation question to be skipped")]
        public SwitchParameter Confirm;

        protected override void ExecuteCmdlet()
        {
            var retrievalExpressions = new Expression<Func<User, object>>[]
            {
                u => u.Id,
                u => u.LoginName,
                u => u.Email
            };

            User user = null;
            if (Identity.User != null)
            {
                WriteVerbose($"Received user instance {Identity.Login}");
                user = Identity.User;
            }
            else if (Identity.Id > 0)
            {
                WriteVerbose($"Retrieving user by Id {Identity.Id}");
                user = ClientContext.Web.GetUserById(Identity.Id);
            }
            else if (!string.IsNullOrWhiteSpace(Identity.Login))
            {
                WriteVerbose($"Retrieving user by LoginName {Identity.Login}");
                user = ClientContext.Web.SiteUsers.GetByLoginName(Identity.Login);
            }
            if (ClientContext.HasPendingRequest)
            {
                ClientContext.Load(user, retrievalExpressions);
                ClientContext.ExecuteQueryRetry();
            }

            if (user != null)
            {
                if (Force || (MyInvocation.BoundParameters.ContainsKey("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveUser, user.Id, user.LoginName, user.Email), Properties.Resources.Confirm))
                {
                    WriteVerbose($"Removing user {user.Id} {user.LoginName} {user.Email}");
                    ClientContext.Web.SiteUsers.Remove(user);
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                throw new ArgumentException("Unable to find user", "Identity");
            }
        }
    }
}