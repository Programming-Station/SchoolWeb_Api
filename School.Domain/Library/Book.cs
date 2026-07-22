using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Library
{
    [Table("LibraryBooks", Schema = "Library")]
    public class Book : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        // ── Core Identifiers ──────────────────────────────────────────────
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? ISBN { get; set; }

        [MaxLength(50)]
        public string? AccessionNumber { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [MaxLength(200)]
        public string? QRCode { get; set; }

        // ── Book Details ──────────────────────────────────────────────────
        [MaxLength(20)]
        public string? Edition { get; set; }

        [MaxLength(20)]
        public string? Volume { get; set; }

        [MaxLength(50)]
        public string? Language { get; set; } = "English";

        [MaxLength(100)]
        public string? SubjectCategory { get; set; }

        // ── Author / Publisher / Vendor ───────────────────────────────────
        [Required, MaxLength(150)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? CoAuthor { get; set; }

        public int? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual BookAuthor? BookAuthor { get; set; }

        [MaxLength(150)]
        public string? Publisher { get; set; }

        public int? PublisherId { get; set; }
        [ForeignKey(nameof(PublisherId))]
        public virtual BookPublisher? BookPublisher { get; set; }

        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual BookVendor? BookVendor { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual BookCategory? Category { get; set; }

        // ── Book Type ─────────────────────────────────────────────────────
        /// <summary>TextBook | Reference | General | Digital | Magazine | Journal | Newspaper</summary>
        [MaxLength(30)]
        public string? BookType { get; set; } = "General";

        // ── Status ────────────────────────────────────────────────────────
        /// <summary>Available | Issued | Reserved | Lost | Damaged | Withdrawn | Inactive</summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Available";

        // ── Purchase Info ─────────────────────────────────────────────────
        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; } = 0;

        // ── Location ──────────────────────────────────────────────────────
        [MaxLength(20)]
        public string? Shelf { get; set; }

        [MaxLength(20)]
        public string? Rack { get; set; }

        [MaxLength(20)]
        public string? Row { get; set; }

        [MaxLength(20)]
        public string? Cupboard { get; set; }

        [MaxLength(50)]
        public string? RackLocation { get; set; }

        // ── Stock ─────────────────────────────────────────────────────────
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;
        public int MinimumStock { get; set; } = 1;
        public int MaximumStock { get; set; } = 100;

        // ── Media ─────────────────────────────────────────────────────────
        [MaxLength(500)]
        public string? BookImagePath { get; set; }

        [MaxLength(500)]
        public string? PdfAttachmentPath { get; set; }

        // ── Metadata ──────────────────────────────────────────────────────
        [MaxLength(500)]
        public string? Keywords { get; set; }

        public string? Description { get; set; }

        // ── Tenant ────────────────────────────────────────────────────────
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
