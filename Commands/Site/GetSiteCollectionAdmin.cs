using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteCollectionAdmin")]
    [CmdletHelp("Returns the current site collection administrators of the site collection in the current context",
        DetailedDescription = "This command will return all current site collection administrators of the site collection in the current context",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
      Code = @"PS:> Get-PnPSiteCollectionAdmin",
      Remarks = @"This will return all the current site collection administrators of the site collection in the current context", SortOrder = 1)]
    public class GetSiteCollectionAdmin : PnPWebCmdlet
    {
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

            var siteCollectionAdminUsersQuery = SelectedWeb.SiteUsers.Where(u => u.IsSiteAdmin);
            var siteCollectionAdminUsers = ClientContext.LoadQuery(siteCollectionAdminUsersQuery.Include(retrievalExpressions));
            ClientContext.ExecuteQueryRetry();

            WriteObject(siteCollectionAdminUsers, true);
        }
    }
}
