using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Diagnostic
{
    public sealed class ResponseTimeStatistics
    {
        public MeasureResponseTimeMode Mode { get; set; }
        public double Average { get; set; }
        public long Max { get; set; }
        public long Min { get; set; }
        public double StandardDeviation { get; set; }
        public double TruncatedAverage { get; set; }
        public Dictionary<long, int> Histogram { get; set; }
        public long Count { get; set; }
    }
}
