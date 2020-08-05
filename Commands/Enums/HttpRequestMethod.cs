using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Enums
{
    public enum HttpRequestMethod
    {
        Default = 0,
        Get = 1,
        Head = 2,
        Post = 3,
        Put = 4,
        Delete = 5,
        Trace = 6,
        Options = 7,
        Merge = 8,
        Patch = 9
    }
}
