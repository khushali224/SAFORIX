using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.Controllers
{
    public class AboutController : Controller
    {
        private readonly SaforixDbContext _db;
        public AboutController(SaforixDbContext db) => _db = db;

        public IActionResult Index()
        {
            // New structured tables
            var aboutContent  = _db.AboutPageContents.FirstOrDefault(x => x.IsActive);
            var missionVision = _db.MissionVisions.Where(x => x.IsActive).OrderBy(x => x.Type).ToList();
            var howItWorks    = _db.HowItWorksSteps.Where(x => x.IsActive).OrderBy(x => x.StepOrder).ToList();
            var journey       = _db.JourneyTimelines.OrderBy(x => x.DisplayOrder).ToList();

            ViewBag.AboutContent  = aboutContent;
            ViewBag.MissionVision = missionVision;
            ViewBag.HowItWorks    = howItWorks;
            ViewBag.Journey       = journey;

            return View();
        }
    }
}
