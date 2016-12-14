using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = true)]
    public sealed class CmdletAliasAttribute : Attribute
    {
        private string _alias;
        public CmdletAliasAttribute(string alias)
        {
            _alias = alias;
        }

        public string Alias { get { return _alias; } }
    }
}
