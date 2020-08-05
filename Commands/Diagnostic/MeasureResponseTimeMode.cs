using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Diagnostic
{
    [Flags]
    public enum MeasureResponseTimeMode
    {
        Undefined = 0,
        RoundTrip = 1,
        SPRequestDuration = 2,
        Latency = 4,
        All = RoundTrip | SPRequestDuration | Latency
    }
}
