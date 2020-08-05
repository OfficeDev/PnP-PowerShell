#if !SP2013 && !SP2016

namespace PnP.PowerShell.Commands.ClientSidePages
{
    internal static class ClientSidePageUtilities
    {
        public static string EnsureCorrectPageName(string pageName)
        {
            if (pageName != null && !pageName.EndsWith(".aspx"))
                pageName += ".aspx";

            return pageName;
        }
    }
}
#endif
