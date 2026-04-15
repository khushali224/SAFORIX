using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════
    // Admin Home Content Controller
    // ═══════════════════════════════════════════════════════════
    public class AdminHomeContentController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminHomeContentController(SaforixDbContext db) => _db = db;

        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.HomeContents.OrderBy(x => x.SortOrder).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(HomeContent model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            model.CreatedAt = DateTime.Now;
            _db.HomeContents.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeContents.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HomeContent model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeContents.Find(id);
            if (item == null) return NotFound();
            item.SectionKey = model.SectionKey;
            item.Title      = model.Title;
            item.Body       = model.Body;
            item.SortOrder  = model.SortOrder;
            item.IsActive   = model.IsActive;
            item.UpdatedAt  = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeContents.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeContents.Find(id);
            if (item == null) return NotFound();
            _db.HomeContents.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
