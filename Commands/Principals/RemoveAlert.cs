#if !SP2013 && !SP2016
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Remove, "PnPAlert")]
    [CmdletHelp("Removes an alert for a user",
        Category = CmdletHelpCategory.Principals,
        SupportedPlatform = CmdletSupportedPlatform.SP2019 | CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPAlert -Identity 641ac67f-0ce0-4837-874a-743c8f8572a7",
        Remarks = @"Removes the alert with the specified ID for the current user",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPAlert -Identity 641ac67f-0ce0-4837-874a-743c8f8572a7 -User ""i:0#.f|membership|Alice@contoso.onmicrosoft.com""",
        Remarks = @"Removes the alert with the specified ID for the user specified",
        SortOrder = 2)]
    public class RemoveAlert : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "User to remove the alert for (User ID, login name or actual User object). Skip this parameter to use the current user. Note: Only site owners can remove alerts for other users.")]
        public UserPipeBind User;

        [Parameter(Mandatory = true, HelpMessage = "The alert id, or the actual alert object to remove.")]
        public AlertPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            User user;
            if (null != User)
            {
                user = User.GetUser(ClientContext);
                if (user == null)
                {
                    throw new ArgumentException("Unable to find user", "Identity");
                }
            }
            else
            {
                user = SelectedWeb.CurrentUser;
            }
            if (!Force)
            {
                user.EnsureProperty(u => u.LoginName);
            }
            if (Force || ShouldContinue($"Remove alert {Identity.Id} for {user.LoginName}?", "Remove alert"))
            {
                user.Alerts.DeleteAlert(Identity.Id);
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
#endif