// Controllers/ClaimsController.cs
using ContractClaims.Data;
using ContractClaims.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractClaims.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly long _maxFileBytes = 5 * 1024 * 1024; // 5 MB

        public ClaimsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Submit()
        {
            ViewBag.Contracts = await _db.Contracts.Include(c => c.Lecturer).ToListAsync();
            ViewBag.Lecturers = await _db.Lecturers.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim model, IFormFile? upload)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contracts = await _db.Contracts.ToListAsync();
                ViewBag.Lecturers = await _db.Lecturers.ToListAsync();
                return View(model);
            }

            // ensure total computed server-side (Total is computed property)
            model.SubmittedAt = DateTime.UtcNow;
            model.Status = ClaimStatus.Submitted;

            _db.Claims.Add(model);
            await _db.SaveChangesAsync();

            // upload handling (safe path, unique name)
            if (upload != null && upload.Length > 0)
            {
                if (upload.Length > _maxFileBytes)
                {
                    TempData["Error"] = "Uploaded file too large (max 5MB).";
                    return RedirectToAction(nameof(Track));
                }

                var allowed = new[] { ".pdf", ".docx", ".xlsx" };
                var ext = Path.GetExtension(upload.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    TempData["Error"] = "Invalid file type.";
                    return RedirectToAction(nameof(Track));
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var unique = $"{Guid.NewGuid()}_{Path.GetFileName(upload.FileName)}";
                var filePath = Path.Combine(uploadsFolder, unique);
                await using var stream = System.IO.File.Create(filePath);
                await upload.CopyToAsync(stream);

                var doc = new Document
                {
                    ClaimId = model.Id,
                    FileName = upload.FileName,
                    FilePath = $"/uploads/{unique}"
                };
                _db.Documents.Add(doc);
                await _db.SaveChangesAsync();
            }

            // Automation: auto-verify when criteria met
            if (model.MeetsAutoVerifyRules())
            {
                model.Status = ClaimStatus.Verified;
                await _db.SaveChangesAsync();
                TempData["Success"] = "Claim submitted and auto-verified.";
            }
            else
            {
                TempData["Success"] = "Claim submitted successfully (pending verification).";
            }

            return RedirectToAction(nameof(Track));
        }

        public async Task<IActionResult> UploadDocuments(int id)
        {
            var claim = await _db.Claims.Include(c => c.Documents).Include(c => c.Lecturer).FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocuments(int id, IFormFile file)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            if (file != null && file.Length > 0)
            {
                if (file.Length > _maxFileBytes)
                {
                    TempData["Error"] = "Uploaded file too large (max 5MB).";
                    return RedirectToAction(nameof(Track));
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var unique = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, unique);
                await using var stream = System.IO.File.Create(filePath);
                await file.CopyToAsync(stream);

                var doc = new Document
                {
                    ClaimId = claim.Id,
                    FileName = file.FileName,
                    FilePath = $"/uploads/{unique}"
                };
                _db.Documents.Add(doc);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Track));
        }

        public async Task<IActionResult> Track()
        {
            var items = await _db.Claims.Include(c => c.Lecturer).Include(c => c.Contract).Include(c => c.Documents).OrderByDescending(c => c.SubmittedAt).ToListAsync();
            return View(items);
        }
    }
}
