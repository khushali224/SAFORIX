using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;

namespace SAFORIX.Controllers.Admin
{
    public class AdminAccountController : Controller
    {
        private readonly SaforixDbContext _db;

        public AdminAccountController(SaforixDbContext db)
        {
            _db = db;
        }

        // ===================== GET : ADMIN LOGIN =====================
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }

            return View();
        }

        // ===================== POST : ADMIN LOGIN =====================
        [HttpPost]
        public IActionResult Login(string? username, string? password)
        {
            username = username?.Trim() ?? string.Empty;
            password = password?.Trim() ?? string.Empty;

            if (username == "" || password == "")
            {
                ViewBag.Error = "Username and Password are required";
                return View();
            }

            var admin = _db.AdminUsers.FirstOrDefault(a =>
                a.Username == username &&
                a.Password == password   // ✅ NO HASH
            );

            if (admin == null)
            {
                ViewBag.Error = "Invalid Admin Username or Password";
                return View();
            }

            HttpContext.Session.SetString("Admin", admin.Username);
            return RedirectToAction("Index", "AdminDashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
