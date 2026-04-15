using System;
using System.ComponentModel.DataAnnotations;

namespace SAFORIX.Models
{
    public class BehaviourScanHistory
    {
        public int Id { get; set; }

        // Optional link to registered user (Registration.Id)
        public int? UserId { get; set; }
        public Registration? User { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string EventType { get; set; } = "Login"; // Login | PasswordChange | etc.

        public DateTime LoginTime { get; set; } = DateTime.Now;

        public string? DeviceName { get; set; }
        public string? DeviceType { get; set; } // PC | Laptop | Mobile | Unknown
        public string? OperatingSystem { get; set; }
        public string? Browser { get; set; }

        public string? IpAddress { get; set; }
        public string? Location { get; set; }

        public int RiskPercentage { get; set; } // 0..100
        public string RiskLevel { get; set; } = "Safe"; // Safe | Suspicious | Risky
        public string AnalysisResult { get; set; } = "No major threats detected.";

        public DateTime ScannedAt { get; set; } = DateTime.Now;
    }
}

