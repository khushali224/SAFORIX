using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFORIX.Data;

namespace SAFORIX.Controllers.Admin
{
    public class AdminUrlScanController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminUrlScanController(SaforixDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            try
            {
                var history = _db.UrlScanHistories
                                 .Include(x => x.User)
                                 .OrderByDescending(x => x.ScannedAt)
                                 .ToList();
                return View(history);
            }
            catch
            {
                // Fallback without User join (in case FK column not yet added)
                var history = _db.UrlScanHistories
                                 .OrderByDescending(x => x.ScannedAt)
                                 .ToList();
                return View(history);
            }
        }
    }
}
