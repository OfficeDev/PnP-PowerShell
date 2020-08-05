using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model
{
    public enum ConnectionMethod
    {
        Unspecified,
        WebLogin,
        Credentials,
        AccessToken,
        AzureADAppOnly,
        AzureADNativeApplication,
        ADFS,
        GraphDeviceLogin,
        DeviceLogin
    }
}
