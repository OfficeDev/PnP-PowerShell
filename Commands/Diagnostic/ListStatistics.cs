namespace PnP.PowerShell.Commands.Diagnostic
{
    public class ListStatistics
    {
        public int ItemCount { get; set; }
        public int ItemVersionCount { get; set; }
        public int FolderCount { get; set; }
        public long TotalFileSize { get; set; }
        public int BrokenPermissionCount { get; set; }

        public static ListStatistics operator +(ListStatistics s1, FolderStatistics s2)
        {
            return new ListStatistics
            {
                ItemCount = s1.ItemCount,
                ItemVersionCount = s1.ItemVersionCount + s2.ItemVersionCount,
                FolderCount = s1.FolderCount + s2.FolderCount,
                TotalFileSize = s1.TotalFileSize + s2.TotalFileSize,
                BrokenPermissionCount = s1.BrokenPermissionCount + s2.BrokenPermissionCount
            };
        }
    }
}
