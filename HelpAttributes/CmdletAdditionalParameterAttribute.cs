using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = true)]
    public sealed class CmdletAdditionalParameter : Attribute
    {
        public Type ParameterType { get; set; }
        public string ParameterName { get; set; }
        public bool Mandatory { get; set; }
        public string HelpMessage { get; set; }

        public int Position { get; set; }
        public string ParameterSetName { get; set; }
    }
}
