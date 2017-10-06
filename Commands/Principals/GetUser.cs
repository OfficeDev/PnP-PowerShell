using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;
using System.Collections.Generic;

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
        Remarks = "Returns all users from the User Information List of the current site collection regardless if they currently have rights to access the current site",
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
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -WithRightsAssigned",
        Remarks = "Returns only those users from the User Information List of the current site collection who currently have any kind of access rights given either directly to the user or Active Directory Group or given to the user or Active Directory Group via membership of a SharePoint Group to the current site",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -WithRightsAssigned -Web subsite1",
        Remarks = "Returns only those users from the User Information List of the current site collection who currently have any kind of access rights given either directly to the user or Active Directory Group or given to the user or Active Directory Group via membership of a SharePoint Group to subsite 'subsite1'",
        SortOrder = 6)]
    public class GetUser : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "User ID or login name")]
        public UserPipeBind Identity;

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "If provided, only users that currently have any kinds of access rights assigned to the current site collection will be returned. Otherwise all users, even those who previously had rights assigned, but not anymore at the moment, will be returned as the information is pulled from the User Information List. Only works if you don't provide an -Identity.")]
        public SwitchParameter WithRightsAssigned;

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

            if (Identity == null)
            {
                SelectedWeb.Context.Load(SelectedWeb.SiteUsers, u => u.Include(retrievalExpressions));

                if (WithRightsAssigned)
                {
                    // Get all the role assignments and role definition bindings to be able to see which users have been given rights directly on the site level
                    SelectedWeb.Context.Load(SelectedWeb.RoleAssignments, ac => ac.Include(a => a.RoleDefinitionBindings, a => a.Member));
                    var usersWithDirectPermissions = SelectedWeb.SiteUsers.Where(u => SelectedWeb.RoleAssignments.Any(ra => ra.Member.LoginName == u.LoginName));

                    // Get all the users contained in SharePoint Groups
                    SelectedWeb.Context.Load(SelectedWeb.SiteGroups, sg => sg.Include(u => u.Users));
                    SelectedWeb.Context.ExecuteQueryRetry();

                    var usersWithGroupPermissions = new List<User>();
                    foreach (var group in SelectedWeb.SiteGroups)
                    {
                        // If they're in a SharePoint Group, they always have some kind of access rights, so add them all
                        usersWithGroupPermissions.AddRange(group.Users);
                    }

                    // Merge the users with rights directly on the site level and those assigned rights through SharePoint Groups
                    var allUsersWithPermissions = new List<User>(usersWithDirectPermissions.Count() + usersWithGroupPermissions.Count());
                    allUsersWithPermissions.AddRange(usersWithDirectPermissions);
                    allUsersWithPermissions.AddRange(usersWithGroupPermissions);

                    // Filter out the users that have been given rights at both places so they will only be returned once
                    WriteObject(allUsersWithPermissions.GroupBy(u => u.Id).Select(u => u.First()), true);
                }
                else
                {
                    SelectedWeb.Context.ExecuteQueryRetry();
                    WriteObject(SelectedWeb.SiteUsers, true);
                }
            }
            else
            {
                User user = null;
                if (Identity.Id > 0)
                {
                    user = SelectedWeb.GetUserById(Identity.Id);
                }
                else if (Identity.User != null && Identity.User.Id > 0)
                {
                    user = SelectedWeb.GetUserById(Identity.User.Id);
                }
                else if (!string.IsNullOrWhiteSpace(Identity.Login))
                {
                    user = SelectedWeb.SiteUsers.GetByLoginName(Identity.Login);
                }
                if (user != null) {
                    SelectedWeb.Context.Load(user, retrievalExpressions);
                    SelectedWeb.Context.ExecuteQueryRetry();
                }
                WriteObject(user);
            }
        }

        private object List<T>()
        {
            throw new NotImplementedException();
        }
    }
}