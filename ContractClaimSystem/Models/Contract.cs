using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractClaimSystem.Models
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        [Required]
        [ForeignKey("Lecturer")]
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        [Required]
        [Display(Name = "Contract Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
