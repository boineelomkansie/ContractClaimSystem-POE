// Models/Lecturer.cs
using System.ComponentModel.DataAnnotations;

namespace ContractClaims.Models
{
    public class Lecturer
    {
        public Lecturer() { }

        public int Id { get; set; }

        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }
    }
}
