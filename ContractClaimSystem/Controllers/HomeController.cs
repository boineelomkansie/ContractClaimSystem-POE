using Microsoft.AspNetCore.Mvc;
using ContractClaimSystem.Models;
using System.Diagnostics;

namespace ContractClaimSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ✅ Landing page
        public IActionResult Index()
        {
            return View();
        }

        // ✅ About page (optional)
        public IActionResult About()
        {
            ViewData["Message"] = "This Contract Claims Management System allows lecturers to submit and track their monthly claims.";
            return View();
        }

        // ✅ Contact page (optional)
        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact system administration for support.";
            return View();
        }

        // ✅ Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
