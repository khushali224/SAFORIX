using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════════
    // Admin Home Hero Controller
    // ═══════════════════════════════════════════════════════════════
    public class AdminHomeHeroController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminHomeHeroController(SaforixDbContext db) => _db = db;
        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.HomeHeroes.OrderByDescending(x => x.CreatedAt).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(HomeHero model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            model.CreatedAt = DateTime.Now;
            _db.HomeHeroes.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeHeroes.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HomeHero model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeHeroes.Find(id);
            if (item == null) return NotFound();
            item.MainTitle   = model.MainTitle;
            item.SubTitle    = model.SubTitle;
            item.Description = model.Description;
            item.ButtonText  = model.ButtonText;
            item.ButtonLink  = model.ButtonLink;
            item.IsActive    = model.IsActive;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeHeroes.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HomeHeroes.Find(id);
            if (item == null) return NotFound();
            _db.HomeHeroes.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
