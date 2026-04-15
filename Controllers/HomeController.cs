using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers
{
    public class HomeController : Controller
    {
        private readonly SaforixDbContext _db;
        public HomeController(SaforixDbContext db) => _db = db;

        public IActionResult Index()
        {
            // New structured tables
            var hero = _db.HomeHeroes.FirstOrDefault(x => x.IsActive);
            var features = _db.HomeFeatures
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            ViewBag.Hero     = hero;
            ViewBag.Features = features;

            return View();
        }
    }
}
