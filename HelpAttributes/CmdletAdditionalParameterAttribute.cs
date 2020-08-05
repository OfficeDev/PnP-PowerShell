using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Specify this cmdlet on the cmdlet class in order to forcibly add parameters to the documentation.
    /// </summary>
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
        public int Order { get; set; }
    }
}
