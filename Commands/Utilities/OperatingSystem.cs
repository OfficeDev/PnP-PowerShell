using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PnP.PowerShell.Commands.Utilities
{
    public static class OperatingSystem
    {
        public static bool IsWindows() =>
#if !PNPPSCORE
            true;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

        public static bool IsMacOS() =>
#if !PNPPSCORE
            false;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif

        public static bool IsLinux() =>
#if !PNPPSCORE
            false;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
    }
}
