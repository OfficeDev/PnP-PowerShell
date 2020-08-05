namespace PnP.PowerShell.Commands.Diagnostic
{
    public class WebStatistics
    {
        public int WebCount { get; set; }
        public int ListCount { get; set; }
        public int FileCount { get; set; }
        public int ItemCount { get; internal set; }
        public long TotalFileSize { get; set; }
        public int SiteUserCount { get; set; }
        public int SiteGroupCount { get; set; }
        public int BrokenPermissionCount { get; set; }

        public static WebStatistics operator +(WebStatistics s1, WebStatistics s2)
        {
            return new WebStatistics
            {
                WebCount = s1.WebCount + s2.WebCount,
                ListCount = s1.ListCount + s2.ListCount,
                ItemCount = s1.ItemCount + s2.ItemCount,
                FileCount = s1.FileCount + s2.FileCount,
                TotalFileSize = s1.TotalFileSize + s2.TotalFileSize,
                SiteUserCount = s1.SiteUserCount,
                SiteGroupCount = s1.SiteGroupCount,
                BrokenPermissionCount = s1.BrokenPermissionCount + s2.BrokenPermissionCount,
            };
        }

        public static WebStatistics operator +(WebStatistics s1, FolderStatistics s2)
        {
            return new WebStatistics
            {
                WebCount = s1.WebCount,
                ListCount = s1.ListCount,
                ItemCount = s1.ItemCount,
                FileCount = s2 != null ? s1.FileCount + s2.ItemCount : s1.FileCount, //folder items = files
                TotalFileSize = s2 != null ? s1.TotalFileSize + s2.TotalFileSize : s1.TotalFileSize,
                SiteUserCount = s1.SiteUserCount,
                SiteGroupCount = s1.SiteGroupCount,
                BrokenPermissionCount = s2 != null ? s1.BrokenPermissionCount + s2.BrokenPermissionCount : s1.BrokenPermissionCount,
            };
        }

    }
}
