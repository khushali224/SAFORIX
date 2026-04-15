using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    public class AdminHeaderNavController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminHeaderNavController(SaforixDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var items = _db.HeaderNavItems.OrderBy(x => x.ParentId ?? 0).ThenBy(x => x.SortOrder).ToList();
            ViewBag.Parents = _db.HeaderNavItems.Where(x => x.ParentId == null).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            ViewBag.Parents = _db.HeaderNavItems.Where(x => x.ParentId == null).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HeaderNavItem model)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            _db.HeaderNavItems.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var item = _db.HeaderNavItems.Find(id);
            if (item == null) return NotFound();

            ViewBag.Parents = _db.HeaderNavItems.Where(x => x.ParentId == null && x.Id != id).ToList();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HeaderNavItem model)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var item = _db.HeaderNavItems.Find(id);
            if (item == null) return NotFound();

            item.DisplayText = model.DisplayText;
            item.Controller = model.Controller;
            item.Action = model.Action;
            item.ParentId = model.ParentId;
            item.SortOrder = model.SortOrder;
            item.IsActive = model.IsActive;

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var item = _db.HeaderNavItems.Find(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var item = _db.HeaderNavItems.Find(id);
            if (item == null) return NotFound();

            _db.HeaderNavItems.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
