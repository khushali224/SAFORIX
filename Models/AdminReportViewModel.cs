using System;
using System.Collections.Generic;

namespace SAFORIX.Models
{
    public class AdminReportViewModel
    {
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalUrlScans { get; set; }
        public int TotalDeviceScans { get; set; }
        public int TotalBehaviourScans { get; set; }
        public int TotalThreats { get; set; }

        public List<UrlScanHistory> ThreatUrls { get; set; } = new();
        public List<DeviceScanHistory> RiskyDevices { get; set; } = new();
        public List<BehaviourScanHistory> RiskyBehaviours { get; set; } = new();
    }
}

