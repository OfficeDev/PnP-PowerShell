namespace PnP.PowerShell.Commands.Enums
{
    public enum InitializationType
    {
        Unknown,
        Credentials,
        Token,
        SPClientSecret,
        AADNativeApp,
        AADAppOnly,
        InteractiveLogin,
        ADFS,
        DeviceLogin,
        Graph,
        GraphDeviceLogin,
        HighTrust
    }
}
