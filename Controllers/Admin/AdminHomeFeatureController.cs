using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════════
    // Admin Home Feature Controller
    // ═══════════════════════════════════════════════════════════════
    public class AdminHomeFeatureController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminHomeFeatureController(SaforixDbContext db) => _db = db;
        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.HomeFeatures.OrderBy(x => x.DisplayOrder).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(HomeFeature model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            _db.HomeFeatures.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeFeatures.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HomeFeature model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeFeatures.Find(id);
            if (item == null) return NotFound();
            item.Icon         = model.Icon;
            item.Title        = model.Title;
            item.Description  = model.Description;
            item.DisplayOrder = model.DisplayOrder;
            item.IsActive     = model.IsActive;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeFeatures.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeFeatures.Find(id);
            if (item == null) return NotFound();
            _db.HomeFeatures.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
