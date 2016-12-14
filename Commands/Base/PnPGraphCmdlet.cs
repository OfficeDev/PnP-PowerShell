using Microsoft.Graph;
using SharePointPnP.PowerShell.Commands.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base
{
    /// <summary>
    /// Base class for all the PnP Microsoft Graph related cmdlets
    /// </summary>
    public abstract class PnPGraphCmdlet : PSCmdlet
    {
        public String AccessToken
        {
            get
            {
                if (PnPAzureADConnection.AuthenticationResult != null)
                {
                    if (PnPAzureADConnection.AuthenticationResult.ExpiresOn < DateTimeOffset.Now)
                    {
                        WriteWarning(Resources.MicrosoftGraphOAuthAccessTokenExpired);
                        PnPAzureADConnection.AuthenticationResult = null;
                        return (null);
                    }
                    else
                    {
                        return (PnPAzureADConnection.AuthenticationResult.Token);
                    }
                }
                else
                {
                    WriteError(new ErrorRecord(new InvalidOperationException(Resources.NoAzureADAccessToken), "NO_OAUTH_TOKEN", ErrorCategory.ConnectionError, null));
                    return (null);
                }
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (PnPAzureADConnection.AuthenticationResult == null || 
                String.IsNullOrEmpty(PnPAzureADConnection.AuthenticationResult.Token))
            {
                throw new InvalidOperationException(Resources.NoAzureADAccessToken);
            }
        }

        protected virtual void ExecuteCmdlet()
        { }

        protected override void ProcessRecord()
        {
            ExecuteCmdlet();
        }
    }
}
