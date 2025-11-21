// Models/Claim.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractClaims.Models
{
    public partial class Claim
    {
        public Claim() { }

        public int Id { get; set; }

        [Required]
        public int LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        [Required]
        [RegularExpression(@"\d{4}-\d{2}")]
        public string Month { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)]
        public double Hours { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal HourlyRate { get; set; }

        public decimal Total => (decimal)Hours * HourlyRate;

        public string? Comments { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public List<Document> Documents { get; set; } = new();
    }
}
