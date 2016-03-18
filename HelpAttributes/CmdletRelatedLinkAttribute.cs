using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = true)]
    public class CmdletRelatedLinkAttribute : Attribute
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
