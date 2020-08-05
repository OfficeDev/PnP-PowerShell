#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPRequestAccessEmails")]
    [CmdletHelp("Sets Request Access Email on a web",
       DetailedDescription = "Enables or disables access requests to be sent and configures which e-mail address should receive these requests. The web you apply this on must have unique rights.",
       SupportedPlatform = CmdletSupportedPlatform.Online,
       Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
       Code = @"PS:> Set-PnPRequestAccessEmails -Emails someone@example.com ",
       Remarks = "This will enable requesting access and send the requests to the provided e-mail address",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Set-PnPRequestAccessEmails -Disabled",
       Remarks = "This will disable the ability to request access to the site",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Set-PnPRequestAccessEmails -Disabled:$false",
       Remarks = "This will enable the ability to request access to the site and send the requests to the default owners of the site",
       SortOrder = 3)]
    public class SetRequestAccessEmails : PnPWebCmdlet
    {
        // Parameter must remain a string array for backwards compatibility, even though only one e-mail address can be provided
        [Parameter(Mandatory = false, HelpMessage = "Email address to send the access requests to")]
        public string[] Emails = null;

        [Parameter(Mandatory = false, HelpMessage = "Enables or disables access to be requested")]
        public SwitchParameter Disabled;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.HasUniqueRoleAssignments);

            // Can only set the Request Access Emails if the web has unique permissions
            if (SelectedWeb.HasUniqueRoleAssignments)
            {
                if (Emails != null && Emails.Length > 0 && !Disabled)
                {
                    if (Emails.Length > 1)
                    {
                        // Only one e-mail address can be configured to receive the access requests
                        throw new ArgumentException(Resources.SetRequestAccessEmailsOnlyOneAddressAllowed, nameof(Emails));
                    }
                    else
                    {
                        // Configure the one e-mail address to receive the access requests
                        SelectedWeb.SetUseAccessRequestDefaultAndUpdate(false);
                        SelectedWeb.EnableRequestAccess(Emails[0]);
                    }
                }
                else
                {
                    if (Disabled)
                    {
                        // Disable requesting access
                        SelectedWeb.DisableRequestAccess();
                    }
                    else
                    {
                        // Enable requesting access and set it to the default owners group
                        // Code can be replaced by SelectedWeb.EnableRequestAccess(); once https://github.com/SharePoint/PnP-Sites-Core/pull/2533 has been accepted for merge.
                        SelectedWeb.SetUseAccessRequestDefaultAndUpdate(true);
                        SelectedWeb.Update();
                        SelectedWeb.Context.ExecuteQueryRetry();
                    }
                }
            }
        }
    }
}
#endif
