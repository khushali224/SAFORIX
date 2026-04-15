using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers
{
    public class ServicesController : Controller
    {
        private readonly SaforixDbContext _db;

        public ServicesController(SaforixDbContext db)
        {
            _db = db;
        }

        // ================= URL SCAN =================
        [HttpGet]
        public IActionResult UrlScan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UrlScan(string url)
        {
            url = (url ?? "").Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                ViewBag.Error = "Enter URL";
                return View();
            }

            if (!url.StartsWith("http"))
                url = "https://" + url;

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                ViewBag.Error = "Invalid URL";
                return View();
            }

            var (risk, details) = CalculateUrlRisk(uri);
            string level = GetRiskLevel(risk);
            string result = GenerateUrlResult(level, details);

            _db.UrlScanHistories.Add(new UrlScanHistory
            {
                Url = url,
                RiskPercentage = risk,
                RiskLevel = level,
                AnalysisResult = result,
                UserId = GetCurrentUserId()
            });

            _db.SaveChanges();

            ViewBag.Url = url;
            ViewBag.Risk = risk;
            ViewBag.Level = level;
            ViewBag.Result = result;

            return View();
        }

        private (int, List<string>) CalculateUrlRisk(Uri uri)
        {
            int risk = 0;
            var details = new List<string>();

            string host = uri.Host.ToLower();
            string url = uri.ToString().ToLower();

            // Trusted domains
            string[] trusted = { "google.com", "microsoft.com", "amazon.com", "github.com" };
            if (trusted.Any(t => host == t || host.EndsWith("." + t)))
            {
                risk -= 30;
                details.Add("Trusted domain");
            }

            // Fake brand detection
            string[] brands = { "paypal", "amazon", "bank", "upi", "google" };
            foreach (var b in brands)
            {
                if (host.Contains(b) && !host.EndsWith(b + ".com"))
                {
                    risk += 40;
                    details.Add("Fake brand: " + b);
                }
            }

            if (uri.Scheme != "https") { risk += 25; details.Add("No HTTPS"); }

            if (Regex.IsMatch(host, @"^\d+\.\d+\.\d+\.\d+$"))
            {
                risk += 35; details.Add("IP address URL");
            }

            if (url.Length > 120) { risk += 20; details.Add("Long URL"); }

            string[] words = { "login", "verify", "bonus", "free", "win", "password", "bank" };
            int count = words.Count(w => url.Contains(w));
            if (count > 0)
            {
                risk += Math.Min(20 + count * 5, 45);
                details.Add("Phishing keywords");
            }

            if (host.Split('.').Length > 4) { risk += 20; details.Add("Too many subdomains"); }

            if (url.Contains("@") || url.Contains("%")) { risk += 15; details.Add("Special chars"); }

            if (Regex.IsMatch(host, @"\.(tk|ml|ga|cf)$")) { risk += 20; details.Add("Bad TLD"); }

            risk = Math.Clamp(risk, 0, 100);
            return (risk, details);
        }

        // ================= BEHAVIOUR SCAN =================
        [HttpGet]
        public IActionResult BehaviourScan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BehaviourScan(
            string email,
            string? deviceName,
            string? deviceType,
            string? operatingSystem,
            string? browser,
            string? ipAddress,
            string? location,
            DateTime? loginTime
        )
        {
            email = (email ?? "").Trim();

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ViewBag.Error = "Invalid Email";
                return View();
            }

            var now = loginTime ?? DateTime.Now;
            int risk = 0;
            var details = new List<string>();

            var last = _db.BehaviourScanHistories
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.LoginTime)
                .FirstOrDefault();

            if (last != null)
            {
                if (last.IpAddress != ipAddress)
                {
                    risk += 25;
                    details.Add("IP changed");
                }

                if (last.Location != location)
                {
                    risk += 20;
                    details.Add("Location changed");
                }

                if (last.DeviceName != deviceName)
                {
                    risk += 30;
                    details.Add("New device");
                }

                if ((now - last.LoginTime).TotalMinutes < 5)
                {
                    risk += 20;
                    details.Add("Rapid login");
                }
            }

            if (now.Hour < 6 || now.Hour > 23)
            {
                risk += 15;
                details.Add("Odd login time");
            }

            if (string.IsNullOrWhiteSpace(ipAddress)) risk += 10;
            if (string.IsNullOrWhiteSpace(deviceName)) risk += 10;

            risk = Math.Clamp(risk, 0, 100);

            string level = risk < 30 ? "Safe" : risk < 70 ? "Suspicious" : "Risky";
            string result = string.Join(", ", details);

            _db.BehaviourScanHistories.Add(new BehaviourScanHistory
            {
                Email = email,
                LoginTime = now,
                DeviceName = deviceName,
                DeviceType = deviceType,
                OperatingSystem = operatingSystem,
                Browser = browser,
                IpAddress = ipAddress,
                Location = location,
                RiskPercentage = risk,
                RiskLevel = level,
                AnalysisResult = result,
                UserId = GetCurrentUserId()
            });

            _db.SaveChanges();

            ViewBag.Risk = risk;
            ViewBag.Level = level;
            ViewBag.Result = result;

            return View();
        }

        // ================= DEVICE SCAN =================
        [HttpGet]
        public IActionResult DeviceScan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeviceScan(
            string deviceName,
            string? deviceType,
            string? operatingSystem,
            string? ipAddress,
            bool firewallEnabled,
            bool antivirusEnabled,
            bool diskEncryptionEnabled,
            bool osUpToDate,
            bool suspiciousProcessesFound,
            bool unknownUsbDetected,
            int openPortsCount
        )
        {
            int risk = 0;
            var details = new List<string>();

            if (!firewallEnabled) { risk += 25; details.Add("Firewall OFF"); }
            if (!antivirusEnabled) { risk += 25; details.Add("Antivirus OFF"); }
            if (!diskEncryptionEnabled) { risk += 15; details.Add("No encryption"); }
            if (!osUpToDate) { risk += 20; details.Add("OS outdated"); }
            if (suspiciousProcessesFound) { risk += 30; details.Add("Malware detected"); }
            if (unknownUsbDetected) { risk += 20; details.Add("Unknown USB"); }

            if (openPortsCount > 20) { risk += 20; details.Add("Too many ports"); }

            if (!firewallEnabled && !antivirusEnabled)
            {
                risk += 15;
                details.Add("Critical protection missing");
            }

            risk = Math.Clamp(risk, 0, 100);

            string level = risk < 30 ? "Safe" : risk < 70 ? "Suspicious" : "Risky";
            string result = string.Join(", ", details);

            _db.DeviceScanHistories.Add(new DeviceScanHistory
            {
                DeviceName = deviceName,
                DeviceType = deviceType,
                OperatingSystem = operatingSystem,
                IpAddress = ipAddress,
                FirewallEnabled = firewallEnabled,
                AntivirusEnabled = antivirusEnabled,
                DiskEncryptionEnabled = diskEncryptionEnabled,
                OsUpToDate = osUpToDate,
                SuspiciousProcessesFound = suspiciousProcessesFound,
                UnknownUsbDetected = unknownUsbDetected,
                OpenPortsCount = openPortsCount,
                RiskPercentage = risk,
                RiskLevel = level,
                AnalysisResult = result,
                UserId = GetCurrentUserId()
            });

            _db.SaveChanges();

            ViewBag.Risk = risk;
            ViewBag.Level = level;
            ViewBag.Result = result;

            return View();
        }

        // ================= COMMON =================
        private string GetRiskLevel(int risk)
        {
            if (risk <= 20) return "Very Safe";
            if (risk <= 40) return "Safe";
            if (risk <= 65) return "Suspicious";
            if (risk <= 85) return "High Risk";
            return "Critical Scam";
        }

        private string GenerateUrlResult(string level, List<string> details)
        {
            return level switch
            {
                "Very Safe" => "Safe website",
                "Safe" => "No issue",
                "Suspicious" => "Be careful: " + string.Join(", ", details),
                "High Risk" => "Danger: " + string.Join(", ", details),
                _ => "SCAM DETECTED: " + string.Join(", ", details)
            };
        }

        private int? GetCurrentUserId()
        {
            var username = HttpContext.Session.GetString("User");

            return _db.Registration
                .Where(x => x.username == username)
                .Select(x => (int?)x.Id)
                .FirstOrDefault();
        }
    }
}