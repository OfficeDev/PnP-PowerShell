using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Collections.Generic;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Remove, "PnPSiteCollectionAdmin")]
    [CmdletHelp("Removes one or more users as site collection administrators from the site collection in the current context",
        DetailedDescription = "This command allows removing one to many users as site collection administrators from the site collection in the current context. All existing site collection administrators not included in this command will remain site collection administrator.",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSiteCollectionAdmin -Owners ""user@contoso.onmicrosoft.com""",
        Remarks = @"This will remove user@contoso.onmicrosoft.com as a site collection owner from the site collection in the current context", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSiteCollectionAdmin -Owners @(""user1@contoso.onmicrosoft.com"", ""user2@contoso.onmicrosoft.com"")",
        Remarks = @"This will remove user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as site collection owners from the site collection in the current context", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUser | ? Title -Like ""*Doe"" | Remove-PnPSiteCollectionAdmin",
        Remarks = @"This will remove all users with their title ending with ""Doe"" as site collection owners from the site collection in the current context", SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteCollectionAdmin | Remove-PnPSiteCollectionAdmin",
        Remarks = @"This will remove all existing site collection administrators from the site collection in the current context", SortOrder = 4)]
    public class RemoveSiteCollectionAdmin : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Specifies owner(s) to remove as site collection administrators. Can be both users and groups.")]
        public List<UserPipeBind> Owners;

        protected override void ExecuteCmdlet()
        {
            foreach (var owner in Owners)
            {
                User user = null;
                if (owner.Id > 0)
                {
                    WriteVerbose($"Removing user with Id \"{owner.Id}\" as site collection administrator");
                    user = ClientContext.Web.GetUserById(owner.Id);
                }
                else if (owner.User != null && owner.User.Id > 0)
                {
                    WriteVerbose($"Removing user provided in pipeline as site collection administrator");
                    user = owner.User;

                }
                else if (!string.IsNullOrWhiteSpace(owner.Login))
                {
                    WriteVerbose($"Removing user with loginname \"{owner.Login}\" as site collection administrator");
                    if (owner.Login.StartsWith("i:"))
                    {
                        user = ClientContext.Web.SiteUsers.GetByLoginName(owner.Login);
                    }
                    else
                    {
                        user = ClientContext.Web.EnsureUser(owner.Login);
                    }
                }
                if (user != null)
                {
                    user.IsSiteAdmin = false;
                    user.Update();

                    try
                    {
                        ClientContext.ExecuteQueryRetry();
                    }
                    catch (ServerException e)
                    {
                        WriteWarning($"Exception occurred while trying to remove the user: \"{e.Message}\". User will be skipped.");
                    }
                }
                else
                {
                    WriteWarning($"Unable to remove user as it wasn't found. User will be skipped.");
                }
            }
        }
    }
}