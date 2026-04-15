using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;
using System.Security.Cryptography;
using System.Text;

namespace SAFORIX.Controllers.Admin
{
    public class AdminUsersController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminUsersController(SaforixDbContext db)
        {
            _db = db;
        }

        // ================= VIEW USERS =================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var users = _db.Registration
                .Select(u => new Registration
                {
                    Id = u.Id,
                    username = u.username ?? "",
                    email = u.email ?? "",
                    password = u.password ?? ""
                })
                .ToList();

            return View(users);
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(string username, string email, string password)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            username = (username ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();
            password = (password ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            string hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            _db.Registration.Add(new Registration
            {
                username = username,
                email = email,
                password = hash
            });
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT GET =================
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var user = _db.Registration.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // ================= EDIT POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string username, string email, string? newPassword)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var user = _db.Registration.Find(id);
            if (user == null) return NotFound();

            user.username = (username ?? string.Empty).Trim();
            user.email = (email ?? string.Empty).Trim();

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                user.password = Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(newPassword.Trim())));
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE GET =================
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var user = _db.Registration.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // ================= DELETE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "AdminAccount");

            var user = _db.Registration.Find(id);
            if (user == null) return NotFound();

            _db.Registration.Remove(user);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
