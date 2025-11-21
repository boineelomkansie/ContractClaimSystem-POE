// Models/Document.cs
using System.ComponentModel.DataAnnotations;

namespace ContractClaims.Models
{
    public class Document
    {
        public Document() { }

        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string? FilePath { get; set; }

        public int ClaimId { get; set; }
        public Claim? Claim { get; set; }
    }
}
