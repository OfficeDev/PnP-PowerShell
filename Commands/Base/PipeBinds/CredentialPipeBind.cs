using System.Management.Automation;
using SharePointPnP.PowerShell.Commands.Utilities;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class CredentialPipeBind
    {
        private readonly PSCredential _pscredential;
        private readonly string _storedcredential;

        public CredentialPipeBind(PSCredential pscredential)
        {
            _pscredential = pscredential;
        }

        public CredentialPipeBind(string id)
        {
            _storedcredential = id;
        }

        public PSCredential Credential
        {
            get
            {
                if (_pscredential != null)
                {
                    return _pscredential;
                }
                else if (_storedcredential != null)
                {
                    return CredentialManager.GetCredential(_storedcredential);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
