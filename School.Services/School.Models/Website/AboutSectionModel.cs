using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class AboutSectionModel
    {
        public int? Id { get; set; }

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

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
