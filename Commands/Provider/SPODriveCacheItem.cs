using System;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    internal class SPODriveCacheItem
    {
        public string Path { get; set; }
        public DateTime LastRefresh { get; set; }
        public object Item { get; set; }
    }
}