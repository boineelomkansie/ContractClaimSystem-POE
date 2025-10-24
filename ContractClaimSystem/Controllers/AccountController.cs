using ContractClaimSystem.Data;
using ContractClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContractClaimSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "FullName");
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(int lecturerId)
        {
            var lecturer = _context.Lecturers.Find(lecturerId);
            if (lecturer == null)
            {
                TempData["ErrorMessage"] = "Invalid lecturer selected.";
                return RedirectToAction(nameof(Login));
            }

            // Store logged-in lecturer in session
            HttpContext.Session.SetInt32("LecturerId", lecturerId);
            HttpContext.Session.SetString("LecturerName", lecturer.FullName);

            return RedirectToAction("TrackClaims", "Claim");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
