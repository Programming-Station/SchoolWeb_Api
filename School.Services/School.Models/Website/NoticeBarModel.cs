using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class NoticeBarModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string NoticeText { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string ContactInfo { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
