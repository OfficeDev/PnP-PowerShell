using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities
{
    public static class FileUtilities
    {
        internal static bool IsOpenOfficeFile(Stream stream)
        {
            byte[] bytes = new byte[6];
            stream.Read(bytes, 0, 6);
            var signature = string.Empty;
            foreach (var b in bytes)
            {
                signature += b.ToString("X2");
            }
            // SIG 50 4B 03 04 14 00
            return signature == "504B03041400";
        }
    }
}
