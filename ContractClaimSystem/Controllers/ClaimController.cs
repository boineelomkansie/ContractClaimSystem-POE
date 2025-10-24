using ContractClaimSystem.Data;
using ContractClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContractClaimsSystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Contract)
                .Include(c => c.Documents)
                .ToListAsync();
            return View(claims);
        }
        // ✅ GET: Track Claims (for lecturer/user)
        public async Task<IActionResult> TrackClaims()
        {
            int? lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (lecturerId == null)
            {
                TempData["Message"] = "You must log in first.";
                return RedirectToAction("Login", "Account");
            }

            var claims = await _context.Claims
                .Where(c => c.LecturerId == lecturerId)
                .Include(c => c.Contract)      // Include the related contract
                .Include(c => c.Documents)     // Include uploaded documents
                .ToListAsync();

            return View(claims);
        }

        // GET: Claim/Create
        public IActionResult Create()
        {
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "FullName");
            ViewData["ContractId"] = new SelectList(_context.Contracts, "ContractId", "Title");
            return View();
        }

        // POST: Claim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("LecturerId,ContractId,Month,HoursWorked,HourlyRate,Notes")] Claim claim,
    List<IFormFile> files) // new parameter for uploaded files
        {
            if (ModelState.IsValid)
            {
                claim.Status = ClaimStatus.Pending;
                claim.DateSubmitted = DateTime.Now;
                _context.Add(claim);
                await _context.SaveChangesAsync();

                // Handle file uploads
                if (files != null && files.Count > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Make filename unique to avoid overwriting
                            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Save document record in DB
                            var document = new Document
                            {
                                ClaimId = claim.ClaimId,
                                FileName = file.FileName,               // Original filename
                                FilePath = "/uploads/" + uniqueFileName // Public path
                            };

                            _context.Documents.Add(document);
                        }
                    }
                    await _context.SaveChangesAsync();
                }



                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "FullName", claim.LecturerId);
            ViewData["ContractId"] = new SelectList(_context.Contracts, "ContractId", "Title", claim.ContractId);
            return View(claim);
        }


        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // ✅ Reject Claim (new)
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
