using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    public class AdminAnalysisController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminAnalysisController(SaforixDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var vm = new AdminAnalysisViewModel
            {
                TotalUrlScans = _db.UrlScanHistories.Count(),
                TotalDeviceScans = _db.DeviceScanHistories.Count(),
                TotalBehaviourScans = _db.BehaviourScanHistories.Count(),
            };

            vm.TotalThreats =
                _db.UrlScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.DeviceScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.BehaviourScanHistories.Count(x => x.RiskLevel != "Safe");

            vm.RecentUrlScans = _db.UrlScanHistories
                .OrderByDescending(x => x.ScannedAt)
                .Take(8)
                .ToList();

            vm.RecentDeviceScans = _db.DeviceScanHistories
                .OrderByDescending(x => x.ScannedAt)
                .Take(8)
                .ToList();

            vm.RecentBehaviourScans = _db.BehaviourScanHistories
                .OrderByDescending(x => x.ScannedAt)
                .Take(8)
                .ToList();

            return View(vm);
        }
    }
}

