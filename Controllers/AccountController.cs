using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;
using System.Security.Cryptography;
using System.Text;

namespace SAFORIX.Controllers
{
    public class AccountController : Controller
    {
        private readonly SaforixDbContext _db;

        public AccountController(SaforixDbContext db)
        {
            _db = db;
        }

        // ===================== LOGIN & REGISTER PAGE =====================
        public IActionResult LoginRegistration()
        {
            // Generate captcha
            string captcha = new Random().Next(1000, 9999).ToString();

            // Store captcha in session (CORRECT WAY)
            HttpContext.Session.SetString("captcha", captcha);

            // Send captcha to view
            ViewBag.Captcha = captcha;

            return View();
        }

        // ===================== LOGIN =====================
        [HttpPost]
        public IActionResult Login(string username, string password, string captcha)
        {
            // Validate captcha
            string sessionCaptcha = HttpContext.Session.GetString("captcha");

            if (captcha != sessionCaptcha)
            {
                TempData["Error"] = "Invalid Captcha";
                return RedirectToAction("LoginRegistration");
            }

            // Hash password
            string hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            // Check user
            var userExists = _db.Registration
                .Any(x => x.username == username && x.password == hash);

            if (!userExists)
            {
                TempData["Error"] = "Invalid Username or Password";
                return RedirectToAction("LoginRegistration");
            }

            // Save user session
            HttpContext.Session.SetString("User", username);

            return RedirectToAction("Index", "Home");
        }

        // ===================== REGISTER =====================
        [HttpPost]
        public IActionResult Register(string username, string email, string password, string captcha)
        {
            // Validate captcha
            string sessionCaptcha = HttpContext.Session.GetString("captcha");

            if (captcha != sessionCaptcha)
            {
                TempData["Error"] = "Invalid Captcha";
                return RedirectToAction("LoginRegistration");
            }

            // Hash password
            string hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            // Insert user
            _db.Registration.Add(new Registration
            {
                username = username,
                email = email,
                password = hash
            });

            _db.SaveChanges();

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("LoginRegistration");
        }
    }
}
