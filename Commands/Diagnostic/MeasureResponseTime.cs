using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Diagnostic
{
    [Cmdlet(VerbsDiagnostic.Measure, "PnPResponseTime")]
    [CmdletHelp("Gets statistics on response time for the specified endpoint by sending probe requests",
        Category = CmdletHelpCategory.Diagnostic)]
    [CmdletExample(
     Code = @"PS:> Measure-PnPResponseTime -Count 100 -Timeout 20",
     Remarks = @"Calculates statistics on sequence of 100 probe requests, sleeps 20ms between probes",
     SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Measure-PnPResponseTime ""/Pages/Test.aspx"" -Count 1000",
     Remarks = @"Calculates statistics on response time of Test.aspx by sending 1000 requests with default sleep time between requests",
     SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Measure-PnPResponseTime $web -Count 1000 -WarmUp 10 -Histogram 20 -Timeout 50 | Select -expa Histogram | % {$_.GetEnumerator() | Export-Csv C:\Temp\responsetime.csv -NoTypeInformation}",
     Remarks = @"Builds histogram of response time for the home page of the web and exports to CSV for later processing in Excel",
     SortOrder = 3)]
    public class MeasureResponseTime : PnPCmdlet
    {
        private ProgressRecord _progressRecord = new ProgressRecord(0, "Measuring response time", "Sending probe requests");

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public DiagnosticEndpointPipeBind Url;

        [Parameter(Mandatory = false, HelpMessage = "Number of probe requests to send")]
        public uint Count = 20;

        [Parameter(Mandatory = false, HelpMessage = "Number of warm up requests to send before start calculating statistics")]
        public uint WarmUp = 1;

        [Parameter(Mandatory = false, HelpMessage = "Idle timeout between requests to avoid request throttling")]
        public uint Timeout = 500;

        [Parameter(Mandatory = false, HelpMessage = "Number of buckets in histogram in output statistics")]
        public uint Histogram = 5;

        [Parameter(Mandatory = false, HelpMessage = "Response time measurement mode. RoundTrip - measures full request round trip. SPRequestDuration - measures server processing time only, based on SPRequestDuration HTTP header. Latency - difference between RoundTrip and SPRequestDuration")]
        public MeasureResponseTimeMode Mode = MeasureResponseTimeMode.RoundTrip;

        protected override void ExecuteCmdlet()
        {
            var uri = GetEndpointUri();
            Stopwatch timer = new Stopwatch();
            Dictionary<MeasureResponseTimeMode, List<long>> measurements = new Dictionary<MeasureResponseTimeMode, List<long>>();
            foreach(var value in (MeasureResponseTimeMode[])Enum.GetValues(typeof(MeasureResponseTimeMode)))
            {
                if (value != MeasureResponseTimeMode.Undefined &&
                   value != MeasureResponseTimeMode.All &&
                   Mode.Has(value))
                {
                    measurements[value] = new List<long>();
                }
            }

            try
            {
                for (int i = -(int)WarmUp; i < Count; i++)
                {
                    bool isWarmUp = i < 0;
                    var probe = WebRequest.CreateHttp(uri);
                    probe.AllowAutoRedirect = false;

                    ClientRuntimeContext.SetupRequestCredential(ClientContext, probe);
                    WriteProgress(isWarmUp ? $"Warming up: {WarmUp + i + 1}/{WarmUp}": $"Sending: {uri}", i);

                    HttpWebResponse response = null;
                    try
                    {
                        try
                        {
                            timer.Restart();
                            response = probe.GetResponse() as HttpWebResponse;
                            timer.Stop();
                        }
                        catch (WebException e)
                        {
                            timer.Stop();
                            response = e.Response as HttpWebResponse;
                            if (response == null)
                            {
                                throw;
                            }
                        }
                        if (!isWarmUp)
                        {
                            long rountTrip = timer.ElapsedMilliseconds;
                            long spRequestDuration = 0;
                            if (Mode.Has(MeasureResponseTimeMode.SPRequestDuration) || 
                                Mode.Has(MeasureResponseTimeMode.Latency))
                            {
                                //try get request duration from headers
                                spRequestDuration = GetSpRequestDuration(response);
                                if (Mode.Has(MeasureResponseTimeMode.SPRequestDuration))
                                {
                                    measurements[MeasureResponseTimeMode.SPRequestDuration].Add(spRequestDuration);
                                }
                            }
                            if (Mode.Has(MeasureResponseTimeMode.RoundTrip))
                            {
                                measurements[MeasureResponseTimeMode.RoundTrip].Add(rountTrip);
                            }
                            if(Mode.Has(MeasureResponseTimeMode.Latency))
                            {
                                measurements[MeasureResponseTimeMode.Latency].Add(rountTrip - spRequestDuration);
                            }
                        }
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            WriteWarning($"Reply from {uri}: {(int)response.StatusCode}");
                        }
                    }
                    finally
                    {
                        if(response != null)
                        {
                            response.Dispose();
                        }
                    }
                    if (!isWarmUp)
                    {
                        WriteVerbose($"Reply from {uri}: {timer.ElapsedMilliseconds}ms");
                    }
                    WriteProgress($"Sleeping: {Timeout}ms", i);
                    Thread.Sleep((int)Timeout);
                }
                _progressRecord.RecordType = ProgressRecordType.Completed;
                WriteProgress(_progressRecord);

            }
            finally
            {
                List<ResponseTimeStatistics> response = new List<ResponseTimeStatistics>();
                foreach (var value in (MeasureResponseTimeMode[])Enum.GetValues(typeof(MeasureResponseTimeMode)))
                {
                    if(value != MeasureResponseTimeMode.Undefined &&
                       value != MeasureResponseTimeMode.All &&
                       Mode.Has(value))
                    {
                        var statistics = GetStatistics(measurements[value]);
                        statistics.Mode = value;
                        response.Add(statistics);
                    }
                    measurements[value] = new List<long>();
                }

                WriteObject(response, true);
            }
        }

        private long GetSpRequestDuration(HttpWebResponse response)
        {
            long res = 0;
            if ((response != null) && (response.Headers != null))
            {
                var header = response.Headers.Get("SPRequestDuration");
                long.TryParse(header, out res);
            }
            return res;
        }

        private void WriteProgress(string message, int step)
        {
            var total = Count + WarmUp;
            var percentage = total != 0 ? Convert.ToInt32((100 / Convert.ToDouble(total)) * Convert.ToDouble(step)) : 100;
            _progressRecord.StatusDescription = message;
            _progressRecord.PercentComplete = percentage;
            _progressRecord.RecordType = ProgressRecordType.Processing;
            WriteProgress(_progressRecord);
        }

        private ResponseTimeStatistics GetStatistics(IEnumerable<long> array)
        {
            double average = 0;
            long max = 0;
            long min = 0;
            int count = 0;
            double truncatedAverage = 0;
            double standardDeviation = 0;
            Dictionary<long, int> histogram = new Dictionary<long, int>();

            if (array != null && array.Count() > 0)
            {
                count = array.Count();
                array = array.OrderBy(a => a);
                min = array.First();
                max = array.Last();
                average = array.Average();
                double sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
                standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / count);

                if (count > 10)
                {
                    var trunc = (int)Math.Round(count * 0.1);
                    truncatedAverage = array.Skip(trunc).Take(count - 2 * trunc).Average();
                }

                if(Histogram > 0)
                {
                    //build histogram
                    var bucketSize = max/(double)Histogram;
                    for (int i = 0; i < Histogram; i++)
                    {
                        var bucketMin = bucketSize * i;
                        var bucketMax = bucketSize * (i + 1);
                        histogram[(long)bucketMax] = array.Where(a => 
                        a >= bucketMin && (a <= bucketMax || (i == Histogram - 1))).Count();
                    }
                }
            }
            return new ResponseTimeStatistics
            {
                Average = Math.Round(average, 2),
                Max = max,
                Min = min,
                StandardDeviation = Math.Round(standardDeviation, 2),
                TruncatedAverage = truncatedAverage,
                Count = count,
                Histogram = Histogram > 0 ? histogram : null
            };
        }

        private Uri GetEndpointUri()
        {
            if (Url == null)
            {
                Url = new DiagnosticEndpointPipeBind(ClientContext.Web);
            }
            var uri = new Uri(Url.ToString(), UriKind.Relative);
            var authority = new Uri(ClientContext.Url).GetLeftPart(UriPartial.Authority);
            uri = new Uri(authority + uri.ToString(), UriKind.Absolute);
            return uri;
        }
    }
}
