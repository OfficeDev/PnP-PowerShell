using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;
using System.Text;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Get, "PnPUser", DefaultParameterSetName = PARAMETERSET_IDENTITY)]
    [CmdletHelp("Returns site users of current web",
        Category = CmdletHelpCategory.Principals,
        DetailedDescription = "This command will return all users that exist in the current site collection's User Information List, optionally identifying their current permissions to this site",
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser",
        Remarks = "Returns all users from the User Information List of the current site collection regardless if they currently have rights to access the current site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -Identity 23",
        Remarks = "Returns the user with Id 23 from the User Information List of the current site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -Identity ""i:0#.f|membership|user@tenant.onmicrosoft.com""",
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
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Get-PnPUser -WithRightsAssignedDetailed",
        Remarks = "Returns all users who have been granted explicit access to the current site, lists and listitems",
        SortOrder = 7)]
#endif
    public class GetUser : PnPWebCmdlet
    {
        private const string PARAMETERSET_IDENTITY = "Identity based request";
        private const string PARAMETERSET_WITHRIGHTSASSIGNED = "With rights assigned";
#if !ONPREMISES
        private const string PARAMETERSET_WITHRIGHTSASSIGNEDDETAILED = "With rights assigned detailed";
#endif

        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = PARAMETERSET_IDENTITY, HelpMessage = "User ID or login name")]
        public UserPipeBind Identity;

        [Parameter(Mandatory = false, ParameterSetName = PARAMETERSET_WITHRIGHTSASSIGNED, HelpMessage = "If provided, only users that currently have any kinds of access rights assigned to the current site collection will be returned. Otherwise all users, even those who previously had rights assigned, but not anymore at the moment, will be returned as the information is pulled from the User Information List. Only works if you don't provide an -Identity.")]
        public SwitchParameter WithRightsAssigned;

#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = PARAMETERSET_WITHRIGHTSASSIGNEDDETAILED, HelpMessage = "If provided, only users that currently have any specific kind of access rights assigned to the current site, lists or listitems/documents will be returned. Otherwise all users, even those who previously had rights assigned, but not anymore at the moment, will be returned as the information is pulled from the User Information List. Only works if you don't provide an -Identity.")]
        public SwitchParameter WithRightsAssignedDetailed;
#endif

        /// <summary>
        /// Output type used with parameter WithRightsAssignedDetailed
        /// </summary>
        public class DetailedUser
        {
            public User User { get; set; }
            public string Url { get; set; }
            public List<string> Permissions { get; set; }
            public GroupCollection Groups { get; set; }
        }

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

                List<DetailedUser> users = new List<DetailedUser>();

                if (WithRightsAssigned
#if !ONPREMISES
                    || WithRightsAssignedDetailed
#endif
                    )
                {
                    // Get all the role assignments and role definition bindings to be able to see which users have been given rights directly on the site level
                    SelectedWeb.Context.Load(SelectedWeb.RoleAssignments, ac => ac.Include(a => a.RoleDefinitionBindings, a => a.Member));
                    var usersWithDirectPermissions = SelectedWeb.SiteUsers.Where(u => SelectedWeb.RoleAssignments.Any(ra => ra.Member.LoginName == u.LoginName));

                    // Get all the users contained in SharePoint Groups
                    SelectedWeb.Context.Load(SelectedWeb.SiteGroups, sg => sg.Include(u => u.Users.Include(retrievalExpressions), u => u.LoginName));
                    SelectedWeb.Context.ExecuteQueryRetry();

                    // Get all SharePoint groups that have been assigned access
                    var usersWithGroupPermissions = new List<User>();
                    foreach (var group in SelectedWeb.SiteGroups.Where(g => SelectedWeb.RoleAssignments.Any(ra => ra.Member.LoginName == g.LoginName)))
                    {
                        usersWithGroupPermissions.AddRange(group.Users);
                    }

                    // Merge the users with rights directly on the site level and those assigned rights through SharePoint Groups
                    var allUsersWithPermissions = new List<User>(usersWithDirectPermissions.Count() + usersWithGroupPermissions.Count());
                    allUsersWithPermissions.AddRange(usersWithDirectPermissions);
                    allUsersWithPermissions.AddRange(usersWithGroupPermissions);

#if !ONPREMISES
                    // Add the found users and add them to the custom object
                    if (WithRightsAssignedDetailed)
                    {
                        SelectedWeb.Context.Load(SelectedWeb, s => s.ServerRelativeUrl);
                        SelectedWeb.Context.ExecuteQueryRetry();

                        WriteWarning("Using the -WithRightsAssignedDetailed parameter will cause the script to take longer than normal because of the all enumerations that take place");
                        users.AddRange(GetPermissions(SelectedWeb.RoleAssignments, SelectedWeb.ServerRelativeUrl));
                        foreach (var user in allUsersWithPermissions)
                        {
                            users.Add(new DetailedUser()
                            {
                                Groups = user.Groups,
                                User = user,
                                Url = SelectedWeb.ServerRelativeUrl
                            });
                        }
                    }
                    else
#endif
                    {
                        // Filter out the users that have been given rights at both places so they will only be returned once
                        WriteObject(allUsersWithPermissions.GroupBy(u => u.Id).Select(u => u.First()), true);
                    }
                }
                else
                {
                    SelectedWeb.Context.ExecuteQueryRetry();
                    WriteObject(SelectedWeb.SiteUsers, true);
                }

#if !ONPREMISES
                if (WithRightsAssignedDetailed)
                {
                    SelectedWeb.Context.Load(SelectedWeb.Lists, l => l.Include(li => li.ItemCount, li => li.IsSystemList, li=>li.IsCatalog, li => li.RootFolder.ServerRelativeUrl, li => li.RoleAssignments, li => li.Title, li => li.HasUniqueRoleAssignments));
                    SelectedWeb.Context.ExecuteQueryRetry();

                    var progress = new ProgressRecord(0, $"Getting lists for {SelectedWeb.ServerRelativeUrl}", "Enumerating through lists");
                    var progressCounter = 0;

                    foreach (var list in SelectedWeb.Lists)
                    {
                        WriteProgress(progress, $"Getting list {list.RootFolder.ServerRelativeUrl}", progressCounter++, SelectedWeb.Lists.Count);

                        // ignoring the system lists
                        if (list.IsSystemList || list.IsCatalog)
                        {
                            continue;
                        }

                        // if a list or a library has unique permissions then proceed
                        if (list.HasUniqueRoleAssignments)
                        {
                            WriteVerbose(string.Format("List found with HasUniqueRoleAssignments {0}", list.RootFolder.ServerRelativeUrl));
                            string url = list.RootFolder.ServerRelativeUrl;

                            SelectedWeb.Context.Load(list.RoleAssignments, r => r.Include(
                                ra => ra.RoleDefinitionBindings,
                                ra => ra.Member.LoginName,
                                ra => ra.Member.Title,
                                ra => ra.Member.PrincipalType));
                            SelectedWeb.Context.ExecuteQueryRetry();

                            users.AddRange(GetPermissions(list.RoleAssignments, url));

                            // if the list with unique permissions also has items, check every item which is uniquely permissioned
                            if (list.ItemCount > 0)
                            {
                                WriteVerbose(string.Format("Enumerating through all listitems of {0}", list.RootFolder.ServerRelativeUrl));

                                CamlQuery query = CamlQuery.CreateAllItemsQuery();
                                var queryElement = XElement.Parse(query.ViewXml);

                                var rowLimit = queryElement.Descendants("RowLimit").FirstOrDefault();
                                if (rowLimit != null)
                                {
                                    rowLimit.RemoveAll();
                                }
                                else
                                {
                                    rowLimit = new XElement("RowLimit");
                                    queryElement.Add(rowLimit);
                                }

                                rowLimit.SetAttributeValue("Paged", "TRUE");
                                rowLimit.SetValue(1000);

                                query.ViewXml = queryElement.ToString();

                                List<ListItemCollection> items = new List<ListItemCollection>();

                                do
                                {
                                    var listItems = list.GetItems(query);
                                    SelectedWeb.Context.Load(listItems);
                                    SelectedWeb.Context.ExecuteQueryRetry();
                                    query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;

                                    items.Add(listItems);

                                } while (query.ListItemCollectionPosition != null);

                                // Progress bar for item enumerations
                                var itemProgress = new ProgressRecord(0, $"Getting items for {list.RootFolder.ServerRelativeUrl}", "Enumerating through items");
                                var itemProgressCounter = 0;

                                foreach (var item in items)
                                {
                                    WriteProgress(itemProgress, $"Retrieving items", itemProgressCounter++, items.Count);

                                    WriteVerbose(string.Format("Enumerating though listitemcollections"));
                                    foreach (var listItem in item)
                                    {
                                        WriteVerbose(string.Format("Enumerating though listitems"));
                                        listItem.EnsureProperty(i => i.HasUniqueRoleAssignments);

                                        if (listItem.HasUniqueRoleAssignments)
                                        {
                                            string listItemUrl = listItem["FileRef"].ToString();
                                            WriteVerbose(string.Format("List item {0} HasUniqueRoleAssignments", listItemUrl));

                                            SelectedWeb.Context.Load(listItem.RoleAssignments, r => r.Include(
                                                ra => ra.RoleDefinitionBindings,
                                                ra => ra.Member.LoginName,
                                                ra => ra.Member.Title,
                                                ra => ra.Member.PrincipalType));
                                            SelectedWeb.Context.ExecuteQueryRetry();

                                            users.AddRange(GetPermissions(listItem.RoleAssignments, listItemUrl));
                                        }
                                    }
                                }
                                itemProgress.RecordType = ProgressRecordType.Completed;
                                WriteProgress(itemProgress);
                            }
                        }
                        progress.RecordType = ProgressRecordType.Completed;
                        WriteProgress(progress);
                    }

                    // Fetch all the unique users from everything that has been collected
                    var uniqueUsers = (from u in users
                                       select u.User.LoginName).Distinct();

                    // Looping through each user, getting all the details like specific permissions and groups an user belongs to
                    foreach (var uniqueUser in uniqueUsers)
                    {
                        // Getting all the assigned permissions per user
                        var userPermissions = (from u in users
                                           where u.User.LoginName == uniqueUser && u.Permissions != null
                                           select u).ToList();

                        // Making the permissions readable by getting the name of the permission and the URL of the artifact
                        Dictionary<string, string> Permissions = new Dictionary<string, string>();
                        foreach (var userPermission in userPermissions)
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (var permissionMask in userPermission.Permissions)
                            {
                                stringBuilder.Append(permissionMask);
                            }
                            Permissions.Add(userPermission.Url, stringBuilder.ToString());
                        }

                        // Getting all the groups where the user is added to
                        var groupsMemberships = (from u in users
                                      where u.User.LoginName == uniqueUser && u.Groups != null
                                      select u.Groups).ToList();

                        // Getting the titles of the all the groups
                        List<string> Groups = new List<string>();
                        foreach (var groupMembership in groupsMemberships)
                        {
                            foreach (var group in groupMembership)
                            {
                                Groups.Add(group.Title);
                            }
                        }
                     
                        // Getting the User object of the user so we can get to the title, loginname, etc
                        var userInformation = (from u in users
                                               where u.User.LoginName == uniqueUser
                                               select u.User).FirstOrDefault();

                        WriteObject(new { userInformation.Title, userInformation.LoginName, userInformation.Email, Groups, Permissions }, true);
                    }
                }
#endif
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
                if (user != null)
                {
                    SelectedWeb.Context.Load(user, retrievalExpressions);
                    SelectedWeb.Context.ExecuteQueryRetry();
                }
                WriteObject(user);
            }
        }

#if !ONPREMISES
        private void WriteProgress(ProgressRecord record, string message, int step, int count)
        {
            var percentage = Convert.ToInt32((100 / Convert.ToDouble(count)) * Convert.ToDouble(step));
            record.StatusDescription = message;
            record.PercentComplete = percentage;
            record.RecordType = ProgressRecordType.Processing;
            WriteProgress(record);
        }

        private static List<DetailedUser> GetPermissions(RoleAssignmentCollection roleAssignments, string url)
        {
            List<DetailedUser> users = new List<DetailedUser>();
            foreach (var roleAssignment in roleAssignments)
            {
                if (roleAssignment.Member.PrincipalType == Microsoft.SharePoint.Client.Utilities.PrincipalType.User)
                {
                    var detailedUser = new DetailedUser();
                    detailedUser.Url = url;
                    detailedUser.User = roleAssignment.Member as User;
                    detailedUser.Permissions = new List<string>();

                    foreach (var roleDefinition in roleAssignment.RoleDefinitionBindings)
                    {
                        if (roleDefinition.Name == "Limited Access")
                            continue;

                        detailedUser.Permissions.Add(roleDefinition.Name);
                    }

                    // if no permissions are recorded (hence, limited access, skip the adding of the permissions)
                    if (detailedUser.Permissions.Count == 0)
                        continue;

                    users.Add(detailedUser);
                }
            }
            return users;
        }
#endif
    }
}
