using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Collections.Generic;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteCollectionAdmin")]
    [CmdletHelp("Adds one or more users as site collection administrators to the site collection in the current context",
        DetailedDescription = "This command allows adding one to many users as site collection administrators to the site collection in the current context. It does not replace or remove exisitng site collection administrators.",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
      Code = @"PS:> Add-PnPSiteCollectionAdmin -Owners ""user@contoso.onmicrosoft.com""",
      Remarks = @"This will add user@contoso.onmicrosoft.com as an additional site collection owner to the site collection in the current context", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Add-PnPSiteCollectionAdmin -Owners @(""user1@contoso.onmicrosoft.com"", ""user2@contoso.onmicrosoft.com"")",
      Remarks = @"This will add user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as additional site collection owners to the site collection in the current context", SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Get-PnPUser | ? Title -Like ""*Doe"" | Add-PnPSiteCollectionAdmin",
      Remarks = @"This will add all users with their title ending with ""Doe"" as additional site collection owners to the site collection in the current context", SortOrder = 3)]
    public class AddSiteCollectionAdmin : PnPCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Specifies owner(s) to add as site collection adminstrators. They will be added as additional site collection administrators to the site in the current context. Existing administrators will stay. Can be both users and groups.")]
        public List<UserPipeBind> Owners;

        protected override void ExecuteCmdlet()
        {
            foreach (var owner in Owners)
            {
                User user = null;
                if (owner.Id > 0)
                {
                    WriteVerbose($"Adding user with Id \"{owner.Id}\" as site collection administrator");
                    user = ClientContext.Web.GetUserById(owner.Id);
                }
                else if (owner.User != null && owner.User.Id > 0)
                {
                    WriteVerbose($"Adding user provided in pipeline as site collection administrator");
                    user = owner.User;

                }
                else if (!string.IsNullOrWhiteSpace(owner.Login))
                {
                    WriteVerbose($"Adding user with loginname \"{owner.Login}\" as site collection administrator");
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
                    user.IsSiteAdmin = true;
                    user.Update();

                    try
                    {
                        ClientContext.ExecuteQueryRetry();
                    }
                    catch (ServerException e)
                    {
                        WriteWarning($"Exception occurred while trying to add the user: \"{e.Message}\". User will be skipped.");
                    }
                }
                else
                {
                    WriteWarning($"Unable to add user as it wasn't found. User will be skipped.");
                }
            }
        }
    }
}