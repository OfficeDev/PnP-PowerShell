using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class Clipboard
    {
        public static void Copy(string val)
        {
#if !NETSTANDARD2_0
            System.Windows.Forms.Clipboard.SetText(val);
#else
            var tempfile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(tempfile, val);
            if (OperatingSystem.IsWindows())
            {
                var cmd = $"clip < {tempfile}";
                Shell.Bat(cmd);
            }

            if (OperatingSystem.IsMacOS())
            {
                var cmd = $"pbcopy < {tempfile}";
                Shell.Bash(cmd);
            }
            System.IO.File.Delete(tempfile);
#endif
        }
    }
}
