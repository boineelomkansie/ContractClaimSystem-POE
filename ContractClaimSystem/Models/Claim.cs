using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractClaimSystem.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        // Foreign keys
        [Required]
        [Display(Name = "Lecturer")]
        public int LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }  // optional for EF

        [Required]
        [Display(Name = "Contract")]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }  // optional for EF

        [Required]
        public string Month { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hours worked must be a positive number")]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be a positive number")]
        public decimal HourlyRate { get; set; }

        public string? Notes { get; set; }

        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
