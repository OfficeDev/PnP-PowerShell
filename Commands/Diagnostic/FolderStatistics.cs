namespace PnP.PowerShell.Commands.Diagnostic
{
    public class FolderStatistics
    {
        public int ItemCount { get; set; }
        public int FolderCount { get; set; }
        public int ItemVersionCount { get; internal set; }
        public long TotalFileSize { get; set; }
        public int BrokenPermissionCount { get; set; }

        public static FolderStatistics operator +(FolderStatistics s1, FolderStatistics s2)
        {
            return s2 != null ? new FolderStatistics
            {
                ItemCount = s1.ItemCount + s2.ItemCount,
                FolderCount = s1.FolderCount + s2.FolderCount,
                ItemVersionCount = s1.ItemVersionCount + s2.ItemVersionCount,
                TotalFileSize = s1.TotalFileSize + s2.TotalFileSize,
                BrokenPermissionCount = s1.BrokenPermissionCount + s2.BrokenPermissionCount
            } : s1.MemberwiseClone() as FolderStatistics;
        }
    }
}
