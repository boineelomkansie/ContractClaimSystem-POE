using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractClaimSystem.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public string? Notes { get; set; }

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
