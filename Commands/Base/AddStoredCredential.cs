using System.Management.Automation;
using System.Security;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Add, "PnPStoredCredential")]
#if !PNPPSCORE
    [CmdletHelp("Adds a credential to the Windows Credential Manager",
        @"Adds an entry to the Windows Credential Manager. If you add an entry in the form of the URL of your tenant/server PnP PowerShell will check if that entry is available when you connect using Connect-PnPOnline. If it finds a matching URL it will use the associated credentials.

If you add a Credential with a name of ""https://yourtenant.sharepoint.com"" it will find a match when you connect to ""https://yourtenant.sharepoint.com"" but also when you connect to ""https://yourtenant.sharepoint.com/sites/demo1"". Of course you can specify more granular entries, allow you to automatically provide credentials for different URLs.",
        Category = CmdletHelpCategory.Base)]
#else
    [CmdletHelp("Adds a credential to the Windows Credential Manager/MacOS KeyChain",
        @"Adds an entry to the Windows Credential Manager or the MacOS KeyChain. If you add an entry in the form of the URL of your tenant/server PnP PowerShell will check if that entry is available when you connect using Connect-PnPOnline. If it finds a matching URL it will use the associated credentials.

If you add a Credential with a name of ""https://yourtenant.sharepoint.com"" it will find a match when you connect to ""https://yourtenant.sharepoint.com"" but also when you connect to ""https://yourtenant.sharepoint.com/sites/demo1"". Of course you can specify more granular entries, allow you to automatically provide credentials for different URLs.",
        Category = CmdletHelpCategory.Base)]
#endif
    [CmdletExample(Code = "PS:> Add-PnPStoredCredential -Name https://tenant.sharepoint.com -Username yourname@tenant.onmicrosoft.com",
        Remarks = "You will be prompted to specify the password and a new entry will be added with the specified values",
        SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Add-PnPStoredCredential -Name https://tenant.sharepoint.com -Username yourname@tenant.onmicrosoft.com -Password (ConvertTo-SecureString -String ""YourPassword"" -AsPlainText -Force)",
        Remarks = "A new entry will be added with the specified values",
        SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Add-PnPStoredCredential -Name https://tenant.sharepoint.com -Username yourname@tenant.onmicrosoft.com -Password (ConvertTo-SecureString -String ""YourPassword"" -AsPlainText -Force)
Connect-PnPOnline -Url https://tenant.sharepoint.com/sites/mydemosite",
        Remarks = "A new entry will be added with the specified values, and a subsequent connection to a sitecollection starting with the entry name will be made. Notice that no password prompt will occur.",
        SortOrder = 3)]
    public class AddStoredCredential : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The credential to set")]
        public string Name;

        [Parameter(Mandatory = true)]
        public string Username;

        [Parameter(Mandatory = false, HelpMessage = @"If not specified you will be prompted to enter your password. 
If you want to specify this value use ConvertTo-SecureString -String 'YourPassword' -AsPlainText -Force")]
        public SecureString Password;
#if PNPPSCORE
        [Parameter(Mandatory = false)]
        public SwitchParameter Overwrite;
#endif
        protected override void ProcessRecord()
        {
            if(Password == null || Password.Length == 0)
            {
                Host.UI.Write("Enter password: ");
                Password = Host.UI.ReadLineAsSecureString();
            } 
            
#if !PNPPSCORE
            Utilities.CredentialManager.AddCredential(Name, Username, Password);
#else
            Utilities.CredentialManager.AddCredential(Name, Username, Password, Overwrite.ToBool());
#endif
        }
    }
}