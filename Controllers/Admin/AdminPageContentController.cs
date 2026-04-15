using Microsoft.AspNetCore.Mvc;

namespace SAFORIX.Controllers.Admin
{
    // This controller is replaced by AdminHomeContentController,
    // AdminAboutUsContentController, and AdminThreatContentController.
    // Kept as a redirect stub so old bookmarked URLs still work.
    public class AdminPageContentController : Controller
    {
        public IActionResult Index(string? pageKey)
        {
            return pageKey switch
            {
                "Home"    => RedirectToAction("Index", "AdminHomeContent"),
                "AboutUs" => RedirectToAction("Index", "AdminAboutUsContent"),
                "Threat"  => RedirectToAction("Index", "AdminThreatContent"),
                _         => RedirectToAction("Index", "AdminDashboard")
            };
        }
    }
}
