using SharePointPnP.PowerShell.Commands.Properties;
using System;
using System.Management.Automation;

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
                    }
                    else
                    {
                        return PnPAzureADConnection.AuthenticationResult.Token;
                    }
                }
                else if (PnPAzureADConnection.ADALAuthenticationResult != null)
                {
                    if (PnPAzureADConnection.ADALAuthenticationResult.ExpiresOn < DateTimeOffset.Now)
                    {
                        WriteWarning(Resources.MicrosoftGraphOAuthAccessTokenExpired);
                        PnPAzureADConnection.AuthenticationResult = null;
                    }
                    else
                    {
                        return PnPAzureADConnection.ADALAuthenticationResult.AccessToken;
                    }
                }
                else
                {
                    ThrowTerminatingError(new ErrorRecord(new InvalidOperationException(Resources.NoAzureADAccessToken), "NO_OAUTH_TOKEN", ErrorCategory.ConnectionError, null));
                }
                return null;
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if ((PnPAzureADConnection.AuthenticationResult == null || String.IsNullOrEmpty(PnPAzureADConnection.AuthenticationResult.Token))
                && (PnPAzureADConnection.ADALAuthenticationResult == null || String.IsNullOrEmpty(PnPAzureADConnection.ADALAuthenticationResult.AccessToken)))
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
