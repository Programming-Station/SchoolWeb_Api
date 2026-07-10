using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("BookCategories")]
    public class BookCategory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>Book | Subject | Digital | Reference</summary>
        [MaxLength(30)]
        public string? CategoryType { get; set; } = "Book";

        [MaxLength(10)]
        public string? ColorCode { get; set; } = "#6366f1";

        public int? ParentCategoryId { get; set; }
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual BookCategory? ParentCategory { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
