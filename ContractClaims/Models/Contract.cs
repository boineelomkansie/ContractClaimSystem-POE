// Models/Contract.cs
using System.ComponentModel.DataAnnotations;

namespace ContractClaims.Models
{
    public class Contract
    {
        public Contract() { }

        public int Id { get; set; }

        [Required]
        public string ContractCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
    }
}
