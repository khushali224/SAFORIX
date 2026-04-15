using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;
using SAFORIX.Models;

namespace SAFORIX.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly SaforixDbContext _db;

        public HeaderViewComponent(SaforixDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var navItems = _db.HeaderNavItems
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToList();

            var topLevel = navItems.Where(x => x.ParentId == null).ToList();
            var children = navItems.Where(x => x.ParentId != null).GroupBy(x => x.ParentId).ToDictionary(g => g.Key!.Value, g => g.ToList());

            var logoText = _db.SiteSettings.FirstOrDefault(x => x.SettingKey == "LogoText")?.SettingValue ?? "SAFORIX";
            var tagline = _db.SiteSettings.FirstOrDefault(x => x.SettingKey == "Tagline")?.SettingValue ?? "Security Analysis Framework";

            ViewBag.NavTopLevel = topLevel;
            ViewBag.NavChildren = children;
            ViewBag.LogoText = logoText;
            ViewBag.Tagline = tagline;

            return View("Default");
        }
    }
}
