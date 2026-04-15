using System.Collections.Generic;

namespace SAFORIX.Models
{
    public class AdminAnalysisViewModel
    {
        public int TotalUrlScans { get; set; }
        public int TotalDeviceScans { get; set; }
        public int TotalBehaviourScans { get; set; }
        public int TotalThreats { get; set; }

        public List<UrlScanHistory> RecentUrlScans { get; set; } = new();
        public List<DeviceScanHistory> RecentDeviceScans { get; set; } = new();
        public List<BehaviourScanHistory> RecentBehaviourScans { get; set; } = new();
    }
}

