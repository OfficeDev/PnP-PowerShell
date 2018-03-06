using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class OperatingSystem
    {
        public static bool IsWindows() =>
#if !NETSTANDARD2_0
            true;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

        public static bool IsMacOS() =>
#if !NETSTANDARD2_0
            false;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif

        public static bool IsLinux() =>
#if !NETSTANDARD2_0
            false;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
    }
}
