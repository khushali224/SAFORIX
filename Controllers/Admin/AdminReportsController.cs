using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    public class AdminReportsController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminReportsController(SaforixDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var vm = new AdminReportViewModel
            {
                TotalUsers = _db.Registration.Count(),
                TotalAdmins = _db.AdminUsers.Count(),
                TotalUrlScans = _db.UrlScanHistories.Count(),
                TotalDeviceScans = _db.DeviceScanHistories.Count(),
                TotalBehaviourScans = _db.BehaviourScanHistories.Count(),
            };

            vm.TotalThreats =
                _db.UrlScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.DeviceScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.BehaviourScanHistories.Count(x => x.RiskLevel != "Safe");

            vm.ThreatUrls = _db.UrlScanHistories
                .Where(x => x.RiskLevel != "Safe")
                .OrderByDescending(x => x.RiskPercentage)
                .ThenByDescending(x => x.ScannedAt)
                .Take(10)
                .ToList();

            vm.RiskyDevices = _db.DeviceScanHistories
                .Where(x => x.RiskLevel != "Safe")
                .OrderByDescending(x => x.RiskPercentage)
                .ThenByDescending(x => x.ScannedAt)
                .Take(10)
                .ToList();

            vm.RiskyBehaviours = _db.BehaviourScanHistories
                .Where(x => x.RiskLevel != "Safe")
                .OrderByDescending(x => x.RiskPercentage)
                .ThenByDescending(x => x.ScannedAt)
                .Take(10)
                .ToList();

            return View(vm);
        }
    }
}

