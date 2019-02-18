using System;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// The list of supported platforms by these cmdlets.
    /// </summary>
    [Flags]
    public enum CmdletSupportedPlatform : int
    {
        All = 1,
        Online = 2,
        OnPremises = 4,
        SP2013 = 8,
        SP2016 = 16,
        SP2019 = 32
    }
}
