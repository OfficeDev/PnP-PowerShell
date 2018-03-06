using System.Management.Automation;
using System.Net;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPStoredCredential")]
    [CmdletHelp("Get a credential",
#if !NETSTANDARD2_0
        "Returns a stored credential from the Windows Credential Manager",
#else
        "Returns a stored credential from the Windows Credential Manager or the MacOS KeyChain",
#endif
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(Code = "PS:> Get-PnPStoredCredential -Name O365",
        Remarks = "Returns the credential associated with the specified identifier",
        SortOrder = 1)]
#if !NETSTANDARD2_0
    [CmdletExample(Code = "PS:> Get-PnPStoredCredential -Name testEnvironment -Type OnPrem",
        Remarks = "Gets the credential associated with the specified identifier from the credential manager and then will return a credential that can be used for on-premises authentication",
        SortOrder = 2)]
#endif
    public class GetStoredCredential : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The credential to retrieve.")]
        public string Name;

#if !NETSTANDARD2_0
        [Parameter(Mandatory = false, HelpMessage = "The object type of the credential to return from the Credential Manager. Possible valus are 'O365', 'OnPrem' or 'PSCredential'")]
        public CredentialType Type = CredentialType.O365;
#endif

        protected override void ProcessRecord()
        {
#if !NETSTANDARD2_0
            var cred = Utilities.CredentialManager.GetCredential(Name);
            if (cred != null)
            {
                switch (Type)
                {
                    case CredentialType.O365:
                        {
                            if (cred != null)
                            {
                                WriteObject(new SharePointOnlineCredentials(cred.UserName, cred.Password));
                            }
                            break;
                        }
                    case CredentialType.OnPrem:
                        {
                            WriteObject(new NetworkCredential(cred.UserName, cred.Password));
                            break;
                        }
                    case CredentialType.PSCredential:
                        {
                            WriteObject(cred);
                            break;
                        }
                }
            }
#else
            var creds = Utilities.CredentialManager.GetCredential(Name);
            if(creds != null)
            {
                var spoCreds = new Microsoft.SharePoint.Client.SharePointOnlineCredentials(creds.UserName, creds.Password);
                WriteObject(spoCreds);
            } else
            {
                WriteError(new ErrorRecord(new System.Exception("Credentials not found"), "CREDSNOTFOUND", ErrorCategory.AuthenticationError, this));
            }
#endif
        }
    }
}
