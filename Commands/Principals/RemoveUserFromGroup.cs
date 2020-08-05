using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Remove, "PnPUserFromGroup")]
    [CmdletHelp("Removes a user from a group",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPUserFromGroup -LoginName user@company.com -GroupName 'Marketing Site Members'",
        SortOrder = 1,
        Remarks = @"Removes the user user@company.com from the Group 'Marketing Site Members'")]
    public class RemoveUserFromGroup : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "A valid login name of a user (user@company.com)")]
        [Alias("LogonName")]
        public string LoginName = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "A group object, an ID or a name of a group")]
        [Alias("GroupName")]
        public GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);
            try
            {
                User user = SelectedWeb.SiteUsers.GetByEmail(LoginName);
                ClientContext.Load(user);
                ClientContext.ExecuteQueryRetry();
                SelectedWeb.RemoveUserFromGroup(group, user);
            }
            catch
            {
                User user = SelectedWeb.SiteUsers.GetByLoginName(LoginName);
                ClientContext.Load(user);
                ClientContext.ExecuteQueryRetry();
                SelectedWeb.RemoveUserFromGroup(group, user);
            }
        }
    }
}
