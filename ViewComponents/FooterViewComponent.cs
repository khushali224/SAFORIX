using Microsoft.AspNetCore.Mvc;
using SAFORIX.Data;

namespace SAFORIX.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly SaforixDbContext _db;

        public FooterViewComponent(SaforixDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var copyright = _db.SiteSettings.FirstOrDefault(x => x.SettingKey == "FooterCopyright")?.SettingValue
                ?? "© 2026 SAFORIX — Security Analysis Framework";
            return View("Default", copyright);
        }
    }
}
