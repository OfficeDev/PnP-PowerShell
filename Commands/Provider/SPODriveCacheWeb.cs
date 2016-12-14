using System;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    internal class SPODriveCacheWeb
    {
        public string Path { get; set; }
        public DateTime LastRefresh { get; set; }
        public Web Web { get; set; }
    }
}