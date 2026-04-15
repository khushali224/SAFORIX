using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════════
    // Admin Mission Vision Controller
    // ═══════════════════════════════════════════════════════════════
    public class AdminMissionVisionController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminMissionVisionController(SaforixDbContext db) => _db = db;
        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.MissionVisions.OrderBy(x => x.Type).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(MissionVision model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            _db.MissionVisions.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.MissionVisions.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MissionVision model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.MissionVisions.Find(id);
            if (item == null) return NotFound();
            item.Type        = model.Type;
            item.Title       = model.Title;
            item.Description = model.Description;
            item.IsActive    = model.IsActive;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.MissionVisions.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.MissionVisions.Find(id);
            if (item == null) return NotFound();
            _db.MissionVisions.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
