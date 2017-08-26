using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Get, "PnPUser")]
    [CmdletHelp("Returns site users of current web",
        Category = CmdletHelpCategory.Principals,
        DetailedDescription = "This command will return all the users that exist in the current site collection its User Information List",
        OutputType = typeof(User),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser",
        Remarks = "Returns all users from the User Information List of the current site collection",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -Identity 23",
        Remarks = "Returns the user with Id 23 from the User Information List of the current site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com",
        Remarks = "Returns the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser | ? Email -eq ""user@tenant.onmicrosoft.com""",
        Remarks = "Returns the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection",
        SortOrder = 4)]
    public class GetUser : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "User ID or login name")]
        public UserPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var retrievalExpressions = new Expression<Func<User, object>>[] 
            {
                u => u.Id,
                u => u.Title,
                u => u.LoginName,
                u => u.Email,
#if !SP2013
                u => u.IsShareByEmailGuestUser,
#endif
                u => u.IsSiteAdmin,
                u => u.UserId,
                u => u.IsHiddenInUI,
                u => u.PrincipalType,
#if !ONPREMISES
                u => u.Alerts.Include(
                    a => a.Title,
                    a => a.Status),
#endif
                u => u.Groups.Include(
                    g => g.Id,
                    g => g.Title,
                    g => g.LoginName)
            };

            if (Identity == null) {
                ClientContext.Load(SelectedWeb.SiteUsers, users => users.Include(retrievalExpressions));
                ClientContext.ExecuteQueryRetry();
                WriteObject(SelectedWeb.SiteUsers, true);
            }
            else
            {
                User user = null;
                if (Identity.Id > 0)
                {
                    user = ClientContext.Web.GetUserById(Identity.Id);
                }
                else if (Identity.User != null && Identity.User.Id > 0)
                {
                    user = ClientContext.Web.GetUserById(Identity.User.Id);
                }
                else if (!string.IsNullOrWhiteSpace(Identity.Login))
                {
                    user = ClientContext.Web.SiteUsers.GetByLoginName(Identity.Login);
                }
                if (user != null) {
                    ClientContext.Load(user, retrievalExpressions);
                    ClientContext.ExecuteQueryRetry();
                }
                WriteObject(user);
            }
        }
    }
}
