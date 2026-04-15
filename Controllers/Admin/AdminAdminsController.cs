using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers.Admin
{
    public class AdminAdminsController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminAdminsController(SaforixDbContext db)
        {
            _db = db;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            return View(_db.AdminUsers.ToList());
        }

        // ================= CREATE =================
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            return View();
        }

        [HttpPost]
        public IActionResult Create(AdminUser admin)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            if (string.IsNullOrWhiteSpace(admin.Username) ||
                string.IsNullOrWhiteSpace(admin.Password))
            {
                ViewBag.Error = "All fields are required";
                return View(admin);
            }

            _db.AdminUsers.Add(admin);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var admin = _db.AdminUsers.Find(id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        [HttpPost]
        public IActionResult Edit(AdminUser admin)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            _db.AdminUsers.Update(admin);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var admin = _db.AdminUsers.Find(id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var admin = _db.AdminUsers.Find(id);
            if (admin != null)
            {
                _db.AdminUsers.Remove(admin);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
