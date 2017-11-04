using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Specify this attribute on a cmdlet parameter to define its ordering in the documentation
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,
                    AllowMultiple = false)]
    public class PnPParameterAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
