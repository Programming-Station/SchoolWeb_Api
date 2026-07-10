using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("BookIssueLogs")]
    public class BookIssueLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; } = null!;

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; } = null!;

        public int? MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public virtual LibraryMember? Member { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime OriginalDueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0;

        public bool FinePaid { get; set; } = false;

        /// <summary>Issued | Returned | Overdue | Renewed | Lost | Damaged</summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Issued";

        [MaxLength(50)]
        public string? BookConditionOnIssue { get; set; } = "Good";

        [MaxLength(50)]
        public string? BookConditionOnReturn { get; set; }

        public bool IsRenewed { get; set; } = false;
        public int RenewalCount { get; set; } = 0;

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
