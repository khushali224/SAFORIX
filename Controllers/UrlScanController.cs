using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;
using System.Text.RegularExpressions;

namespace SAFORIX.Controllers
{
    public class UrlScanController : Controller
    {
        private readonly SaforixDbContext _db;

        public UrlScanController(SaforixDbContext db)
        {
            _db = db;
        }

        // ================= GET =================
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // ================= POST =================
        [HttpPost]
        public IActionResult Index(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ViewBag.Error = "Please enter a valid URL";
                return View();
            }

            try
            {
                int risk = CalculateRisk(url);
                string level = GetRiskLevel(risk);
                string result = GenerateResult(level);

                var scan = new UrlScanHistory
                {
                    Url = url,
                    RiskPercentage = risk,
                    RiskLevel = level,
                    AnalysisResult = result,
                    ScannedAt = DateTime.Now,
                    UserId = GetCurrentUserId()
                };

                _db.UrlScanHistories.Add(scan);
                _db.SaveChanges();

                ViewBag.Url = url;
                ViewBag.Risk = risk;
                ViewBag.Level = level;
                ViewBag.Result = result;
            }
            catch
            {
                ViewBag.Error = "Invalid URL format";
            }

            return View();
        }

        // ================= FINAL ULTRA SAFE LOGIC =================
        private int CalculateRisk(string inputUrl)
        {
            if (string.IsNullOrWhiteSpace(inputUrl))
                return 0;

            string url = inputUrl.Trim().ToLower();

            // Add scheme if missing
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                return 95;

            string host = uri.Host;

            int score = 0;

            // ================= 🚨 CRITICAL DIRECT DETECTION =================

            if (url.Contains("@") || url.Contains("%00"))
                return 95;

            if (Regex.IsMatch(url, @"\.(exe|apk|bat|sh|zip|rar)$"))
                return 100;

            if (Regex.IsMatch(host, @"(g00gle|faceb00k|paypa1|amaz0n)"))
                return 95;

            if (Regex.IsMatch(host, @"\.(tk|ml|ga|cf|gq)$"))
                score += 30;

            string[] blacklist = { "malware.com", "phishing.com", "scamurl.net" };
            if (blacklist.Any(b => host.Contains(b)))
                return 100;

            // ================= 🔍 NORMAL CHECKS =================

            // HTTPS
            if (uri.Scheme != Uri.UriSchemeHttps)
                score += 25;

            // IP address
            if (Regex.IsMatch(host, @"^\d{1,3}(\.\d{1,3}){3}$"))
                score += 30;

            // Long URL
            if (url.Length > 80)
                score += 20;

            // Suspicious keywords
            if (Regex.IsMatch(url, @"(login|verify|secure|account|bank|update|password|otp|free|bonus|win)"))
                score += 25;

            // Subdomains
            if (host.Split('.').Length > 3)
                score += 20;

            // Shorteners
            if (Regex.IsMatch(host, @"(bit\.ly|tinyurl|goo\.gl|t\.co|is\.gd)"))
                score += 25;

            // Special characters
            if (Regex.IsMatch(url, @"[@\-_]"))
                score += 15;

            // Double slash attack
            string cleanUrl = url.Replace("http://", "").Replace("https://", "");
            if (cleanUrl.Contains("//"))
                score += 20;

            // Numbers in domain
            if (Regex.IsMatch(host, @"\d"))
                score += 10;

            // Encoding tricks
            if (url.Contains("%") || url.Contains("%20"))
                score += 15;

            // Unicode attack
            if (host.Any(c => c > 127))
                score += 25;

            return Math.Min(score, 100);
        }

        // ================= RISK LEVEL =================
        private string GetRiskLevel(int risk)
        {
            if (risk >= 90) return "Critical Scam";
            if (risk >= 70) return "High Risk";
            if (risk >= 40) return "Suspicious";
            if (risk >= 20) return "Safe";
            return "Very Safe";
        }

        // ================= RESULT =================
        private string GenerateResult(string level)
        {
            return level switch
            {
                "Very Safe" =>
                    "URL appears safe with strong trust indicators.",

                "Safe" =>
                    "No major threats detected. Safe to browse.",

                "Suspicious" =>
                    "Some warning signs detected. Proceed carefully.",

                "High Risk" =>
                    "Multiple phishing indicators found. Avoid entering sensitive data.",

                "Critical Scam" =>
                    "Dangerous URL detected! High chance of phishing or malware.",

                _ =>
                    "Analysis completed."
            };
        }

        // ================= USER =================
        private int? GetCurrentUserId()
        {
            var username = HttpContext.Session.GetString("User");

            if (string.IsNullOrWhiteSpace(username))
                return null;

            return _db.Registration
                .Where(x => x.username == username)
                .Select(x => (int?)x.Id)
                .FirstOrDefault();
        }
    }
}