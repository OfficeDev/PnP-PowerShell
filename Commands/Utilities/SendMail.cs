using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    [Cmdlet(VerbsCommunications.Send, "PnPMail")]
    [CmdletAlias("Send-SPOMail")]
    [CmdletHelp("Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.",
        Category = CmdletHelpCategory.Utilities)]
    [CmdletExample(
        Code = @"PS:> Send-PnPMail -To address@tenant.sharepointonline.com -Subject test -Body test",
        Remarks = @"Sends an e-mail using the SharePoint SendEmail method using the current context. E-mail is sent from the system account and can only be sent to accounts in the same tenant", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@tenant.onmicrosoft.com -Password xyz",
        Remarks = @"Sends an e-mail via Office 365 SMTP and requires a from address and password. E-mail is sent from the from user and can be sent to both internal and external addresses.", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@server.net -Password xyz -Server yoursmtp.server.net",
        Remarks = @"Sends an e-mail via a custom SMTP server and requires a from address and password. E-mail is sent from the from user.", SortOrder = 3)]
    public class SendMail : SPOWebCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Server = "smtp.office365.com";

        [Parameter(Mandatory = false, HelpMessage = "If using from address, you also have to provide a password")]
        public string From;

        [Parameter(Mandatory = false, HelpMessage = "If using a password, you also have to provide the associated from address")]
        public string Password;

        [Parameter(Mandatory = true, HelpMessage = @"List of recipients")]
        public string[] To;

        [Parameter(Mandatory = false, HelpMessage = @"List of recipients on CC")]
        public string[] Cc;

        [Parameter(Mandatory = true, HelpMessage = @"Subject of the email")]
        public string Subject;

        [Parameter(Mandatory = true, HelpMessage = @"Body of the email")]
        public string Body;
        
        protected override void ExecuteCmdlet()
        {
            if (string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(From))
            {
                MailUtility.SendEmail(ClientContext, To, Cc, Subject, Body);
            }
            else
            {
                MailUtility.SendEmail(Server, From, Password, To, Cc, Subject, Body);
            }
        }
    }
}
