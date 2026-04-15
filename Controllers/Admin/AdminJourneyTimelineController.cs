using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════════
    // Admin Journey Timeline Controller
    // ═══════════════════════════════════════════════════════════════
    public class AdminJourneyTimelineController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminJourneyTimelineController(SaforixDbContext db) => _db = db;
        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.JourneyTimelines.OrderBy(x => x.DisplayOrder).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(JourneyTimeline model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            _db.JourneyTimelines.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.JourneyTimelines.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, JourneyTimeline model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.JourneyTimelines.Find(id);
            if (item == null) return NotFound();
            item.Year         = model.Year;
            item.Description  = model.Description;
            item.DisplayOrder = model.DisplayOrder;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.JourneyTimelines.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.JourneyTimelines.Find(id);
            if (item == null) return NotFound();
            _db.JourneyTimelines.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
