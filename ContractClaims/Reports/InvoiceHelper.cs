// Reports/InvoiceHelper.cs
using ContractClaims.Data;
using ContractClaims.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ContractClaims.Reports
{
    public static class InvoiceHelper
    {
        public static async Task<byte[]> GenerateInvoiceCsv(ApplicationDbContext db)
        {
            var approved = await db.Claims.Include(c => c.Lecturer).Where(c => c.Status == ClaimStatus.Approved).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Lecturer,TotalApprovedAmount");
            var groups = approved.GroupBy(c => c.LecturerId);
            foreach (var g in groups)
            {
                var name = g.First().Lecturer?.FullName?.Replace("\"", "'") ?? "Unknown";
                var total = g.Sum(x => x.Total);
                sb.AppendLine($"\"{name}\",{total}");
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
