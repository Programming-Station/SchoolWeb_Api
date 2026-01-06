using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class ContactInfoModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string OrganizationName { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone1 { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone2 { get; set; }

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
