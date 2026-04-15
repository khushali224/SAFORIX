using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;

namespace SAFORIX.Controllers
{
    public class ThreatController : Controller
    {
        private readonly SaforixDbContext _db;
        public ThreatController(SaforixDbContext db) => _db = db;

        public IActionResult Index()
        {
            var contents = _db.ThreatContents
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToList();
            return View(contents);
        }
    }
}
