using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    internal class SPODriveInfo : PSDriveInfo
    {
        public Web Web { get; set; }
        internal int ItemTimeout { get; set; }
        internal int WebTimeout { get; set; }
        internal List<SPODriveCacheItem> CachedItems { get; set; }
        internal List<SPODriveCacheWeb> CachedWebs { get; set; }

        public SPODriveInfo(PSDriveInfo driveInfo) : base(driveInfo)
        {
            CachedItems = new List<SPODriveCacheItem>();
            CachedWebs = new List<SPODriveCacheWeb>();
        }
    }
}