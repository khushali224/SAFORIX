using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFORIX.Data;

namespace SAFORIX.Controllers.Admin
{
    public class AdminDeviceScanController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminDeviceScanController(SaforixDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            try
            {
                var history = _db.DeviceScanHistories
                    .Include(x => x.User)
                    .OrderByDescending(x => x.ScannedAt)
                    .ToList();
                return View(history);
            }
            catch
            {
                // Fallback without User join (in case FK column not yet added)
                var history = _db.DeviceScanHistories
                    .OrderByDescending(x => x.ScannedAt)
                    .ToList();
                return View(history);
            }
        }
    }
}
