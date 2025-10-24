using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractClaimSystem.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
