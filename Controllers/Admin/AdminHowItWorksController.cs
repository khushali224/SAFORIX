using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    // ═══════════════════════════════════════════════════════════════
    // Admin How It Works Controller
    // ═══════════════════════════════════════════════════════════════
    public class AdminHowItWorksController : Controller
    {
        private readonly SaforixDbContext _db;
        public AdminHowItWorksController(SaforixDbContext db) => _db = db;
        private bool IsAdmin() => HttpContext.Session.GetString("Admin") != null;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var items = _db.HowItWorksSteps.OrderBy(x => x.StepOrder).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(HowItWorksStep model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            _db.HowItWorksSteps.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HowItWorksSteps.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HowItWorksStep model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HowItWorksSteps.Find(id);
            if (item == null) return NotFound();
            item.StepTitle   = model.StepTitle;
            item.Description = model.Description;
            item.StepOrder   = model.StepOrder;
            item.IsActive    = model.IsActive;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HowItWorksSteps.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "AdminAccount");
            var item = _db.HowItWorksSteps.Find(id);
            if (item == null) return NotFound();
            _db.HowItWorksSteps.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
