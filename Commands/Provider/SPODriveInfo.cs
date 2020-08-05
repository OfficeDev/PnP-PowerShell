using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Provider
{
    internal class SPODriveInfo : PSDriveInfo
    {
        private List<SPODriveCacheItem> _cachedItems;
        private List<SPODriveCacheWeb> _cachedWebs;
        private const int MaxAllowedItemCacheTimeout = 1000 * 60 * 10; //10 minutes max caching of item object
        private const int MaxAllowedWebCacheTimeout = 1000 * 60 * 60; //60 minutes max caching of web object

        public Web Web { get; set; }
        internal int ItemTimeout { get; set; }
        internal int WebTimeout { get; set; }

        internal List<SPODriveCacheItem> CachedItems
        {
            get
            {
                CleanStaleItems();
                return _cachedItems;
            }
            set
            {
                CleanStaleItems();
                _cachedItems = value;
            }
        }

        internal List<SPODriveCacheWeb> CachedWebs
        {
            get
            {
                CleanStaleWebs();
                return _cachedWebs;
            }
            set
            {
                CleanStaleWebs();
                _cachedWebs = value;
            }
        }

        public SPODriveInfo(PSDriveInfo driveInfo) : base(driveInfo)
        {
            CachedItems = new List<SPODriveCacheItem>();
            CachedWebs = new List<SPODriveCacheWeb>();
        }

        private void CleanStaleItems()
        {
            if (_cachedItems == null) return;
            var nowTicks = DateTime.Now.Ticks;
            var timeout = MaxAllowedItemCacheTimeout < ItemTimeout ? MaxAllowedItemCacheTimeout : ItemTimeout;
            _cachedItems.RemoveAll(item => new TimeSpan(nowTicks - item.LastRefresh.Ticks).TotalMilliseconds > timeout);

        }

        private void CleanStaleWebs()
        {
            if (_cachedWebs == null) return;
            var nowTicks = DateTime.Now.Ticks;
            var timeout = MaxAllowedWebCacheTimeout < WebTimeout ? MaxAllowedWebCacheTimeout : WebTimeout;
            _cachedWebs.RemoveAll(item => new TimeSpan(nowTicks - item.LastRefresh.Ticks).TotalMilliseconds > timeout);
        }

        public void ClearState()
        {
            this.CachedItems = new List<SPODriveCacheItem>();
            this.CachedWebs = new List<SPODriveCacheWeb>();
            this.Web = null;
        }
    }
}