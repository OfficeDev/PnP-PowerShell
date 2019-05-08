using System;
using System.Management.Automation;
using System.Threading;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.Commands.Base;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace SharePointPnP.PowerShell.Commands
{
    public class PnPCmdlet : PSCmdlet
    {
        public ClientContext ClientContext => Connection?.Context ?? SPOnlineConnection.CurrentConnection.Context;

        [Parameter(Mandatory = false, HelpMessage = "Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.")] // do not remove '#!#99'
        [PnPParameter(Order = 99)]
        public SPOnlineConnection Connection = null;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (SPOnlineConnection.CurrentConnection != null && SPOnlineConnection.CurrentConnection.TelemetryClient != null)
            {
                SPOnlineConnection.CurrentConnection.TelemetryClient.TrackEvent(MyInvocation.MyCommand.Name);
            }

            if (MyInvocation.InvocationName.ToUpper().IndexOf("-SPO", StringComparison.Ordinal) > -1)
            {
                WriteWarning($"PnP Cmdlets starting with the SPO Prefix have been deprecated since the June 2017 release. Please update your scripts and use {MyInvocation.MyCommand.Name} instead.");
            }
            if (SPOnlineConnection.CurrentConnection == null && Connection == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
            if (SPOnlineConnection.CurrentConnection == null && ClientContext == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
            if (SPOnlineConnection.CurrentConnection.ConnectionMethod == Model.ConnectionMethod.GraphDeviceLogin)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
        }

        protected virtual void ExecuteCmdlet()
        { }

        protected override void ProcessRecord()
        {
            try
            {
                if (SPOnlineConnection.CurrentConnection.MinimalHealthScore != -1)
                {
                    int healthScore = Utility.GetHealthScore(SPOnlineConnection.CurrentConnection.Url);
                    if (healthScore <= SPOnlineConnection.CurrentConnection.MinimalHealthScore)
                    {
                        ExecuteCmdlet();
                    }
                    else
                    {
                        if (SPOnlineConnection.CurrentConnection.RetryCount != -1)
                        {
                            int retry = 1;
                            while (retry <= SPOnlineConnection.CurrentConnection.RetryCount)
                            {
                                WriteWarning(string.Format(Resources.Retry0ServerNotHealthyWaiting1seconds, retry, SPOnlineConnection.CurrentConnection.RetryWait, healthScore));
                                Thread.Sleep(SPOnlineConnection.CurrentConnection.RetryWait * 1000);
                                healthScore = Utility.GetHealthScore(SPOnlineConnection.CurrentConnection.Url);
                                if (healthScore <= SPOnlineConnection.CurrentConnection.MinimalHealthScore)
                                {
                                    var tag = SPOnlineConnection.CurrentConnection.PnPVersionTag + ":" + MyInvocation.MyCommand.Name.Replace("SPO", "");
                                    if (tag.Length > 32)
                                    {
                                        tag = tag.Substring(0, 32);
                                    }
                                    ClientContext.ClientTag = tag;


                                    ExecuteCmdlet();
                                    break;
                                }
                                retry++;
                            }
                            if (retry > SPOnlineConnection.CurrentConnection.RetryCount)
                            {
                                ThrowTerminatingError(new ErrorRecord(new Exception(Resources.HealthScoreNotSufficient), "HALT", ErrorCategory.LimitsExceeded, null));
                            }
                        }
                        else
                        {
                            ThrowTerminatingError(new ErrorRecord(new Exception(Resources.HealthScoreNotSufficient), "HALT", ErrorCategory.LimitsExceeded, null));
                        }
                    }
                }
                else
                {
                    var tag = SPOnlineConnection.CurrentConnection.PnPVersionTag + ":" + MyInvocation.MyCommand.Name.Replace("SPO", "");
                    if (tag.Length > 32)
                    {
                        tag = tag.Substring(0, 32);
                    }
                    ClientContext.ClientTag = tag;

                    ExecuteCmdlet();
                }
            }
            catch (System.Management.Automation.PipelineStoppedException)
            {
                //swallow pipeline stopped exception
            }
            catch (Exception ex)
            {
                SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
                WriteError(new ErrorRecord(ex, "EXCEPTION", ErrorCategory.WriteError, null));
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        /// <summary>
        /// Computes additional headers for REST API calls that have to be included during composition of the request
        /// </summary>
        /// <returns><see cref="Dictionary{TKey,TValue}"/> that contains the key and value of the additional headers</returns>
        protected virtual Dictionary<string, string> GetAdditionalHeaders()
        {
            var additionalHeaders = new Dictionary<string, string>();
            if (SPOnlineConnection.CurrentConnection.DisableFormsBasedAuthentication)
            {
                additionalHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
            }

            return additionalHeaders;
        }
    }
}
