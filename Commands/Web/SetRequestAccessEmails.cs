#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPRequestAccessEmails")]
    [CmdletAlias("Set-SPORequestAccessEmails")]
    [CmdletHelp("Sets Request Access Emails on a web",
       Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
       Code = @"PS:> Set-PnPRequestAccessEmails -Emails someone@example.com ",
       Remarks = "This will update the request access e-mail address",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Set-PnPRequestAccessEmails -Emails @( someone@example.com; someoneelse@example.com )",
       Remarks = "This will update multiple request access e-mail addresses",
       SortOrder = 2)]
    public class SetRequestAccessEmails : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Email address(es) to set the RequestAccessEmails to")]
        public string[] Emails = null;

        protected override void ExecuteCmdlet()
        {
            if (Emails != null && Emails.Length > 0)
            {
                SelectedWeb.EnsureProperty(w => w.HasUniqueRoleAssignments);

                // Can only set the Request Access Emails if the web has unique permissions
                if (SelectedWeb.HasUniqueRoleAssignments)
                {
                    SelectedWeb.EnableRequestAccess(Emails);
                }                
            }
        }
    }
}
#endif
