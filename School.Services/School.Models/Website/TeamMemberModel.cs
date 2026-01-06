using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class TeamMemberModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Designation { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Qualification { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Experience { get; set; } = null!;

        [MaxLength(500)]
        public string? ProfilePhotoUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
