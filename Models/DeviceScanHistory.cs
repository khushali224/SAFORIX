using System;
using System.ComponentModel.DataAnnotations;

namespace SAFORIX.Models
{
    public class DeviceScanHistory
    {
        public int Id { get; set; }

        // Optional link to registered user (Registration.Id)
        public int? UserId { get; set; }
        public Registration? User { get; set; }

        [Required]
        public string DeviceName { get; set; } = string.Empty;

        public string? DeviceType { get; set; } // PC | Laptop | Server | Unknown
        public string? OperatingSystem { get; set; }
        public string? IpAddress { get; set; }

        // "Full device scan" inputs (form-based / demo)
        public bool FirewallEnabled { get; set; }
        public bool AntivirusEnabled { get; set; }
        public bool DiskEncryptionEnabled { get; set; }
        public bool OsUpToDate { get; set; }
        public bool SuspiciousProcessesFound { get; set; }
        public bool UnknownUsbDetected { get; set; }
        public int OpenPortsCount { get; set; }

        public int RiskPercentage { get; set; } // 0..100
        public string RiskLevel { get; set; } = "Safe"; // Safe | Suspicious | Risky
        public string AnalysisResult { get; set; } = "No major threats detected.";

        public DateTime ScannedAt { get; set; } = DateTime.Now;
    }
}

