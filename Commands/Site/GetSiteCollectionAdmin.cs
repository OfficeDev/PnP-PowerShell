using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteCollectionAdmin")]
    [CmdletHelp("Returns the current site collection administrators of the site colleciton in the current context",
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

            ClientContext.Load(SelectedWeb.SiteUsers, users => users.Include(retrievalExpressions));
            ClientContext.ExecuteQueryRetry();

            var siteCollectionAdminUsers = SelectedWeb.SiteUsers.Where(su => su.IsSiteAdmin);
            WriteObject(siteCollectionAdminUsers, true);
        }
    }
}