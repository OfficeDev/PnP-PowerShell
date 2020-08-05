using System;
using System.Management.Automation;
using System.Threading;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.Commands.Base;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    /// <summary>
    /// Base class for all the PnP SharePoint related cmdlets
    /// </summary>
    public class PnPSharePointCmdlet : PnPConnectedCmdlet
    {
        /// <summary>
        /// Reference the the SharePoint context on the current connection. If NULL it means there is no SharePoint context available on the current connection.
        /// </summary>
        public ClientContext ClientContext => Connection?.Context ?? PnPConnection.CurrentConnection.Context;

        [Parameter(Mandatory = false, HelpMessage = "Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.")] // do not remove '#!#99'
        [PnPParameter(Order = 99)]
        public PnPConnection Connection = null;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (PnPConnection.CurrentConnection != null && PnPConnection.CurrentConnection.TelemetryClient != null)
            {
                PnPConnection.CurrentConnection.TelemetryClient.TrackEvent(MyInvocation.MyCommand.Name);
            }

            if (Connection == null && ClientContext == null)
            {
                throw new InvalidOperationException(Resources.NoSharePointConnection);
            }
        }

        protected override void ProcessRecord()
        {
            try
            {
                if (PnPConnection.CurrentConnection.MinimalHealthScore.HasValue && PnPConnection.CurrentConnection.MinimalHealthScore.Value >= 0)
                {
                    int healthScore = Utility.GetHealthScore(PnPConnection.CurrentConnection.Url);
                    if (healthScore <= PnPConnection.CurrentConnection.MinimalHealthScore.Value)
                    {
                        ExecuteCmdlet();
                    }
                    else
                    {
                        if (PnPConnection.CurrentConnection.RetryCount != -1)
                        {
                            int retry = 1;
                            while (retry <= PnPConnection.CurrentConnection.RetryCount)
                            {
                                WriteWarning(string.Format(Resources.Retry0ServerNotHealthyWaiting1seconds, retry, PnPConnection.CurrentConnection.RetryWait, healthScore));
                                Thread.Sleep(PnPConnection.CurrentConnection.RetryWait * 1000);
                                healthScore = Utility.GetHealthScore(PnPConnection.CurrentConnection.Url);
                                if (healthScore <= PnPConnection.CurrentConnection.MinimalHealthScore.Value)
                                {
                                    var tag = PnPConnection.CurrentConnection.PnPVersionTag + ":" + MyInvocation.MyCommand.Name.Replace("SPO", "");
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
                            if (retry > PnPConnection.CurrentConnection.RetryCount)
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
                    var tag = PnPConnection.CurrentConnection.PnPVersionTag + ":" + MyInvocation.MyCommand.Name.Replace("SPO", "");
                    if (tag.Length > 32)
                    {
                        tag = tag.Substring(0, 32);
                    }
                    ClientContext.ClientTag = tag;

                    ExecuteCmdlet();
                }
            }
            catch (PipelineStoppedException)
            {
                //don't swallow pipeline stopped exception
                //it makes select-object work weird
                throw;
            }
            catch (Exception ex)
            {
                PnPConnection.CurrentConnection.RestoreCachedContext(PnPConnection.CurrentConnection.Url);
                ex.Data.Add("CorrelationId", PnPConnection.CurrentConnection.Context.TraceCorrelationId);
                ex.Data.Add("TimeStampUtc", DateTime.UtcNow);
                var errorDetails = new ErrorDetails(ex.Message);
                
                errorDetails.RecommendedAction = "Use Get-PnPException for more details.";
                var errorRecord = new ErrorRecord(ex, "EXCEPTION", ErrorCategory.WriteError, null);
                errorRecord.ErrorDetails = errorDetails;

                WriteError(errorRecord);
            }
        }
        
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
