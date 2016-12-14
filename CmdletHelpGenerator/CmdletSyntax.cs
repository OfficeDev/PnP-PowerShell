using System.Collections.Generic;

namespace SharePointPnP.PowerShell.CmdletHelpGenerator
{
    public class CmdletSyntax
    {
        public string ParameterSetName { get; set; }
        public List<CmdletParameterInfo> Parameters { get; set; }

        public CmdletSyntax()
        {
            Parameters = new List<CmdletParameterInfo>();
        }
    }
}