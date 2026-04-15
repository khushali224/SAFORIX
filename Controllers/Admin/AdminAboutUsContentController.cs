using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════
    // Admin About Us Content Controller
    // ═══════════════════════════════════════════════════════════
    public class AdminAboutUsContentController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminAboutUsContentController(SaforixDbContext db) => _db = db;

        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.AboutUsContents.OrderBy(x => x.SortOrder).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(AboutUsContent model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            model.CreatedAt = DateTime.Now;
            _db.AboutUsContents.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.AboutUsContents.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AboutUsContent model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.AboutUsContents.Find(id);
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
            var item = _db.AboutUsContents.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.AboutUsContents.Find(id);
            if (item == null) return NotFound();
            _db.AboutUsContents.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
