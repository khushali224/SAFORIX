using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    public class AdminDashboardController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminDashboardController(SaforixDbContext db)
        {
            _db = db;
        }

        public dynamic GetViewBag()
        {
            return ViewBag;
        }

        public IActionResult Index(dynamic viewBag)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            ViewBag.TotalUsers = _db.Registration.Count();
            ViewBag.TotalAdmins = _db.AdminUsers.Count();
            ViewBag.UrlScans = _db.UrlScanHistories.Count();
            ViewBag.DeviceScans = _db.DeviceScanHistories.Count();
            ViewBag.BehaviourScans = _db.BehaviourScanHistories.Count();

            // Simple "threats" metric: anything not Safe
            ViewBag.Threats =
                _db.UrlScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.DeviceScanHistories.Count(x => x.RiskLevel != "Safe") +
                _db.BehaviourScanHistories.Count(x => x.RiskLevel != "Safe");

            ViewBag.LatestUsers = _db.Registration
    .OrderByDescending(u => u.Id)
    .Take(5)
    .Select(u => new
    {
        Id = u.Id,
        username = u.username ?? "N/A",
        email = u.email ?? "N/A"
    })
    .ToList();


            ViewBag.LatestAdmins = _db.AdminUsers
                .OrderByDescending(a => a.Id)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
