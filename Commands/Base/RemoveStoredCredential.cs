using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Remove, "PnPStoredCredential")]
    [CmdletHelp("Removes a credential",
#if !NETSTANDARD2_0
        "Removes a stored credential from the Windows Credential Manager",
#else
        "Removes a stored credential from the Windows Credential Manager or the MacOS KeyChain",
#endif
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(Code = "PS:> Remove-PnPStoredCredential -Name https://tenant.sharepoint.com",
#if !NETSTANDARD2_0
        Remarks = "Removes the specified credential from the Windows Credential Manager",
#else
          Remarks = "Removes the specified credential from the Windows Credential Manager or the MacOS KeyChain",
#endif
        SortOrder = 1)]
    public class RemoveStoredCredential : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The credential to remove")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "If specified you will not be asked for confirmation")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var cred = Utilities.CredentialManager.GetCredential(Name);
            if (cred != null)
            {
                if (Force || ShouldContinue($"Remove credential {Name}?", "Confirm"))
                {
                    if (!Utilities.CredentialManager.RemoveCredential(Name))
                    {
                        WriteError(new ErrorRecord(new System.Exception($"Credential {Name} not removed"), "CREDENTIALNOTREMOVED", ErrorCategory.WriteError, Name));
                    }
                }
            }
            else
            {
                WriteError(new ErrorRecord(new System.Exception($"Credential {Name} not found"), "CREDENTIALNOTFOUND", ErrorCategory.ObjectNotFound, Name));
            }
        }
    }
}
