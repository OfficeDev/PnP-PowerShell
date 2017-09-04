using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    public enum CmdletSupportedPlatform
    {
        All = 0,
        Online = 1,
        OnPremises = 2,
        SP2013 = 3,
        SP2016 = 4
    }
}
