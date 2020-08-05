using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = false)]
    public sealed class CmdletPipelineAttribute : Attribute
    {
        public string Description { get; set; }
        public Type Type { get; set; }
    }
}
