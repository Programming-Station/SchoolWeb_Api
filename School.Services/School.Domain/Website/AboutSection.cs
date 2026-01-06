using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class AboutSection : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CompanyNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RegisteredDate { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Location { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
