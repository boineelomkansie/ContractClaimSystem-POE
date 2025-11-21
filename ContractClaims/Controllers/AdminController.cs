// Controllers/AdminController.cs
using ContractClaims.Data;
using ContractClaims.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ContractClaims.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Verify()
        {
            var pending = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Contract)
                .Where(c => c.Status == ClaimStatus.Submitted)
                .OrderBy(c => c.SubmittedAt)
                .ToListAsync();
            return View(pending);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAction(int id, string action)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            if (action == "verify")
                claim.Status = ClaimStatus.Verified;
            else if (action == "reject")
                claim.Status = ClaimStatus.Rejected;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Verify));
        }

        public async Task<IActionResult> Approve()
        {
            var toApprove = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Contract)
                .Where(c => c.Status == ClaimStatus.Verified)
                .OrderBy(c => c.SubmittedAt)
                .ToListAsync();
            return View(toApprove);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAction(int id, string action)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            if (action == "approve")
                claim.Status = ClaimStatus.Approved;
            else if (action == "reject")
                claim.Status = ClaimStatus.Rejected;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Approve));
        }

        // NEW: Export Approved Claims CSV for HR
        public async Task<IActionResult> ExportApprovedCsv()
        {
            var approved = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Contract)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("ClaimId,Lecturer,EmployeeNumber,ContractCode,Month,Hours,Rate,Total,SubmittedAt");
            foreach (var c in approved)
            {
                var lecturerName = c.Lecturer?.FullName?.Replace("\"", "'") ?? "Unknown";
                csv.AppendLine($"{c.Id},\"{lecturerName}\",{c.Lecturer?.EmployeeNumber},{c.Contract?.ContractCode},{c.Month},{c.Hours},{c.HourlyRate},{c.Total},{c.SubmittedAt:O}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "approved-claims.csv");
        }

        // NEW: Generate simple invoice CSV grouped by lecturer
        public async Task<IActionResult> ExportInvoicesCsv()
        {
            var bytes = await Reports.InvoiceHelper.GenerateInvoiceCsv(_db);
            return File(bytes, "text/csv", "invoices.csv");
        }
    }
}
