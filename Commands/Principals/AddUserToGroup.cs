using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Add, "PnPUserToGroup")]
    [CmdletHelp("Adds a user to a SharePoint group", 
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Add-PnPUserToGroup -LoginName user@company.com -Identity 'Marketing Site Members'",
		Remarks = @"Add the specified user to the SharePoint group ""Marketing Site Members""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPUserToGroup -LoginName user@company.com -Identity 5",
        Remarks = "Add the specified user to the SharePoint group with Id 5",
        SortOrder = 2)]
    public class AddUserToGroup : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "The login name of the user", ParameterSetName = "Internal")]
        public string LoginName;

        [Parameter(Mandatory = true, HelpMessage = "The SharePoint group id, SharePoint group name or SharePoint group object to add the user to", ValueFromPipeline = true, ParameterSetName = "Internal")]
#if !ONPREMISES
        [Parameter(Mandatory = true, HelpMessage = "The SharePoint group id, SharePoint group name or SharePoint group object to add the user to", ValueFromPipeline = true, ParameterSetName = "External")]
#endif
        public GroupPipeBind Identity;

#if !ONPREMISES
        [Parameter(Mandatory = true, HelpMessage = "The email address of the user", ParameterSetName = "External")]
        public string EmailAddress;

        [Parameter(Mandatory = false, ParameterSetName = "External")]
        public SwitchParameter SendEmail;

        [Parameter(Mandatory = false, ParameterSetName = "External")]
        public string EmailBody = "Site shared with you.";
#endif
        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);
#if !ONPREMISES
            if (ParameterSetName == "External")
            {
                group.InviteExternalUser(EmailAddress, SendEmail, EmailBody);
            }
            else
            {
                SelectedWeb.AddUserToGroup(group, LoginName);
            }
#else
            SelectedWeb.AddUserToGroup(group, LoginName);
#endif
        }
    }
}
