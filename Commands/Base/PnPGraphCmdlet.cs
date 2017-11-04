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
                if (SPOnlineConnection.AuthenticationResult != null)
                {
                    if (SPOnlineConnection.AuthenticationResult.ExpiresOn < DateTimeOffset.Now)
                    {
                        WriteWarning(Resources.MicrosoftGraphOAuthAccessTokenExpired);
                        SPOnlineConnection.AuthenticationResult = null;
                        return (null);
                    }
                    else
                    {
                        return (SPOnlineConnection.AuthenticationResult.Token);
                    }
                }
                else
                {
                    ThrowTerminatingError(new ErrorRecord(new InvalidOperationException(Resources.NoAzureADAccessToken), "NO_OAUTH_TOKEN", ErrorCategory.ConnectionError, null));
                    return (null);
                }
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (SPOnlineConnection.AuthenticationResult == null || 
                String.IsNullOrEmpty(SPOnlineConnection.AuthenticationResult.Token))
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
